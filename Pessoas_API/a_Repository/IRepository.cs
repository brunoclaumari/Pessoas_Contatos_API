using Pessoas_API.Entidades;

namespace Pessoas_API.a_Repository
{
    public interface IRepository
    {
        void Add<T>(T entity) where T : EntidadePadrao;

        void Update<T>(T entity) where T : EntidadePadrao;

        void Delete<T>(T entity) where T : EntidadePadrao;

        void SalvaOuAtualiza<T>(T entity, bool fazRegistroNovo) where T : EntidadePadrao;

        Task<List<Pessoa>> GetAllPessoasAsync(bool incluiContatos = false);

        Task<Pessoa> GetPessoaByIdAsync(int id);
    }
}
