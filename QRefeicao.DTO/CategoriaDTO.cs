using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRefeicao.DTO
{
    public class CategoriaDTO:BaseDTO
    {
        public Guid RestauranteId { get; set; }
        public string? Nome { get; set; }
        public int OrdemExibicao { get; set; }
        public bool Ativo { get; set; }
        public RestauranteDTO? Restaurante { get; set; }
    }
}
