using Pessoas_API.Entidades;

namespace Pessoas_API.a_Repository
{
    public interface IRepository
    {
        void Add<T>(T entity) where T : EntidadePadrao;

        void Update<T>(T entity) where T : EntidadePadrao;

        void Delete<T>(T entity) where T : EntidadePadrao;

        void SalvaOuAtualiza<T>(T entity, bool fazRegistroNovo) where T : EntidadePadrao;

        Task<List<Pessoa>> GetAllPessoasAsync(bool incluiContatos = true);

        Task<Pessoa> GetPessoaByIdAsNoTrackingAsync(int id, bool incluiContatos = true);

        Task<Pessoa> GetPessoaByIdWithTrackingAsync(int id, bool incluiContatos = true);

        Task TransfereEntradaParaEntidadeParaUpdate<T>(T entrada, T entidadeExistente) where T : EntidadePadrao;

        Task<bool> SaveChangesAsync();

        void IniciaTransacaoAsync();

        void ConfirmaTransacaoAsync();

        void CancelaTransacaoAsync();
    }
}
