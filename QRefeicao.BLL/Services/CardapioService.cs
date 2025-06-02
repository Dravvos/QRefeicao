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

        private void ValidarDTO(CardapioDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Nome))
                throw new ArgumentNullException("Digite um nome para o cardápio. Nem que seja 'Cardápio do restaurante abc'");

            if (dto.RestauranteId == Guid.Empty)
                throw new ArgumentNullException("Restaurante não selecionado");

            if (dto.IdTGIdioma == Guid.Empty)
                throw new ArgumentNullException("Selecione o idioma do cardápio");
        }

        private void ValidarDTO(CardapioItemDTO dto)
        {
            if (dto.CardapioId == Guid.Empty)
                throw new ArgumentNullException("Selecione o cardápio");

            if (dto.CategoriaId == Guid.Empty)
                throw new ArgumentNullException("Selecione a categoria do item");

            if (string.IsNullOrEmpty(dto.Nome))
                throw new ArgumentNullException("Digite o nome do prato/item");

            if (dto.Preco <= 0)
                throw new ArgumentOutOfRangeException("O preço deve ser maior que zero");

        }

        public async Task CreateCardapio(CardapioDTO dto)
        {
            ValidarDTO(dto);
            dto.Id = Guid.NewGuid();
            dto.DataInclusao = DateTime.UtcNow;
            await _repository.CreateCardapio(dto);
        }

        public async Task CreateCardapioItem(CardapioItemDTO dto)
        {
            ValidarDTO(dto);
            dto.Id = Guid.NewGuid();
            dto.DataInclusao = DateTime.UtcNow;
            await _repository.CreateCardapioItem(dto);
        }

        public async Task DeleteCardapio(Guid id)
        {
            if (await _repository.GetById(id) == null)
                throw new KeyNotFoundException();

            await _repository.DeleteCardapio(id);
        }

        public async Task DeleteCardapioItem(Guid id)
        {
            if (await _repository.GetItemById(id) == null)
                throw new KeyNotFoundException();

            await _repository.DeleteCardapioItem(id);
        }

        public async Task<CardapioDTO> GetCardapioByRestaurante(Guid restauranteId)
        {
            return await _repository.GetCardapioByRestaurante(restauranteId);
        }

        public async Task<IList<CardapioItemDTO>> GetCardapioItensByRestaurante(Guid restauranteId)
        {
            return await _repository.GetCardapioItensByRestaurante(restauranteId);
        }

        public async Task UpdateCardapio(CardapioDTO dto)
        {
            ValidarDTO(dto);
            if (dto.Id.HasValue == false || dto.Id.Value == Guid.Empty)
                throw new ArgumentNullException("Id inválido");

            if (await _repository.GetById(dto.Id.Value) == null)
                throw new KeyNotFoundException();

            dto.DataAlteracao = DateTime.UtcNow;
            await _repository.UpdateCardapio(dto);
        }

        public async Task UpdateCardapioItem(CardapioItemDTO dto)
        {
            ValidarDTO(dto);
            dto.DataAlteracao = DateTime.UtcNow;
            await _repository.UpdateCardapioItem(dto);
        }
    }
}
