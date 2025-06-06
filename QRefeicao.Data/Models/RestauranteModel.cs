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
    public class RestauranteModel : BaseModel
    {
        [Required]
        [Column("Nome")]
        public string Nome { get; set; } = null!;

        [Column("Telefone")]
        [Required]
        [StringLength(20)]
        public string Telefone { get; set; } = null!;

        [Column("Email")]
        [Required]
        public string Email { get; set; } = null!;

        [Column("LogoBytes")]
        public byte[]? LogoBytes { get; set; }

        [Column("CorPrincipal")]
        public string? CorPrincipal { get; set; }

        [Column("CorSecundaria")]
        public string? CorSecundaria { get; set; }

        [Column("CEP")]
        [Required]
        [StringLength(10, MinimumLength = 8)]
        public string CEP { get; set; } = null!;

        [Column("Endereco")]
        [Required]
        public string Endereco { get; set; } = null!;

        [Column("Numero")]
        [Required]
        [Range(1, 99999)]
        public uint Numero { get; set; }

        [Column("Complemento")]
        public string? Complemento { get; set; }

        [Column("Bairro")]
        [Required]
        public string Bairro { get; set; } = null!;

        [Column("Cidade")]
        [Required]
        public string Cidade { get; set; } = null!;

        [Column("Estado")]
        [Required]
        [StringLength(2)]
        public string Estado { get; set; } = null!;

        [Required]
        [Column("UsuarioId")]
        public Guid UsuarioId { get; set; }
    }
}
