using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pessoas_API.a_Repository;
using Pessoas_API.Entidades;
using Pessoas_API.Helpers;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pessoas_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PessoaController : ControllerBase
    {

        private readonly IRepository _repo;

        public PessoaController(IRepository repo)
        {
            _repo = repo;
        }        

        /// <summary>
        /// Busca todas as pessoas cadastradas (com contatos)
        /// </summary>
        /// <returns></returns>
        // GET: api/<PessoaController>
        [HttpGet]
        //public Task<List<Pessoa>> GetAllPessoa()
        public async Task<IActionResult> GetAllPessoaAsync()
        {
            try
            {
                var retorno = await _repo.GetAllPessoasAsync();
                return Ok(retorno);
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "Ocorreu um erro ao obter os dados" });
            }
        }

        /// <summary>
        /// Busca todas as pessoas cadastradas (sem contatos)
        /// </summary>
        /// <returns></returns>
        // GET: api/<PessoaController>/semcontatos
        [HttpGet("semcontatos")]
        //public Task<List<Pessoa>> GetAllPessoa()
        public async Task<IActionResult> GetAllPessoaSemContatoAsync()
        {
            try
            {
                var retorno = await _repo.GetAllPessoasAsync(false);
                return Ok(retorno);
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "Ocorreu um erro ao obter os dados" });
            }
        }

        /// <summary>
        /// Busca pessoa cadastrada pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/<PessoaController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var retorno = await _repo.GetPessoaByIdAsNoTrackingAsync(id);
                if (retorno == null)
                {
                    return NotFound(new { Erros = $"Pessoa com id {id} não encontrada" });
                }

                return Ok(retorno);
            }
            catch (Exception e)
            {
                return BadRequest(new { Erros = "Ocorreu um erro ao obter os dados" });
            }
            //return "value";
        }

        /// <summary>
        /// Cadastra uma pessoa com seus contatos vinculados
        /// </summary>
        /// <param name="pessoa"></param>
        /// <returns></returns>
        // POST api/<PessoaController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Pessoa pessoa)
        {
            List<string> listaErros = new List<string>();
            try
            {               

                _repo.Add(pessoa);
                bool deuCerto = await _repo.SaveChangesAsync();
                if (deuCerto)
                {
                    return Created($"api/[controller]", pessoa);
                }
                else
                {
                    listaErros.Add("Não foi possível salvar o registro");                    
                    return BadRequest(new { Erros = listaErros });
                }

            }
            catch (DbUpdateException ex)
            {
                string msg = string.Empty;                
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.Message.Contains("IX_tbPessoa_email"))
                    {
                        listaErros.Add($"O email {pessoa.Email} já está em uso para alguma pessoa. Por favor, insira um email diferente.");                        
                    }
                    if (ex.InnerException.Message.Contains("IX_tbContato_email"))
                    {
                        string emails = string.Empty;
                        pessoa.Contatos.ForEach(c => { emails += $"{c.Email} "; });

                        msg = "Um ou mais emails em seus contatos já foram cadastrados em outros contatos.\n" + emails;
                        listaErros.Add(msg);
                    }
                }
                else
                {
                    msg = "Ocorreu um erro ao salvar os dados da Pessoa.";
                    listaErros.Add(msg);
                }

                _repo.CancelaTransacaoAsync();
                Console.WriteLine(msg);
                return UnprocessableEntity(new { Erros = listaErros });
            }
            catch (Exception e)
            {
                return BadRequest(new { Erro = e.Message, Message = "Ocorreu um erro ao obter os dados" });
            }
        }

        /// <summary>
        /// Atualiza os dados de pessoa cadastrada junto com seus contatos
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entradaPessoa"></param>
        /// <returns></returns>
        // PUT api/<PessoaController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Pessoa entradaPessoa)
        {
            List<string> listaErros=new List<string>();
            try
            {
                
                var pessoa = await _repo.GetPessoaByIdWithTrackingAsync(id);
                if (pessoa is null)
                {                    
                    return UnprocessableEntity(new { Erros = $"Pessoa id = {id} não encontrada!!" });
                }
                if(_repo != null && await ((Repository)_repo).ExisteEmailRepetido(entradaPessoa, listaErros))
                {                    
                    return UnprocessableEntity(new { Erros = listaErros });
                }
                _repo.IniciaTransacaoAsync();

                var helper = new PessoaHelper();
                //await _repo.TransfereEntradaParaEntidadeParaUpdate(entradaPessoa, pessoa);
                await helper.TransfereEntradaParaPessoa(entradaPessoa, pessoa);
                _repo.Update(pessoa);
                if (await _repo.SaveChangesAsync())
                {
                    _repo.ConfirmaTransacaoAsync();
                    return Ok(new  { FoiSucesso = true, Message = $"A pessoa {pessoa.Nome} foi alterada com sucesso!"});
                }
                else
                {
                    _repo.CancelaTransacaoAsync();
                    return BadRequest(new  {  Erros = $"Não foi possível alterar a pessoa id: {id}" });
                }

            }
            catch (DbUpdateException ex)
            {
                string msg = string.Empty;
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.Message.Contains("IX_tbPessoa_email"))
                    {
                        listaErros.Add($"O email {entradaPessoa.Email} já está em uso para alguma pessoa. Por favor, insira um email diferente.");
                    }
                    if (ex.InnerException.Message.Contains("IX_tbContato_email"))
                    {
                        string emails = string.Empty;
                        entradaPessoa.Contatos.ForEach(c => { emails += $"{c.Email} "; });

                        msg = "Um ou mais emails em seus contatos já foram cadastrados em outros contatos.\n" + emails;
                        listaErros.Add(msg);
                    }
                }
                else
                {
                    msg = "Ocorreu um erro ao salvar os dados da Pessoa.";
                    listaErros.Add(msg);
                }
                _repo.CancelaTransacaoAsync();
                Console.WriteLine(msg);
                return UnprocessableEntity(new { Erros = listaErros });
            }
            catch (Exception e)
            {
                _repo.CancelaTransacaoAsync();
                return BadRequest(new { Message = "Ocorreu um erro inesperado. Verifique os dados de entrada" });
            }
        }

        /// <summary>
        /// Apaga uma pessoa cadastrada e todos os contatos
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE api/<PessoaController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            List<string> listaErros = new List<string>();
            /*
             */
            try
            {
                var pessoa = await _repo.GetPessoaByIdAsNoTrackingAsync(id);
                if (pessoa is null)
                {
                    listaErros.Add($"Pessoas id = {id} não encontrado!!");
                    return UnprocessableEntity(new { Erros = listaErros });
                }

                _repo.Delete(pessoa);
                if (await _repo.SaveChangesAsync())
                {
                    return Ok(new  { FoiSucesso = true,  Message = $"A pessoa {pessoa.Nome} foi deletada com sucesso!" });
                }
                else
                {
                    listaErros.Add($"Não foi possível excluir a pessoa id: {id}");
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
