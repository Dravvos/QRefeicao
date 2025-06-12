using QRefeicao.DTO;

namespace QRefeicao.BLL.Repositories.Interfaces
{
    public interface ITabelaGeralRepository
    {
        Task<TabelaGeralDTO> GetByIdAsync(Guid id);
        Task<TabelaGeralDTO> GetByNomeAsync(string nome);
        Task<IList<TabelaGeralDTO>> GetAllAsync();
        Task<TabelaGeralDTO> AddAsync(TabelaGeralDTO dto);
        Task<TabelaGeralDTO> UpdateAsync(TabelaGeralDTO dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
