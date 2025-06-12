using QRefeicao.DTO;

namespace QRefeicao.BLL.Services.Interfaces
{
    public interface ICategoriaService
    {
        Task<IList<CategoriaDTO>> GetCategoriasByRestaurante(Guid restauranteId);
        Task CreateCategoria(CategoriaDTO dto);
        Task UpdateCategoria(CategoriaDTO dto);
        Task DeleteCategoria(Guid id);
    }
}
