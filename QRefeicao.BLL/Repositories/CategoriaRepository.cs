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

        public Task<bool> CategoriaExists(Guid id)
        {
            return con.Categoria.AsNoTracking().AnyAsync(x => x.Id == id);
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

        public async Task<CategoriaDTO?> GetById(Guid id)
        {
            var model = await con.Categoria.AsNoTracking().Where(x => x.Id == id).Select(x => new CategoriaDTO
            {
                Id = x.Id,
                Restaurante = new RestauranteDTO
                {
                    Id = x.Restaurante.Id,
                    Nome = x.Restaurante.Nome
                },
                Ativo = x.Ativo,
                Nome = x.Nome,
                OrdemExibicao = x.OrdemExibicao
            }).FirstOrDefaultAsync();
            return model;
        }

        public async Task<IList<CategoriaDTO>> GetCategoriasByRestaurante(Guid restauranteId)
        {
            var models = await con.Categoria.AsNoTracking().Where(x => x.RestauranteId == restauranteId)
                .Select(x => new CategoriaDTO
                {
                    Id = x.Id,
                    Restaurante = new RestauranteDTO
                    {
                        Id = x.Restaurante.Id,
                        Nome = x.Restaurante.Nome
                    },
                    OrdemExibicao = x.OrdemExibicao,
                    Ativo = x.Ativo,
                    Nome = x.Nome
                }).OrderBy(x => x.OrdemExibicao).ToListAsync();
            return models;
        }

        public async Task<int> GetCategoriasCount(Guid restauranteId)
        {
            var count = await con.Categoria.AsNoTracking().Where(x => x.RestauranteId == restauranteId).CountAsync();
            return count;
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
