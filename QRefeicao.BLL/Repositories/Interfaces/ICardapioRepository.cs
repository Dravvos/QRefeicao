using QRefeicao.DTO;

namespace QRefeicao.BLL.Repositories.Interfaces
{
    public interface ICardapioRepository
    {
        Task<IList<CardapioDTO>> GetCardapioByRestaurante(Guid restauranteId);
        Task CreateCardapio(CardapioDTO dto);
        Task UpdateCardapio(CardapioDTO dto);
        Task DeleteCardapio(Guid id);
        Task<int> GetCardapiosCountByRestaurante(Guid restauranteId);
        Task<bool> CardapioExists(Guid id);
        Task<bool> CardapioItemExists(Guid id);
        Task<IList<CardapioItemDTO>> GetCardapioItensByCardapio(Guid restauranteId);
        Task<IList<CardapioItemDTO>> GetItensByCardapio(Guid cardapioId);
        Task CreateCardapioItem(CardapioItemDTO dto);
        Task UpdateCardapioItem(CardapioItemDTO dto);
        Task DeleteCardapioItem(Guid id);


    }
}
