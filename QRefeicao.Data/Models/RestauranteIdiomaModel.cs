using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRefeicao.Data.Models
{
    [Table("RestauranteIdioma")]
    public class RestauranteIdiomaModel:BaseModel
    {
        [Required]
        [Column("RestauranteId")]
        public Guid RestauranteId { get; set; }

        [Required]
        [Column("IdTGIdioma")]
        public Guid IdTGIdioma { get; set; }

        [ForeignKey("RestauranteId")]
        public virtual RestauranteModel? Restaurante { get; set; }

        [ForeignKey("IdTGIdioma")]
        public virtual TabelaGeralItemModel? Idioma { get; set; }
    }
}
