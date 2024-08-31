using Pessoas_API.Entidades;

namespace Pessoas_API.a_Repository
{
    public class Repository : IRepository
    {   

        public void Add<T>(T entity) where T : EntidadePadrao
        {
            throw new NotImplementedException();
        }

        public void Delete<T>(T entity) where T : EntidadePadrao
        {
            throw new NotImplementedException();
        }

        public Task<List<Pessoa>> GetAllPessoasAsync(bool incluiContatos = false)
        {
            throw new NotImplementedException();
        }

        public Task<Pessoa> GetPessoaByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void SalvaOuAtualiza<T>(T entity, bool fazRegistroNovo) where T : EntidadePadrao
        {
            throw new NotImplementedException();
        }

        public void Update<T>(T entity) where T : EntidadePadrao
        {
            throw new NotImplementedException();
        }
    }
}
