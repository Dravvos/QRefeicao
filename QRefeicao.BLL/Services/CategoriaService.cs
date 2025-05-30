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
        public Task CreateCategoria(CategoriaDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCategoria(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IList<CategoriaDTO>> GetCategoriasByRestaurante(Guid restauranteId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCategoria(CategoriaDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
