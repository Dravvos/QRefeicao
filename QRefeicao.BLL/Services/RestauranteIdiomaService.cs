using Microsoft.AspNetCore.Mvc.Localization;
using QRefeicao.BLL.Repositories;
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
        private readonly IAssinaturaRepository _assinaturaRepository;
        private readonly IUserRepository _userRepository;

        public RestauranteIdiomaService(IRestauranteIdiomaRepository repository, IAssinaturaRepository assinaturaRepository, IUserRepository userRepository)
        {
            _repository = repository;
            _assinaturaRepository = assinaturaRepository;
            _userRepository = userRepository;
        }

        private async Task ValidarDTO(List<RestauranteIdiomaDTO> dtos)
        {
            foreach (var dto in dtos)
            {
                if (dto.RestauranteId == Guid.Empty)
                    throw new ArgumentNullException("Selecione o restaurante");

                if (dto.IdTGIdioma == Guid.Empty)
                    throw new ArgumentNullException("Selecione pelo menos 1 idioma do restaurante");

            }

            var userId = await _userRepository.GetUserId(dtos[0].UsuarioInclusao);

            var assinatura = await _assinaturaRepository.GetAssinaturaByUserId(userId);

            var idiomasRestaurante = await _repository.GetIdiomasRestaurante(dtos[0].RestauranteId);
            if (assinatura.TipoAssinatura.Sigla == "BASE" && dtos.Count >1)
            {
                throw new Exception("A assinatura BÁSICA não permite seu restaurante ter mais de 1 idioma");
            }
            else if (assinatura.TipoAssinatura.Sigla == "PRO" && dtos.Count > 3)
            {
                throw new Exception("A assinatura PROFISSIONAL não permite seu restaurante ter mais de 3 idiomas");
            }
        }

        public async Task Create(List<RestauranteIdiomaDTO> dtos)
        {
            await ValidarDTO(dtos);

            foreach(var dto in dtos)
            {
                dto.Id = Guid.NewGuid();
                dto.DataInclusao = DateTime.UtcNow;

                await _repository.Create(dto);
            }
            
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

        public async Task Update(List<RestauranteIdiomaDTO> dtos)
        {
            await ValidarDTO(dtos);
            foreach(var dto in dtos)
            {
                dto.DataAlteracao = DateTime.UtcNow;
                await _repository.Update(dto);
            }
            
        }
    }
}
