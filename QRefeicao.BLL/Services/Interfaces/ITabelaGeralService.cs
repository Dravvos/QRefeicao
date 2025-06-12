using QRefeicao.DTO;

namespace QRefeicao.BLL.Services.Interfaces
{
    public interface ITabelaGeralService
    {
        Task<TabelaGeralDTO> GetByIdAsync(Guid id);
        Task<TabelaGeralDTO> GetByNomeAsync(string nome);
        Task<IEnumerable<TabelaGeralDTO>> GetAllAsync();
        Task<TabelaGeralDTO> AddAsync(TabelaGeralDTO dto);
        Task UpdateAsync(TabelaGeralDTO dto);
        Task DeleteAsync(Guid id);
    }
}
