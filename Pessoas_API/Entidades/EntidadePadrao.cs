using System.ComponentModel.DataAnnotations.Schema;

namespace Pessoas_API.Entidades
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class EntidadePadrao
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        protected EntidadePadrao(long id)
        {
            Id = id;
        }

        /// <summary>
        /// 
        /// </summary>
        [Column("id")]
        public virtual long Id { get; set; }
        
    }
}
