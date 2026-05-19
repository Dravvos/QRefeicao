using Microsoft.EntityFrameworkCore;
using QRefeicao.BLL.Repositories.Interfaces;
using QRefeicao.Data;
using QRefeicao.Data.Models;
using QRefeicao.DTO;

namespace QRefeicao.BLL.Repositories
{
    public class RestauranteIdiomaRepository : IRestauranteIdiomaRepository
    {
        private readonly QRContext con;

        public RestauranteIdiomaRepository(QRContext con)
        {
            this.con = con;
        }

        public async Task Create(RestauranteIdiomaDTO dto)
        {
            var model = Map<RestauranteIdiomaModel>.Convert(dto);
            await con.RestauranteIdioma.AddAsync(model);
            await con.SaveChangesAsync();

        }

        public async Task Delete(Guid id)
        {
            var model = await con.RestauranteIdioma.FirstAsync(x => x.Id == id);
            con.RestauranteIdioma.Remove(model);
            await con.SaveChangesAsync();
        }

        public async Task<IList<RestauranteIdiomaDTO>> GetIdiomasRestaurante(Guid restauranteId)
        {
            var model = await con.RestauranteIdioma.AsNoTracking().Where(x => x.RestauranteId == restauranteId).Include(x => x.Restaurante).Include(x => x.Idioma)
                .Select(x => new RestauranteIdiomaDTO
                {
                    Id = x.Id,
                    Restaurante = new RestauranteDTO
                    {
                        Id = x.Restaurante.Id,
                        Nome = x.Restaurante.Nome
                    },
                    Idioma = new TabelaGeralItemDTO
                    {
                        Id = x.Idioma.Id,
                        Descricao = x.Idioma.Descricao,
                        Sigla = x.Idioma.Sigla
                    }
                })
                .ToListAsync();
            return model;
        }

        public async Task Update(RestauranteIdiomaDTO dto)
        {
            var model = await con.RestauranteIdioma.FirstAsync(x => x.Id == dto.Id);

            model.RestauranteId = dto.RestauranteId;
            model.IdTGIdioma = dto.IdTGIdioma;
            model.DataAlteracao = dto.DataAlteracao;
            model.UsuarioAlteracao = dto.UsuarioAlteracao;

            await con.SaveChangesAsync();
        }

        public async Task<RestauranteIdiomaDTO?> GetById(Guid id)
        {
            var model = await con.RestauranteIdioma.AsNoTracking().Where(x => x.Id == id).Select(x => new RestauranteIdiomaDTO
            {
                Id = x.Id,
                Idioma = new TabelaGeralItemDTO
                {
                    Id = x.Idioma.Id,
                    Descricao = x.Idioma.Descricao,
                    Sigla = x.Idioma.Sigla
                },
                Restaurante = new RestauranteDTO
                {
                    Id = x.Restaurante.Id,
                    Nome = x.Restaurante.Nome,
                }
            }).FirstOrDefaultAsync();
            return Map<RestauranteIdiomaDTO>.Convert(model);
        }

        public async Task<bool> ItemExists(Guid id)
        {
            return await con.RestauranteIdioma.AsNoTracking().AnyAsync(x => x.Id == id);
        }
    }
}
