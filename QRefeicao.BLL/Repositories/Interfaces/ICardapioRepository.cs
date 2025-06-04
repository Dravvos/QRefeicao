using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRefeicao.DTO;

namespace QRefeicao.BLL.Repositories.Interfaces
{
    public interface ICardapioRepository
    {
        Task<CardapioDTO> GetById(Guid id);
        Task<IList<CardapioDTO>> GetCardapioByRestaurante(Guid restauranteId);
        Task CreateCardapio(CardapioDTO dto);
        Task UpdateCardapio(CardapioDTO dto);
        Task DeleteCardapio(Guid id);

        Task<CardapioItemDTO> GetItemById(Guid id);
        Task<IList<CardapioItemDTO>> GetCardapioItensByCardapio(Guid restauranteId);
        Task<IList<CardapioItemDTO>> GetItensByCardapio(Guid cardapioId);
        Task CreateCardapioItem(CardapioItemDTO dto);
        Task UpdateCardapioItem(CardapioItemDTO dto);
        Task DeleteCardapioItem(Guid id);


    }
}
