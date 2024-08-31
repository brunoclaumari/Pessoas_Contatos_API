using Microsoft.EntityFrameworkCore;
using Pessoas_API.Entidades;

namespace Pessoas_API.Context
{
    public class PessoaContext : DbContext
    {
        

        public PessoaContext(DbContextOptions<PessoaContext> options)
        : base(options)
        {
        }

        public virtual DbSet<Pessoa> Pessoas { get; set; }
        public virtual DbSet<Contato> Contatos { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {

        }
    }
}
