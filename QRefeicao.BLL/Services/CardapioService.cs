using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRefeicao.BLL.Repositories.Interfaces;
using QRefeicao.BLL.Services.Interfaces;
using QRefeicao.DTO;

namespace QRefeicao.BLL.Services
{
    public class CardapioService : ICardapioService
    {
        private readonly ICardapioRepository _repository;

        public CardapioService(ICardapioRepository repository)
        {
            _repository = repository;
        }

        public Task CreateCardapio(CardapioDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task CreateCardapioItem(CardapioItemDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCardapio(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCardapioItem(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<CardapioDTO> GetCardapioByRestaurante(Guid restauranteId)
        {
            throw new NotImplementedException();
        }

        public Task<IList<CardapioItemDTO>> GetCardapioItensByRestaurante(Guid restauranteId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCardapio(CardapioDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCardapioItem(CardapioItemDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
