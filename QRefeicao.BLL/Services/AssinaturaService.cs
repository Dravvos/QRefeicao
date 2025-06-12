using QRefeicao.BLL.Repositories.Interfaces;
using QRefeicao.BLL.Services.Interfaces;
using QRefeicao.DTO;

namespace QRefeicao.BLL.Services
{
    public class AssinaturaService : IAssinaturaService
    {
        private readonly IAssinaturaRepository _repository;

        public AssinaturaService(IAssinaturaRepository repository)
        {
            _repository = repository;
        }

        public void ValidarAssinatura(AssinaturaDTO dto)
        {
            if (dto.DataInicio == DateTime.MinValue || dto.DataInicio == DateTime.MaxValue || dto.DataInicio.Date < DateTime.Now.Date)
                throw new ArgumentException("Data de início inválida.");
            if (dto.DataFim == DateTime.MinValue || dto.DataFim == DateTime.MaxValue || dto.DataFim < DateTime.Now.Date)
                throw new ArgumentException("Data de fim inválida.");
            if (dto.UsuarioId == Guid.Empty)
                throw new ArgumentNullException("Usuário inválido.");
            if (dto.IdTGTipoAssinatura == Guid.Empty)
                throw new ArgumentNullException("Tipo de assinatura inválido.");
            if (dto.IdTGStatusAssinatura == Guid.Empty)
                throw new ArgumentNullException("Status de assinatura inválido.");
            if (dto.DataInicio > dto.DataFim)
                throw new ArgumentException("Data de início não pode ser maior que a data de fim.");
        }

        public async Task CreateAssinatura(AssinaturaDTO assinatura)
        {
            ValidarAssinatura(assinatura);
            assinatura.DataInclusao = DateTime.UtcNow.ToUniversalTime();
            assinatura.Id = Guid.NewGuid();
            await _repository.CreateAssinatura(assinatura);
        }

        public async Task DeleteAssinatura(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException("Id inválido.");

            var assinatura = await _repository.GetAssinaturaByUserId(id);
            if (assinatura == null)
                throw new KeyNotFoundException();

            await _repository.DeleteAssinatura(id);
        }

        public async Task<AssinaturaDTO> GetAssinaturaByUserId(Guid usuarioId)
        {
            if (usuarioId == Guid.Empty)
                throw new ArgumentNullException("Usuário inválido.");
            var assinatura = await _repository.GetAssinaturaByUserId(usuarioId);
            return assinatura;
        }

        public async Task UpdateAssinatura(AssinaturaDTO assinatura)
        {
            ValidarAssinatura(assinatura);
            if (assinatura.Id == Guid.Empty)
                throw new ArgumentNullException("Id inválido.");
            var assinaturaExistente = _repository.GetAssinaturaByUserId(assinatura.UsuarioId);
            if (assinaturaExistente == null)
                throw new KeyNotFoundException("Assinatura não encontrada.");

            assinatura.DataAlteracao = DateTime.UtcNow.ToUniversalTime();

            await _repository.UpdateAssinatura(assinatura);
        }
    }
}
