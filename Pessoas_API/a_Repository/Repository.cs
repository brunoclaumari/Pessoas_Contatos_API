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

        public void IniciaTransacaoAsync()
        {
            _context.Database.BeginTransactionAsync();
        }

        public void ConfirmaTransacaoAsync()
        {
            _context.Database.CommitTransactionAsync();
        }

        public void CancelaTransacaoAsync()
        {
            _context.Database.RollbackTransactionAsync();
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

        public void Update<T>(T entity) where T : EntidadePadrao
        {
            _context.Update(entity);
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
        /// <param name="incluiContatos"></param>
        /// <returns></returns>
        public async Task<Pessoa?> GetPessoaByIdAsNoTrackingAsync(int id, bool incluiContatos = true)
        {
            //var pessoa = await _context.Pessoas.Include(ct => ct.Contatos).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            IQueryable<Pessoa> query = _context.Pessoas;

            if (incluiContatos)
            {
                query = query.AsNoTracking().Include(p => p.Contatos);
            }

            var pessoa = await query.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            return pessoa;
        }

        public async Task<Pessoa?> GetPessoaByIdWithTrackingAsync(int id, bool incluiContatos = true)
        {            
            IQueryable<Pessoa> query = _context.Pessoas;

            if (incluiContatos)
            {
                query = query.Include(p => p.Contatos);
            }

            var pessoa = await query.FirstOrDefaultAsync(x => x.Id == id);

            return pessoa;
        }


        //

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

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entrada"></param>
        /// <param name="entidadeExistente"></param>
        public async Task TransfereEntradaParaEntidadeParaUpdate<T>(T entrada, T entidadeExistente) where T : EntidadePadrao
        {
            _context.Entry(entidadeExistente).CurrentValues.SetValues(entrada);
            //_context.Entry(entidadeExistente).State = EntityState.Modified;

            //_context.SaveChangesAsync();
            //Update(entidadeExistente);
        }

        public async Task<bool> ExisteEmailRepetido(Pessoa pessoa, List<string> listaErros)
        {
            //listaErros = new List<string>();
            var contatosPessoaEnviada = pessoa.Contatos.ToList();
            var contatosSalvos = _context.Contatos.ToList();
            var pessoasSalvas = _context.Pessoas.ToList();

            if (pessoasSalvas != null && pessoasSalvas.Count > 0)
            {
                var pessoasMesmoEmail = pessoasSalvas.FindAll(x => x.Email == pessoa.Email && x.Id != pessoa.Id);
                if (pessoasMesmoEmail != null && pessoasMesmoEmail.Count > 0)
                {
                    listaErros.Add($"O email {pessoa.Email} já existe para outra pessoa.");
                }
            }

            var interseccaoPorEmailPessoaDif = contatosSalvos.IntersectBy(contatosPessoaEnviada.Select(x =>
                                                  x.Email), x => x.Email).Where(x => x.PessoaId != pessoa.Id).ToList();            
            

            if(interseccaoPorEmailPessoaDif != null && interseccaoPorEmailPessoaDif.Count > 0)
            {
                interseccaoPorEmailPessoaDif.ForEach(e =>
                {
                    listaErros.Add($"O email {e.Email} já existe em contatos de outra pessoa");
                });
            }

            var contatosSalvosPessoaAtual = contatosSalvos.FindAll(x => x.PessoaId == pessoa.Id);

            if(contatosSalvosPessoaAtual != null && contatosSalvosPessoaAtual.Count > 0)
            {
                var interseccaoPorEmailContatosMesmaPessoa = contatosSalvosPessoaAtual
                                        .FindAll(x => contatosPessoaEnviada.Any(k => (k.Email == x.Email && x.Id != k.Id)));

                if (interseccaoPorEmailContatosMesmaPessoa != null && interseccaoPorEmailContatosMesmaPessoa.Count > 0 && interseccaoPorEmailContatosMesmaPessoa.FirstOrDefault() != null)
                {
                    var email = interseccaoPorEmailContatosMesmaPessoa.FirstOrDefault().Email;
                    listaErros.Add($"O email {email} já existe em seus próprios contatos");                   
                }
            }         


            return listaErros.Count > 0;
        }
       
    }
}
