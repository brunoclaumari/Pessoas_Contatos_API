﻿using Microsoft.AspNetCore.Http;
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

        /// <summary>
        /// Busca todos os contatos (paginada)
        /// </summary>
        /// <param name="pagina"></param>
        /// <param name="totalPorPagina"></param>
        /// <returns></returns>
        // GET: api/<ContatoController>
        [HttpGet]        
        public async Task<IActionResult> GetAllContatosPagedAsync([FromQuery] int pagina = 1, [FromQuery] int totalPorPagina = 10)
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

        /// <summary>
        /// Busca todos os contatos de uma pessoa pelo id dela
        /// </summary>
        /// <param name="pessoaId"></param>
        /// <returns></returns>
        // GET api/<ContatoController>/5
        [HttpGet("pessoa/{pessoaId}")]
        public async Task<IActionResult> GetAllContatosByPessoaId(int pessoaId)
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


        /// <summary>
        /// Cria um novo contato
        /// </summary>
        /// <param name="entradaContato"></param>
        /// <returns></returns>
        // POST api/<ContatoController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Contato entradaContato)
        {
            List<string> listaErros = new List<string>();
            try
            {
                _repo.Add(entradaContato);
                bool deuCerto = await _repo.SaveChangesAsync();
                if (deuCerto)
                {
                    return Created($"api/[controller]", entradaContato);
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
                    msg = "Ocorreu um erro ao salvar os dados de contato.";
                    listaErros.Add(msg);
                }

                
                Console.WriteLine(msg);
                return UnprocessableEntity(new { Erros = listaErros });
            }
            catch (Exception e)
            {                
                return BadRequest(new { Message = "Ocorreu um erro inesperado. Verifique os dados de entrada" });
            }

        }

        /// <summary>
        /// Atualiza um contato
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entradaContato"></param>
        /// <returns></returns>
        // PUT api/<ContatoController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Contato entradaContato)
        {
            List<string> listaErros = new List<string>();
            try
            {
                _repo.IniciaTransacaoAsync();
                var contato = await _repo.GetContatoByIdWithTrackingAsync(id);
                if (contato is null)
                {
                    _repo.CancelaTransacaoAsync();
                    return UnprocessableEntity(new { Erros = $"Pessoa id = {id} não encontrada!!" });
                }
                if (_repo != null && await ((Repository)_repo).ExisteEmailRepetido(entradaContato, listaErros, false))
                {
                    _repo.CancelaTransacaoAsync();
                    return UnprocessableEntity(new { Erros = listaErros });
                }
                
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
        /// Apaga um contato pelo seu id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
