using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using QRefeicao.BLL.Repositories.Interfaces;
using QRefeicao.BLL.Services.Interfaces;
using QRefeicao.DTO;

namespace QRefeicao.BLL.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _repository;
        private readonly IAssinaturaRepository _assinaturaRepository;
        private readonly ITabelaGeralItemRepository _tabelaGeralItemRepository;
        private readonly IUserRepository _userRepository;

        public CategoriaService(ICategoriaRepository repository, IAssinaturaRepository assinaturaRepository,
            ITabelaGeralItemRepository tabelaGeralItemRepository, IUserRepository userRepository)
        {
            _repository = repository;
            _assinaturaRepository = assinaturaRepository;
            _tabelaGeralItemRepository = tabelaGeralItemRepository;
            _userRepository = userRepository;
        }

        private async Task ValidarDTO(CategoriaDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Nome))
                throw new ArgumentNullException("Digite o nome da categoria");
            if (dto.RestauranteId == Guid.Empty)
                throw new ArgumentNullException("Restaurante não selecionado");
            if (dto.OrdemExibicao < 0)
                throw new ArgumentOutOfRangeException("O menor índice é zero");
            if (dto.IdTGIdioma == Guid.Empty)
                throw new ArgumentNullException("Selecione o idioma da categoria");

            var idiomasCategorias = (await _repository.GetCategoriasByRestaurante(dto.RestauranteId)).Select(x => x.Idioma).Distinct().ToList();

            Guid userId = await _userRepository.GetUserId(dto.UsuarioInclusao);

            if (userId != Guid.Empty)
            {
                var assinatura = await _assinaturaRepository.GetAssinaturaByUserId(userId);
                var tipoAssinatura = await _tabelaGeralItemRepository.GetByIdAsync(assinatura.IdTGTipoAssinatura);
                var tiposAssinatura = await _tabelaGeralItemRepository.GetAllItemsAsync(tipoAssinatura.TabelaGeralId);
                var basica = tiposAssinatura.First(x => x.Sigla == "BASE");
                var profissional = tiposAssinatura.First(x => x.Sigla == "PRO");
                if (assinatura.IdTGTipoAssinatura == basica.Id && idiomasCategorias.Count == 1)
                {
                    throw new Exception("A assinatura básica não permite a criação de categorias em mais de um idioma. " +
                        "Por favor, atualize sua assinatura para desbloquear essa funcionalidade.");
                }
                else if(assinatura.IdTGTipoAssinatura == profissional.Id && idiomasCategorias.Count == 3)
                {
                    throw new Exception("A assinatura profissional não permite a criação de categorias em mais de 3 idiomas. " +
                        "Por favor, atualize sua assinatura para desbloquear essa funcionalidade.");
                }
            }



        }

        public async Task CreateCategoria(CategoriaDTO dto)
        {
            await ValidarDTO(dto);
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
            await ValidarDTO(dto);
            dto.DataAlteracao = DateTime.UtcNow;
            await _repository.UpdateCategoria(dto);
        }
    }
}
