using Microsoft.EntityFrameworkCore;
using Pessoas_API.Entidades;

namespace Pessoas_API.Context
{
    /// <summary>
    /// 
    /// </summary>
    public class PessoaContext : DbContext
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public PessoaContext(DbContextOptions<PessoaContext> options)
        : base(options)
        {
        }

        public virtual DbSet<Pessoa> Pessoas { get; set; }
        public virtual DbSet<Contato> Contatos { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Pessoa>()
                .HasData(new List<Pessoa>(){
                    new Pessoa(1,"Maria Santana", "mariasanta@gmail.com"),
                    new Pessoa(2, "João Carlos da Costa", "jcarlos@gmail.com"),
                    new Pessoa(3, "Ricardo Pereira dos Santos", "ricardopr@gmail.com"),
                });

            builder.Entity<Contato>()
                .HasData(new List<Contato>(){
                    new Contato(1,"Joana Santana","(11) 91111-1111",string.Empty, "joana@gmail.com", 1),
                    new Contato(2,"Raquel Santana","(11) 92222-2222",string.Empty, "raquelst@gmail.com", 1),
                    new Contato(3,"João Alberto Santana","(11) 93333-3333",string.Empty, "jalberto@gmail.com", 1),

                    new Contato(4,"Vinícius Ribeiro","(11) 94444-4444",string.Empty, "viniribeiro@gmail.com", 2),
                    new Contato(5,"José Vieira","(11) 95555-5555",string.Empty, "josevieira@gmail.com", 2),

                    new Contato(6,"Leila Sanches","(11) 96666-6666","(11) 96666-6666", "leilasanches@gmail.com", 3),
                    new Contato(7,"Luana de Freitas","(11) 97777-7777",string.Empty, "luanafr@gmail.com", 3),

                });

        }
    }
}
