using System.ComponentModel.DataAnnotations.Schema;

namespace Pessoas_API.Entidades
{
    /// <summary>
    /// 
    /// </summary>
    [Table("tbContato")]
    public class Contato : EntidadePadrao
    {        
        /// <summary>
        /// 
        /// </summary>
        [Column("nome")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public long PessoaId { get;set; }
         
    }
}
