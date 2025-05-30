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
        Task<CardapioDTO> GetCardapioByRestaurante(Guid restauranteId);
        Task CreateCardapio(CardapioDTO dto);
        Task UpdateCardapio(CardapioDTO dto);
        Task DeleteCardapio(Guid id);

        Task<IList<CardapioItemDTO>> GetCardapioItensByRestaurante(Guid restauranteId);
        Task CreateCardapioItem(CardapioItemDTO dto);
        Task UpdateCardapioItem(CardapioItemDTO dto);
        Task DeleteCardapioItem(Guid id);


    }
}
