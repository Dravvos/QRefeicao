using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRefeicao.Data.Models
{
    [Table("TabelaGeralItem")]
    public class TabelaGeralItemModel : BaseModel
    {
        [Required]
        [Column("TabelaGeralId")]
        public Guid TabelaGeralId { get; set; }

        [Column("Sigla")]
        [StringLength(5)]
        [Required]
        public string? Sigla { get; set; }

        [Column("Descricao")]
        [StringLength(150)]
        [Required]
        public string? Descricao { get; set; }

        [ForeignKey("TabelaGeralId")]
        public virtual TabelaGeralModel? TabelaGeral { get; set; }
    }
}
