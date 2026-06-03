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

        public async Task<bool> CardapioItemExists(Guid id)
        {
            return await con.CardapioItem.AsNoTracking().AnyAsync(x => x.Id == id);
        }

        public async Task<IList<CardapioDTO>> GetCardapioByRestaurante(Guid restauranteId)
        {
            var model = await con.Cardapio.AsNoTracking().Where(x => x.RestauranteId == restauranteId).Include(x => x.Restaurante.Idiomas).ToListAsync();
            return Map<List<CardapioDTO>>.Convert(model);
        }

        public async Task<int> GetCardapiosCountByRestaurante(Guid restauranteId)
        {
            var count = await con.Cardapio.AsNoTracking().Where(x => x.RestauranteId == restauranteId).CountAsync();
            return count;
        }

        public async Task<IList<CardapioItemDTO>> GetCardapioItensByCardapio(Guid cardapioId)
        {
            var models = await con.CardapioItem.AsNoTracking().Where(x => x.CardapioId == cardapioId)
                .Select(x => new CardapioItemDTO
                {
                    Id = x.Id,
                    CategoriaId = x.CategoriaId,
                    Nome = x.Nome,
                    Descricao = x.Descricao,
                    Preco = x.Preco,
                    ImagemURL = x.ImagemURL,
                    ImagemBytes = x.ImagemBytes,
                    Ordem = x.Ordem,
                    Cardapio = new CardapioDTO
                    {
                        Restaurante = new RestauranteDTO
                        {
                            Nome = x.Nome,
                            Id = x.Id
                        },
                        Nome = x.Cardapio.Nome,
                        Id = x.Cardapio.Id
                    },
                    Categoria = new CategoriaDTO
                    {
                        Id = x.Categoria.Id,
                        Nome = x.Categoria.Nome,
                        OrdemExibicao = x.Categoria.OrdemExibicao
                    },
                    ImagemBase64 = x.ImagemBytes != null ? Convert.ToBase64String(x.ImagemBytes) : null
                })
                .OrderBy(x => x.Categoria.OrdemExibicao)
                .ThenBy(x => x.Ordem).ToListAsync();

            return models;
        }

        public async Task<CardapioItemDTO?> GetItemById(Guid id)
        {
            var model = await con.CardapioItem.AsNoTracking().Include(x => x.Cardapio.Restaurante).Include(x => x.Cardapio.Restaurante.Idiomas)
                .Include(x => x.Categoria).Where(x => x.Id == id)
                .Select(x => new CardapioItemDTO
                {
                    Id = x.Id,
                    Nome = x.Nome,
                    Ordem = x.Ordem,
                    Descricao = x.Descricao,
                    Preco = x.Preco
                }).FirstOrDefaultAsync();
            return model;
        }

        public async Task<IList<CardapioItemDTO>> GetItensByCardapio(Guid cardapioId)
        {
            var models = await con.CardapioItem.AsNoTracking().Where(x => x.CardapioId == cardapioId)
                .Include(x => x.Cardapio.Restaurante.Idiomas)
                .Include(x => x.Categoria)
                .Select(x => new CardapioItemDTO
                {
                    Id = x.Id,
                    CardapioId = x.CardapioId,
                    ImagemBase64 = x.ImagemBytes != null ? Convert.ToBase64String(x.ImagemBytes) : null,
                    ImagemURL = x.ImagemURL,
                    ImagemBytes = x.ImagemBytes,
                    Nome = x.Nome,
                    Descricao = x.Descricao,
                    Preco = x.Preco,
                    Ordem = x.Ordem,
                    CategoriaId = x.CategoriaId,
                })
                .ToListAsync();
            return Map<List<CardapioItemDTO>>.Convert(models);
        }

        public async Task UpdateCardapio(CardapioDTO dto)
        {
            var model = await con.Cardapio.FirstAsync(x => x.Id == dto.Id);
            model.Nome = dto.Nome;
            model.Descricao = dto.Descricao;
            model.UsuarioAlteracao = dto.UsuarioAlteracao;
            model.RestauranteId = dto.RestauranteId;
            model.DataAlteracao = dto.DataAlteracao;

            await con.SaveChangesAsync();
        }

        public async Task UpdateCardapioItem(CardapioItemDTO dto)
        {
            var model = await con.CardapioItem.FirstAsync(x => x.Id == dto.Id);

            model.CategoriaId = dto.CategoriaId;
            model.Nome = dto.Nome;
            model.Descricao = dto.Descricao;
            model.Preco = dto.Preco;
            model.ImagemURL = dto.ImagemURL;
            model.ImagemBytes = dto.ImagemBytes;
            model.UsuarioAlteracao = dto.UsuarioAlteracao;
            model.DataAlteracao = dto.DataAlteracao;
            model.Ordem = dto.Ordem;

            await con.SaveChangesAsync();
        }

        public Task<bool> CardapioExists(Guid id)
        {
            return con.Cardapio.AsNoTracking().AnyAsync(x => x.Id == id);
        }
    }
}
