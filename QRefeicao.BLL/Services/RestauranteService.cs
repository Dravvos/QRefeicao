using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql.Internal;
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

        private void ValidarDTO(RestauranteDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Nome))
                throw new ArgumentNullException("Digite o nome do restaurante");

            if (dto.UsuarioId == Guid.Empty)
                throw new ArgumentNullException("Sem associação de usuário");

            if (string.IsNullOrEmpty(dto.Email))
                throw new ArgumentNullException("Preencha o email do restaurante. Se não tiver preencha com");
            
            if (string.IsNullOrEmpty(dto.Telefone))
                throw new ArgumentNullException("Preencha o telefone do restaurante. Se não tiver preencha com 0");

            if (string.IsNullOrEmpty(dto.CEP))
                throw new ArgumentNullException("Preencha o CEP do restaurante");
            
            if (string.IsNullOrEmpty(dto.Endereco))
                throw new ArgumentNullException("Preencha o endereço do restaurante");

            if (dto.Numero <= 0)
                throw new ArgumentOutOfRangeException("O número do endereço do restaurante deve ser maior que zero");
            
            if (string.IsNullOrEmpty(dto.Bairro))
                throw new ArgumentNullException("Preencha o bairro do restaurante");
            
            if (string.IsNullOrEmpty(dto.Cidade))
                throw new ArgumentNullException("Preencha a cidade do restaurante");
            
            if (string.IsNullOrEmpty(dto.Estado))
                throw new ArgumentNullException("Preencha o estado do restaurante");


        }

        public async Task CreateRestaurante(RestauranteDTO dto)
        {
            ValidarDTO(dto);
            
            dto.Id = Guid.NewGuid();
            dto.DataInclusao = DateTime.UtcNow;

            await _repository.CreateRestaurante(dto);
        }

        public async Task DeleteRestaurante(Guid id)
        {
            if (await _repository.GetById(id) == null)
                throw new KeyNotFoundException();

            await _repository.DeleteRestaurante(id);
        }

        public async Task<RestauranteDTO> GetRestauranteByUserId(Guid userId)
        {
            return await _repository.GetRestauranteByUserId(userId);
        }

        public async Task UpdateRestaurante(RestauranteDTO dto)
        {
            ValidarDTO(dto);
            if (dto.Id.HasValue == false || dto.Id.Value == Guid.Empty)
                throw new ArgumentNullException("Id inválido");

            if (await _repository.GetById(dto.Id.Value) == null)
                throw new KeyNotFoundException();

            dto.DataAlteracao = DateTime.UtcNow;
            await _repository.UpdateRestaurante(dto);
        }
    }
}
