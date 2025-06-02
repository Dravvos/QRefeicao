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
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _repository;
        public CategoriaService(ICategoriaRepository repository)
        {
            _repository = repository;
        }

        private void ValidarDTO(CategoriaDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Nome))
                throw new ArgumentNullException("Digite o nome da categoria");
            if (dto.RestauranteId == Guid.Empty)
                throw new ArgumentNullException("Restaurante não selecionado");
            if (dto.OrdemExibicao < 0)
                throw new ArgumentOutOfRangeException("O menor índice é zero");
            
        }

        public async Task CreateCategoria(CategoriaDTO dto)
        {
            ValidarDTO(dto);
            dto.Id = Guid.NewGuid();
            dto.DataInclusao = DateTime.UtcNow;
            await _repository.CreateCategoria(dto);
        }

        public async Task DeleteCategoria(Guid id)
        {
            if (await _repository.GetById(id) == null)
                throw new KeyNotFoundException();

            await _repository.DeleteCategoria(id);
        }

        public async Task<IList<CategoriaDTO>> GetCategoriasByRestaurante(Guid restauranteId)
        {
            return await _repository.GetCategoriasByRestaurante(restauranteId);
        }

        public async Task UpdateCategoria(CategoriaDTO dto)
        {
            ValidarDTO(dto);
            dto.DataAlteracao = DateTime.UtcNow;
            await _repository.UpdateCategoria(dto);
        }
    }
}
