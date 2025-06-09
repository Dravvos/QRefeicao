using QRefeicao.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRefeicao.BLL.Services.Interfaces
{
    public interface IRestauranteIdiomaService
    {
        Task<IList<RestauranteIdiomaDTO>> GetIdiomasRestaurante(Guid restauranteId);
        Task Create(List<RestauranteIdiomaDTO> dtos);
        Task Update(List<RestauranteIdiomaDTO> dtos);
        Task Delete(Guid id);
    }
}
