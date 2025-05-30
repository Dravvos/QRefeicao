using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRefeicao.Data.Models
{
    [Table("Cardapio")]
    public class CardapioModel:BaseModel
    {
        [Required]
        [Column("Nome")]
        public string? Nome { get; set; }

        [Required]
        [Column("RestauranteId")]
        public Guid RestauranteId { get; set; }

        [Column("Descrição")]
        public string? Descricao { get; set; }

        
        [ForeignKey("RestauranteId")]
        public virtual RestauranteModel? Restaurante { get; set; }

        public virtual List<CardapioItemModel> ItensCardapio { get; set; } = new List<CardapioItemModel>();
    }
}
