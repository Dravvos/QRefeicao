using QRefeicao.DTO;

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
