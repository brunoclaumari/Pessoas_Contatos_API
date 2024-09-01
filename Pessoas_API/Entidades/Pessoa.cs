using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pessoas_API.Entidades
{
    /// <summary>
    /// 
    /// </summary>
    [Table("tbPessoa")]
    public class Pessoa : EntidadePadrao
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nome"></param>
        /// <param name="email"></param>
        public Pessoa(long id, string nome, string email):base(id)
        {
            Nome = nome;
            Email = email;            
        }


        /// <summary>
        /// Nome da pessoa
        /// </summary>
        [Column("nome")]
        public string Nome { get; set; } = string.Empty;


        /// <summary>
        /// Email da pessoa
        /// </summary>
        //[RegularExpression(@"b[A-Z0-9._%-]+@[A-Z0-9.-]+.[A-Z]{2,4}b", ErrorMessage = "E-mail em formato inválido.")]
        [Column("email")]
        [EmailAddress(ErrorMessage = "E-mail em formato inválido.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public List<Contato> Contatos { get; set; } = new List<Contato>();
    }
}
