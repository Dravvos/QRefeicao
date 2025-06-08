using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRefeicao.DTO
{
    public class RestauranteIdiomaDTO : BaseDTO
    {
        public Guid RestauranteId { get; set; }

        public Guid IdTGIdioma { get; set; }

        public RestauranteDTO? Restaurante { get; set; }

        public TabelaGeralItemDTO? Idioma { get; set; }
    }
}
