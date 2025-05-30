using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRefeicao.DTO
{
    public class CardapioDTO:BaseDTO
    {
        public string? Nome { get; set; }
        public Guid RestauranteId { get; set; }
        public string? Descricao { get; set; }
        public RestauranteDTO? Restaurante { get; set; }
    }
}
