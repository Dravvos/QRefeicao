using QRefeicao.DTO;

namespace QRefeicao.BLL.Repositories.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<CategoriaDTO> GetById(Guid id);
        Task<IList<CategoriaDTO>> GetCategoriasByRestaurante(Guid restauranteId);
        Task CreateCategoria(CategoriaDTO dto);
        Task UpdateCategoria(CategoriaDTO dto);
        Task DeleteCategoria(Guid id);
    }
}
