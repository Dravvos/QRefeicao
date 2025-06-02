using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRefeicao.DTO
{
    public class CardapioItemDTO : BaseDTO
    {
        public Guid CardapioId { get; set; }
        public Guid CategoriaId { get; set; } // Prato Principal, Entrada, Bebida etc
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public string? ImagemURL { get; set; }
        public byte[]? ImagemBytes { get; set; }
        public int Ordem { get; set; }

        public CardapioDTO? Cardapio { get; set; }
        public CategoriaDTO? Categoria { get; set; }
    }
}
