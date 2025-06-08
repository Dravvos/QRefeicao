using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRefeicao.DTO
{
    public class TraducaoDTO
    {
        public Guid Id { get; set; }
        public string? TextoOriginal { get; set; }
        public string? IdiomaOriginal { get; set; }
        public string? TextoTraduzido { get; set; }
        public string? IdiomaTraduzido { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}
