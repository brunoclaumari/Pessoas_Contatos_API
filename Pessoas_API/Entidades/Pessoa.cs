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
        /// Nome da pessoa
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public List<Contato> Contatos { get; set; } = new List<Contato>();
    }
}
