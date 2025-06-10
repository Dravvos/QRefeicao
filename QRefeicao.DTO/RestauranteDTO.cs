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
        public byte[]? LogoBytes { get; set; }
        public string LogoBase64
        {
            get
            {
                if (LogoBytes != null && LogoBytes.Length > 0)
                    return Convert.ToBase64String(LogoBytes);
                return string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    LogoBytes = Convert.FromBase64String(value);
                else
                    LogoBytes = null;
            }
        }
        public string? CorPrincipal { get; set; }
        public string? CorSecundaria { get; set; }
        public Guid UsuarioId { get; set; }

        public List<TabelaGeralItemDTO> Idiomas { get; set; } = new List<TabelaGeralItemDTO>();
    }
}
