using QRefeicao.DTO;

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
