using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRefeicao.DTO;

namespace QRefeicao.BLL.Services.Interfaces
{
    public interface IRestauranteService
    {
        Task<RestauranteDTO> GetRestauranteByUserId(Guid userId);
        Task CreateRestaurante(RestauranteDTO dto);
        Task UpdateRestaurante(RestauranteDTO dto);
        Task DeleteRestaurante(Guid id);
    }
}
