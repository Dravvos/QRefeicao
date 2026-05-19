using QRefeicao.DTO;

namespace QRefeicao.BLL.Services.Interfaces
{
    public interface IAssinaturaService
    {
        Task CreateAssinatura(AssinaturaDTO assinatura);
        Task UpdateAssinatura(AssinaturaDTO assinatura);
        Task DeleteAssinatura(Guid id);
        Task<AssinaturaDTO?> GetAssinaturaByUserId(Guid usuarioId);
    }
}
