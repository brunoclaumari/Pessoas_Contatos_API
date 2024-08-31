using Microsoft.AspNetCore.Mvc;
using Pessoas_API.a_Repository;
using Pessoas_API.Entidades;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pessoas_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoaController : ControllerBase
    {

        private readonly IRepository _repo;

        public PessoaController(IRepository repo)
        {
            _repo = repo;
        }



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

        // GET api/<PessoaController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PessoaController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PessoaController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PessoaController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
