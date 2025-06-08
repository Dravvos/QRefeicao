using Microsoft.EntityFrameworkCore;
using QRefeicao.BLL.Repositories.Interfaces;
using QRefeicao.Data;
using QRefeicao.Data.Models;
using QRefeicao.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var model = await con.RestauranteIdioma.Where(x => x.RestauranteId == restauranteId).ToListAsync();
            return Map<List<RestauranteIdiomaDTO>>.Convert(model);
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

        public async Task<RestauranteIdiomaDTO> GetById(Guid id)
        {
            var model = await con.RestauranteIdioma.FirstOrDefaultAsync(x => x.Id == id);
            return Map<RestauranteIdiomaDTO>.Convert(model);
        }
    }
}
