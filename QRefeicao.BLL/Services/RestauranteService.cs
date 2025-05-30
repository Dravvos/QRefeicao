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
    public class RestauranteService : IRestauranteService
    {
        private readonly IRestauranteRepository _repository;

        public RestauranteService(IRestauranteRepository repository)
        {
            _repository = repository;
        }

        public Task CreateRestaurante(RestauranteDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRestaurante(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<RestauranteDTO> GetRestauranteByUserId(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRestaurante(RestauranteDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
