using QRefeicao.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRefeicao.BLL.Repositories.Interfaces
{
    public interface IRestauranteIdiomaRepository
    {
        Task<RestauranteIdiomaDTO> GetById(Guid id);
        Task<IList<RestauranteIdiomaDTO>> GetIdiomasRestaurante(Guid restauranteId);
        Task Create(RestauranteIdiomaDTO dto);
        Task Update(RestauranteIdiomaDTO dto);
        Task Delete(Guid id);
    }
}
