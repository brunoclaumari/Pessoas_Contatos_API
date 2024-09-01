using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pessoas_API.DTOs
{
    /// <summary>
    /// 
    /// </summary>
    [Table("tbContato")]
    public class ContatoDTO
    {

        public long Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nome"></param>
        /// <param name="telefone"></param>
        /// <param name="whatsapp"></param>
        /// <param name="email"></param>
        /// <param name="pessoaId"></param>
        public ContatoDTO(long id, string nome, string telefone, string whatsapp, string email, long pessoaId)            
        {
            Id = id;
            Nome = nome;
            Telefone = telefone;
            Whatsapp = whatsapp;
            Email = email;
            PessoaId = pessoaId;
        }

        /// <summary>
        /// 
        /// </summary>        
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Telefone de contato
        /// </summary>        
        [MaxLength(15)]
        [Required(ErrorMessage = "Campo \"telefone\" é obrigatório!")]
        [RegularExpression(@"^\([1-9]{2}\) 9[0-9]{4}-[0-9]{4}$", ErrorMessage = "O número de telefone celular deve estar no formato (XX) 9XXXX-XXXX")]
        public string Telefone { get; set; } = string.Empty;

        /// <summary>
        /// Whatsapp do contato
        /// </summary>        
        [MaxLength(15)]        
        [RegularExpression(@"^\([1-9]{2}\) 9[0-9]{4}-[0-9]{4}$", ErrorMessage = "O número de whatsapp deve estar no formato (XX) 9XXXX-XXXX")]
        public string Whatsapp { get; set; } = string.Empty;

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
        [Column("pessoa_id")]
        public long PessoaId { get;set; }
         
    }
}
