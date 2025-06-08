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
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly QRContext con;

        public CategoriaRepository(QRContext con)
        {
            this.con = con;
        }

        public async Task CreateCategoria(CategoriaDTO dto)
        {
            var model = Map<CategoriaModel>.Convert(dto);
            await con.Categoria.AddAsync(model);
            await con.SaveChangesAsync();
        }

        public async Task DeleteCategoria(Guid id)
        {
            var model = await con.Categoria.FirstAsync(con => con.Id == id);
            con.Categoria.Remove(model);
            await con.SaveChangesAsync();
        }

        public async Task<CategoriaDTO> GetById(Guid id)
        {
            var model = await con.Categoria.FirstOrDefaultAsync(x => x.Id == id);
            return Map<CategoriaDTO>.Convert(model);
        }

        public async Task<IList<CategoriaDTO>> GetCategoriasByRestaurante(Guid restauranteId)
        {
            var models = await con.Categoria.Where(x => x.RestauranteId == restauranteId).OrderBy(x => x.OrdemExibicao).ToListAsync();
            return Map<List<CategoriaDTO>>.Convert(models);
        }

        public async Task UpdateCategoria(CategoriaDTO dto)
        {
            var model = await con.Categoria.FirstAsync(x => x.Id == dto.Id);

            model.Ativo = dto.Ativo;
            model.Nome = dto.Nome;
            model.OrdemExibicao = dto.OrdemExibicao;
            model.RestauranteId = dto.RestauranteId;
            model.DataAlteracao = dto.DataAlteracao;
            model.UsuarioAlteracao = dto.UsuarioAlteracao;

            await con.SaveChangesAsync();
        }
    }
}
