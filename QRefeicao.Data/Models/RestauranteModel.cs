using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRefeicao.Data.Models
{
    [Table("Restaurante")]
    public class RestauranteModel:BaseModel
    {
        [Required]
        [Column("Nome")]
        public string Nome { get; set; } = null!;

        [Column("LogoURL")]
        public string? LogoURL { get; set; }

        [Column("LogoBytes")]
        public byte[]? LogoBytes { get; set; }

        [Required]
        [Column("Endereco")]
        public string Endereco { get; set; } = null!;

        [Column("CorPrincipal")]
        public string? CorPrincipal { get; set; }

        [Column("CorSecundaria")]
        public string? CorSecundaria{ get; set; }

        [Required]
        [Column("UsuarioId")]
        public Guid UsuarioId { get; set; }
    }
}
