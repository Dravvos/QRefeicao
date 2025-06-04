using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QRefeicao.BLL.Repositories.Interfaces;
using QRefeicao.Data;
using QRefeicao.Data.Models;
using QRefeicao.DTO;

namespace QRefeicao.BLL.Repositories
{
    public class CardapioRepository : ICardapioRepository
    {
        private readonly QRContext con;

        public CardapioRepository(QRContext con)
        {
            this.con = con;
        }

        public async Task CreateCardapio(CardapioDTO dto)
        {
            var model = Map<CardapioModel>.Convert(dto);
            await con.Cardapio.AddAsync(model);
            await con.SaveChangesAsync();
        }

        public async Task CreateCardapioItem(CardapioItemDTO dto)
        {
            var model = Map<CardapioItemModel>.Convert(dto);
            await con.CardapioItem.AddAsync(model);
            await con.SaveChangesAsync();

        }

        public async Task DeleteCardapio(Guid id)
        {
            var cardapio = await con.Cardapio.FirstAsync(x => x.Id == id);
            con.Cardapio.Remove(cardapio);
            await con.SaveChangesAsync();
        }

        public async Task DeleteCardapioItem(Guid id)
        {
            var cardapio = await con.CardapioItem.FirstAsync(x => x.Id == id);
            con.CardapioItem.Remove(cardapio);
            await con.SaveChangesAsync();
        }

        public async Task<CardapioDTO> GetById(Guid id)
        {
            var model = await con.Cardapio.FirstOrDefaultAsync(x => x.Id == id);
            return Map<CardapioDTO>.Convert(model);
        }

        public async Task<IList<CardapioDTO>> GetCardapioByRestaurante(Guid restauranteId)
        {
            var model = await con.Cardapio.Where(x => x.RestauranteId == restauranteId).ToListAsync();
            return Map<List<CardapioDTO>>.Convert(model);
        }

        public async Task<IList<CardapioItemDTO>> GetCardapioItensByCardapio(Guid cardapioId)
        {
            var models = await con.CardapioItem.Where(x => x.CardapioId == cardapioId).ToListAsync();
            return Map<List<CardapioItemDTO>>.Convert(models);
        }

        public async Task<CardapioItemDTO> GetItemById(Guid id)
        {
            var model = await con.CardapioItem.FirstOrDefaultAsync(x => x.Id == id);
            return Map<CardapioItemDTO>.Convert(model);
        }

        public async Task<IList<CardapioItemDTO>> GetItensByCardapio(Guid cardapioId)
        {
            var models = await con.CardapioItem.Where(x => x.CardapioId == cardapioId).ToListAsync();
            return Map<List<CardapioItemDTO>>.Convert(models);
        }

        public async Task UpdateCardapio(CardapioDTO dto)
        {
            var model = await con.Cardapio.FirstAsync(x => x.Id == dto.Id);
            model.Nome = dto.Nome;
            model.IdTGIdioma = dto.IdTGIdioma;
            model.Descricao = dto.Descricao;
            model.UsuarioAlteracao = dto.UsuarioAlteracao;
            model.RestauranteId = dto.RestauranteId;
            model.DataAlteracao = dto.DataAlteracao;

            await con.SaveChangesAsync();
        }

        public async Task UpdateCardapioItem(CardapioItemDTO dto)
        {
            var model = await con.CardapioItem.FirstAsync(x => x.Id == dto.Id);

            model.CardapioId = dto.CardapioId;
            model.CategoriaId = dto.CategoriaId;
            model.Nome = dto.Nome;
            model.Descricao = dto.Descricao;
            model.Preco = dto.Preco;
            model.ImagemURL = dto.ImagemURL;
            model.ImagemBytes = dto.ImagemBytes;
            model.UsuarioAlteracao = dto.UsuarioAlteracao;
            model.DataAlteracao = dto.DataAlteracao;

            await con.SaveChangesAsync();
        }
    }
}
