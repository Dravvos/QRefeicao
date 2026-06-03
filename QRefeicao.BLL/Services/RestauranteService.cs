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
            if (await _repository.RestauranteExists(id) == false)
                throw new KeyNotFoundException();

            await _repository.DeleteRestaurante(id);
        }

        public async Task<RestauranteDTO?> GetRestauranteByUserId(Guid userId)
        {
            return await _repository.GetRestauranteByUserId(userId);
        }

        public async Task UpdateRestaurante(RestauranteDTO dto)
        {
            ValidarDTO(dto);
            if (dto.Id.HasValue == false || dto.Id.Value == Guid.Empty)
                throw new ArgumentNullException("Id inválido");

            if (await _repository.RestauranteExists(dto.Id.Value) == false)
                throw new KeyNotFoundException();

            dto.DataAlteracao = DateTime.UtcNow;
            await _repository.UpdateRestaurante(dto);
        }
    }
}
