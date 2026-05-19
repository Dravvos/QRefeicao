using QRefeicao.DTO;

namespace QRefeicao.BLL.Repositories.Interfaces
{
    public interface IRestauranteRepository
    {
        Task<bool> RestauranteExists(Guid id);
        Task<RestauranteDTO?> GetRestauranteByUserId(Guid userId);
        Task CreateRestaurante(RestauranteDTO dto);
        Task UpdateRestaurante(RestauranteDTO dto);
        Task DeleteRestaurante(Guid id);

    }
}
