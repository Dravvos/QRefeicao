using Microsoft.EntityFrameworkCore;
using QRefeicao.BLL.Repositories.Interfaces;
using QRefeicao.Data;
using QRefeicao.Data.Models;
using QRefeicao.DTO;

namespace QRefeicao.BLL.Repositories
{
    public class RestauranteRepository : IRestauranteRepository
    {
        private readonly QRContext con;

        public RestauranteRepository(QRContext con)
        {
            this.con = con;
        }

        public async Task CreateRestaurante(RestauranteDTO dto)
        {
            var model = Map<RestauranteModel>.Convert(dto);
            await con.Restaurante.AddAsync(model);
            await con.SaveChangesAsync();
        }

        public Task DeleteRestaurante(Guid id)
        {
            var model = con.Restaurante.FirstAsync(x => x.Id == id);
            con.Restaurante.Remove(model.Result);
            return con.SaveChangesAsync();
        }

        public async Task<RestauranteDTO?> GetById(Guid id)
        {
            var restauranteIdiomas = con.RestauranteIdioma.AsNoTracking().Where(ri => ri.RestauranteId == id).Select(ri => new TabelaGeralItemDTO
            {
                Id = ri.Id,
                Descricao = ri.Idioma.Descricao,
                Sigla = ri.Idioma.Sigla
            }).ToList();
            var model = await con.Restaurante.AsNoTracking().Where(x => x.Id == id).Select(x => new RestauranteDTO
            {
                Id = x.Id,
                Nome = x.Nome,
                CorPrincipal = x.CorPrincipal,
                CorSecundaria = x.CorSecundaria,
                LogoBytes = x.LogoBytes,
                Idiomas = restauranteIdiomas,
                LogoBase64 = x.LogoBytes != null ? Convert.ToBase64String(x.LogoBytes) : null
            }).FirstOrDefaultAsync();
            return model;
        }

        public async Task<RestauranteDTO?> GetRestauranteByUserId(Guid userId)
        {
            var model = await con.Restaurante.AsNoTracking().Where(x => x.UsuarioId == userId).Select(x => new RestauranteDTO
            {
                Id = x.Id,
                Nome = x.Nome,
                CorPrincipal = x.CorPrincipal,
                CorSecundaria = x.CorSecundaria,
                LogoBytes = x.LogoBytes,
                Idiomas = new List<TabelaGeralItemDTO>(5),
                LogoBase64 = x.LogoBytes != null ? Convert.ToBase64String(x.LogoBytes) : null
            }).FirstOrDefaultAsync();
            if (model == null)
                return model;
            var restauranteIdiomas = con.RestauranteIdioma.AsNoTracking().Where(ri => ri.RestauranteId == model.Id).Select(ri => new TabelaGeralItemDTO
            {
                Id = ri.Id,
                Descricao = ri.Idioma.Descricao,
                Sigla = ri.Idioma.Sigla
            }).ToList();
            model.Idiomas = restauranteIdiomas;
            return model;
        }

        public async Task<bool> RestauranteExists(Guid id)
        {
            return await con.Restaurante.AsNoTracking().AnyAsync(x => x.Id == id);
        }

        public async Task UpdateRestaurante(RestauranteDTO dto)
        {
            var model = await con.Restaurante.FirstAsync(x => x.Id == dto.Id);

            model.Nome = dto.Nome;
            model.CorPrincipal = dto.CorPrincipal;
            model.CorSecundaria = dto.CorSecundaria;
            model.LogoBytes = dto.LogoBytes;
            model.DataAlteracao = dto.DataAlteracao;
            model.UsuarioAlteracao = dto.UsuarioAlteracao;

            await con.SaveChangesAsync();
        }
    }
}
