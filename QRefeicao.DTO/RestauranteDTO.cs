using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRefeicao.DTO
{
    public class RestauranteDTO : BaseDTO
    {
        public string Nome { get; set; } = null!;
        public string? LogoURL { get; set; }
        public byte[]? LogoBytes { get; set; }
        public string? CorPrincipal { get; set; }
        public string? CorSecundaria { get; set; }
        public Guid UsuarioId { get; set; }
    }
}
