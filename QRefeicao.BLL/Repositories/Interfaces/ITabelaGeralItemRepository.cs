using QRefeicao.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRefeicao.BLL.Repositories.Interfaces
{
    public interface ITabelaGeralItemRepository
    {
        Task<TabelaGeralItemDTO> GetByIdAsync(Guid id);
        Task<TabelaGeralItemDTO> GetBySiglaAsync(Guid tabelaGeralId, string sigla);
        Task<IList<TabelaGeralItemDTO>> GetAllAsync();
        Task<IList<TabelaGeralItemDTO>> GetAllItemsAsync(Guid? tabelaGeralId);
        Task<TabelaGeralItemDTO> AddAsync(TabelaGeralItemDTO item);
        Task<TabelaGeralItemDTO> UpdateAsync(TabelaGeralItemDTO item);
        Task<bool> DeleteAsync(Guid id);
    }
}
