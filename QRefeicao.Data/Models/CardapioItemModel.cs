using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace QRefeicao.Data.Models
{
    [Table("CardapioItem")]
    public class CardapioItemModel:BaseModel
    {
        [Required]
        [Column("CardapioId")]
        public Guid CardapioId { get; set; }

        [Required]
        [Column("CategoriaId")]
        public Guid CategoriaId { get; set; }

        [Required]
        [Column("Nome")]
        public string? Nome { get; set; }

        [Column("Descricao")]
        public string? Descricao { get; set; }

        [Required]
        [Column("Preco")]
        [Precision(12,2)]
        public decimal Preco { get; set; }

        [Column("ImagemURL")]
        public string? ImagemURL { get; set; }

        [Column("ImagemBytes")]
        public byte[]? ImagemBytes { get; set; }

        [Required]
        [Column("Ordem")]
        public int Ordem { get; set; }

        [ForeignKey("CardapioId")]
        public virtual CardapioModel? Cardapio { get; set; }

        [ForeignKey("CategoriaId")]
        public virtual CategoriaModel? Categoria { get; set; }
    }
}
