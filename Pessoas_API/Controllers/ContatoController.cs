using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pessoas_API.a_Repository;
using Pessoas_API.Entidades;
using Pessoas_API.Helpers;

namespace Pessoas_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContatoController : ControllerBase
    {
        private readonly IRepository _repo;

        public ContatoController(IRepository repo)
        {
            _repo = repo;
        }

        // GET: api/<ContatoController>
        [HttpGet]        
        public async Task<IActionResult> GetAllPagedAsync([FromQuery] int pagina, [FromQuery] int totalPorPagina)
        {
            try
            {
                var retorno = await _repo.GetAllContatosPagedAsync(pagina, totalPorPagina);
                return Ok(retorno);
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "Ocorreu um erro ao obter os dados" });
            }
        }

        // GET api/<ContatoController>/5
        [HttpGet("pessoa/{pessoaId}")]
        public async Task<IActionResult> GetByPessoaId(int pessoaId)
        {
            try
            {
                var retorno = await _repo.GetAllContatosPessoaIdAsync(pessoaId);
                if (retorno == null)
                {
                    return NotFound(new { Erros = $"Não existe contatos para Pessoa com id {pessoaId}." });
                }

                return Ok(retorno);
            }
            catch (Exception e)
            {
                return BadRequest(new { Erros = "Ocorreu um erro ao obter os dados" });
            }
            
        }

        // PUT api/<ContatoController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Contato entradaContato)
        {
            List<string> listaErros = new List<string>();
            try
            {

                var contato = await _repo.GetContatoByIdWithTrackingAsync(id);
                if (contato is null)
                {                    
                    return UnprocessableEntity(new { Erros = $"Pessoa id = {id} não encontrada!!" });
                }
                if (_repo != null && await ((Repository)_repo).ExisteEmailRepetido(entradaContato, listaErros, false))
                {                    
                    return UnprocessableEntity(new { Erros = listaErros });
                }
                _repo.IniciaTransacaoAsync();

                var helper = new PessoaHelper();
                
                await helper.TransfereEntradaParaContato(entradaContato, contato);
                _repo.Update(contato);
                if (await _repo.SaveChangesAsync())
                {
                    _repo.ConfirmaTransacaoAsync();
                    return Ok(new { FoiSucesso = true, Message = $"A pessoa {contato.Nome} foi alterada com sucesso!" });
                }
                else
                {
                    _repo.CancelaTransacaoAsync();
                    return BadRequest(new { Erros = $"Não foi possível alterar a pessoa id: {id}" });
                }

            }
            catch (DbUpdateException ex)
            {
                string msg = string.Empty;
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.Message.Contains("IX_tbPessoa_email"))
                    {
                        listaErros.Add($"O email {entradaContato.Email} já está em uso para alguma pessoa. Por favor, insira um email diferente.");
                    }
                    if (ex.InnerException.Message.Contains("IX_tbContato_email"))
                    {
                        string emails = entradaContato.Email;                        

                        msg = "Um ou mais emails em seus contatos já foram cadastrados em outros contatos.\n" + emails;
                        listaErros.Add(msg);
                    }
                }
                else
                {
                    msg = "Ocorreu um erro ao salvar os dados da Pessoa.";
                    listaErros.Add(msg);
                }

                Console.WriteLine(msg);
                return UnprocessableEntity(new { Erros = listaErros });
            }
            catch (Exception e)
            {
                _repo.CancelaTransacaoAsync();
                return BadRequest(new { Message = "Ocorreu um erro inesperado. Verifique os dados de entrada" });
            }
        }

        // DELETE api/<ContatoController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            List<string> listaErros = new List<string>();
            /*
             */
            try
            {
                var contato = await _repo.GetContatoByIdAsNoTrackingAsync(id);
                if (contato is null)
                {
                    listaErros.Add($"Contato id = {id} não encontrado!!");
                    return UnprocessableEntity(new { Erros = listaErros });
                }

                _repo.Delete(contato);
                if (await _repo.SaveChangesAsync())
                {
                    return Ok(new { FoiSucesso = true, Message = $"O contato {contato.Nome} foi deletado com sucesso!" });
                }
                else
                {
                    listaErros.Add($"Não foi possível excluir o contato id: {id}");
                    return BadRequest(new { Erros = listaErros });
                }

            }
            catch (Exception e)
            {
                listaErros.Add("Ocorreu um erro inesperado. Verifique os dados de entrada");
                return BadRequest(new { Erros = listaErros });
            }
        }
    }
}
