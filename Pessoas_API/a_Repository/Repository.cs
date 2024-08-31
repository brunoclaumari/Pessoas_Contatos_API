using Microsoft.EntityFrameworkCore;
using Pessoas_API.Context;
using Pessoas_API.Entidades;

namespace Pessoas_API.a_Repository
{
    public class Repository : IRepository
    {
        private readonly PessoaContext _context;

        public Repository(PessoaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adiciona uma entidade
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Add<T>(T entity) where T : EntidadePadrao
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : EntidadePadrao
        {
            _context.Remove(entity);
        }

        /// <summary>
        /// Lista todas as pessoas com seus contatos
        /// </summary>
        /// <param name="incluiContatos"></param>
        /// <returns></returns>
        public async Task<List<Pessoa>> GetAllPessoasAsync(bool incluiContatos = true)
        {
            IQueryable<Pessoa> query = _context.Pessoas;

            if (incluiContatos)
            {
                query = query.Include(p => p.Contatos);
            }

            query = query.AsNoTracking().OrderBy(a => a.Id);

            return await query.ToListAsync();
        }

        /// <summary>
        /// Retorna uma pessoa e todos seus contatos por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Pessoa?> GetPessoaByIdAsync(int id)
        {
            return await _context.Pessoas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Salva ou atualiza os dados
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="fazRegistroNovo"></param>
        public void SalvaOuAtualiza<T>(T entity, bool fazRegistroNovo) where T : EntidadePadrao
        {
            if (fazRegistroNovo)
            {
                Add(entity);
            }
            else
                Update(entity);
        }

        public void Update<T>(T entity) where T : EntidadePadrao
        {
            throw new NotImplementedException();
        }
    }
}
