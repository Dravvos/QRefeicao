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
    public class AssinaturaService : IAssinaturaService
    {
        private readonly IAssinaturaRepository _repository;

        public AssinaturaService(IAssinaturaRepository repository)
        {
            _repository = repository;
        }

        public Task CreateAssinatura(AssinaturaDTO assinatura)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAssinatura(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<AssinaturaDTO> GetAssinaturaByUserId(Guid usuarioId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAssinatura(AssinaturaDTO assinatura)
        {
            throw new NotImplementedException();
        }
    }
}
