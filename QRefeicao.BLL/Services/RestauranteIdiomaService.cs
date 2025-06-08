using Microsoft.AspNetCore.Mvc.Localization;
using QRefeicao.BLL.Repositories.Interfaces;
using QRefeicao.BLL.Services.Interfaces;
using QRefeicao.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRefeicao.BLL.Services
{
    public class RestauranteIdiomaService : IRestauranteIdiomaService
    {
        private readonly IRestauranteIdiomaRepository _repository;

        public RestauranteIdiomaService(IRestauranteIdiomaRepository repository)
        {
            _repository = repository;
        }

        private void ValidarDTO(RestauranteIdiomaDTO dto)
        {
            if (dto.RestauranteId == Guid.Empty)
                throw new ArgumentNullException("Selecione o restaurante");

            if (dto.IdTGIdioma == Guid.Empty)
                throw new ArgumentNullException("Selecione o idioma do restaurante");
        }

        public async Task Create(RestauranteIdiomaDTO dto)
        {
            ValidarDTO(dto);

            dto.Id = Guid.NewGuid();
            dto.DataInclusao = DateTime.UtcNow;

            await _repository.Create(dto);
        }

        public async Task Delete(Guid id)
        {
            var dto = await _repository.GetById(id);
            if (dto == null)
                throw new KeyNotFoundException();

            await _repository.Delete(id);
        }

        public async Task<IList<RestauranteIdiomaDTO>> GetIdiomasRestaurante(Guid restauranteId)
        {
            return await _repository.GetIdiomasRestaurante(restauranteId);
        }

        public async Task Update(RestauranteIdiomaDTO dto)
        {
            ValidarDTO(dto);

            dto.DataAlteracao = DateTime.UtcNow;
            await _repository.Update(dto);
        }
    }
}
