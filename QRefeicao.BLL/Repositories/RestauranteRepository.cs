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

        public async Task<RestauranteDTO> GetById(Guid id)
        {
            var model = await con.Restaurante.FirstOrDefaultAsync(x => x.Id == id);
            return Map<RestauranteDTO>.Convert(model);
        }

        public async Task<RestauranteDTO> GetRestauranteByUserId(Guid userId)
        {
            var model = await con.Restaurante.FirstOrDefaultAsync(x => x.UsuarioId == userId);
            return Map<RestauranteDTO>.Convert(model);
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
