using QRefeicao.BLL.Repositories.Interfaces;
using QRefeicao.BLL.Services.Interfaces;
using QRefeicao.DTO;

namespace QRefeicao.BLL.Services
{
    public class CardapioService : ICardapioService
    {
        private readonly ICardapioRepository _repository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IAssinaturaRepository _assinaturaRepository;
        private readonly IUserRepository _userRepository;

        public CardapioService(ICardapioRepository repository, ICategoriaRepository categoriaRepository, IAssinaturaRepository assinaturaRepository, IUserRepository userRepository)
        {
            _repository = repository;
            _categoriaRepository = categoriaRepository;
            _assinaturaRepository = assinaturaRepository;
            _userRepository = userRepository;
        }

        private async Task ValidarDTO(CardapioDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Nome))
                throw new ArgumentNullException("Digite um nome para o cardápio. Nem que seja 'Cardápio do restaurante abc'");

            if (dto.RestauranteId == Guid.Empty)
                throw new ArgumentNullException("Restaurante não selecionado");

            var categorias = await _categoriaRepository.GetCategoriasByRestaurante(dto.RestauranteId);
            if (categorias == null || categorias.Count == 0)
                throw new Exception("O restaurante não possui categorias cadastradas. Cadastre uma categoria antes de criar o cardápio.");

        }

        private void ValidarDTO(CardapioItemDTO dto)
        {
            if (dto.CardapioId == Guid.Empty)
                throw new ArgumentNullException("Selecione o cardápio");

            if (dto.CategoriaId == Guid.Empty)
                throw new ArgumentNullException("Selecione a categoria do item");

            if (string.IsNullOrEmpty(dto.Nome))
                throw new ArgumentNullException("Digite o nome do prato/item");

            if (dto.Preco <= 0)
                throw new ArgumentOutOfRangeException("O preço deve ser maior que zero");

            if (dto.Ordem < 0)
                throw new ArgumentOutOfRangeException("O menor índice é zero");


        }

        public async Task CreateCardapio(CardapioDTO dto)
        {
            await ValidarDTO(dto);

            var userId = await _userRepository.GetUserId(dto.UsuarioInclusao);
            var assinatura = await _assinaturaRepository.GetAssinaturaByUserId(userId);

            var cardapios = await _repository.GetCardapioByRestaurante(dto.RestauranteId);
            if (assinatura.TipoAssinatura.Sigla == "BASE" && cardapios.Count == 1)
            {
                throw new Exception("A assinatura BÁSICA permite a criação de apenas um único cardápio");
            }
            else if (assinatura.TipoAssinatura.Sigla == "PRO" && cardapios.Count == 3)
            {
                throw new Exception("A assinatura PROFISSIONAL permite a criação de apenas 3 cardápios");
            }

            dto.Id = Guid.NewGuid();
            dto.DataInclusao = DateTime.UtcNow;
            await _repository.CreateCardapio(dto);
        }

        public async Task CreateCardapioItem(CardapioItemDTO dto)
        {
            ValidarDTO(dto);

            var userId = await _userRepository.GetUserId(dto.UsuarioInclusao);
            var assinatura = await _assinaturaRepository.GetAssinaturaByUserId(userId);
            if (assinatura.TipoAssinatura.Sigla == "BASE" && (string.IsNullOrEmpty(dto.ImagemURL) == false || string.IsNullOrEmpty(dto.ImagemBase64) == false))
            {
                throw new Exception("A assinatura BÁSICA não permite cadastrar foto dos pratos/itens");
            }

            var itensCardapaio = await _repository.GetItensByCardapio(dto.CardapioId);

            if (assinatura.TipoAssinatura.Sigla == "BASE" && itensCardapaio.Count == 50)
            {
                throw new Exception("Você atingiu o limite de 50 itens da assinatura BÁSICA");
            }

            var qtdImagensBase64 = itensCardapaio.Select(x => x.ImagemBase64).Distinct().Count();
            var qtdImagensURL = itensCardapaio.Select(x => x.ImagemURL).Distinct().Count();
            var qtdTotalImagens = qtdImagensBase64 + qtdImagensURL;
            if (assinatura.TipoAssinatura.Sigla == "PRO" && qtdTotalImagens == 100)
            {
                throw new Exception("Você atingiu o limite de 100 fotos dos pratos/itens da assinatura PROFISSIONAL");
            }

            dto.Id = Guid.NewGuid();
            dto.DataInclusao = DateTime.UtcNow;
            await _repository.CreateCardapioItem(dto);
        }

        public async Task DeleteCardapio(Guid id)
        {
            if (await _repository.GetById(id) == null)
                throw new KeyNotFoundException();

            await _repository.DeleteCardapio(id);
        }

        public async Task DeleteCardapioItem(Guid id)
        {
            if (await _repository.GetItemById(id) == null)
                throw new KeyNotFoundException();

            await _repository.DeleteCardapioItem(id);
        }

        public async Task<IList<CardapioDTO>> GetCardapioByRestaurante(Guid restauranteId)
        {
            return await _repository.GetCardapioByRestaurante(restauranteId);
        }

        public async Task<IList<CardapioItemDTO>> GetCardapioItensByCardapio(Guid cardapioId)
        {
            return await _repository.GetCardapioItensByCardapio(cardapioId);
        }

        public async Task UpdateCardapio(CardapioDTO dto)
        {
            await ValidarDTO(dto);
            if (dto.Id.HasValue == false || dto.Id.Value == Guid.Empty)
                throw new ArgumentNullException("Id inválido");

            if (await _repository.GetById(dto.Id.Value) == null)
                throw new KeyNotFoundException();

            dto.DataAlteracao = DateTime.UtcNow;
            await _repository.UpdateCardapio(dto);
        }

        public async Task UpdateCardapioItem(CardapioItemDTO dto)
        {
            ValidarDTO(dto);
            dto.DataAlteracao = DateTime.UtcNow;
            await _repository.UpdateCardapioItem(dto);
        }
    }
}
