using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRefeicao.Data.Models
{
    [Table("Categoria")]
    public class CategoriaModel : BaseModel
    {
        [Required]
        [Column("RestauranteId")]
        public Guid RestauranteId { get; set; }

        [Required]
        [Column("Nome")]
        public string? Nome { get; set; }

        [Required]
        [Column("OrdemExibicao")]
        [Range(0, int.MaxValue)]
        public int OrdemExibicao { get; set; }

        [Required]
        [Column("Ativo")]
        public bool Ativo { get; set; }


        [ForeignKey("RestauranteId")]
        public virtual RestauranteModel? Restaurante { get; set; }
    }
}
