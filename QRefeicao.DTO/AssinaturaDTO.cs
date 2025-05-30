using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRefeicao.DTO
{
    public class AssinaturaDTO:BaseDTO
    {
        public Guid UsuarioId { get; set; }
        public Guid IdTGTipoAssinatura { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public Guid IdTGStatusAssinatura { get; set; }
        public TabelaGeralItemDTO? StatusAssinatura { get; set; }
        public TabelaGeralItemDTO? TipoAssinatura { get; set; }
    }
}
