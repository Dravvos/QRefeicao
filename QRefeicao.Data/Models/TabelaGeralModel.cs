using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRefeicao.Data.Models
{
    [Table("TabelaGeral")]
    public class TabelaGeralModel : BaseModel
    {
        [Column("Nome")]
        [Required]
        public string Nome { get; set; } = null!;
        [Column("Descricao")]
        [Required]
        public string Descricao { get; set; } = null!;
    }
}
