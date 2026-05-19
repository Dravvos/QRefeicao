using QRefeicao.DTO;

namespace QRefeicao.BLL.Repositories.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<bool> CategoriaExists(Guid id);
        Task<int> GetCategoriasCount(Guid restauranteId);
        Task<IList<CategoriaDTO>> GetCategoriasByRestaurante(Guid restauranteId);
        Task CreateCategoria(CategoriaDTO dto);
        Task UpdateCategoria(CategoriaDTO dto);
        Task DeleteCategoria(Guid id);
    }
}
