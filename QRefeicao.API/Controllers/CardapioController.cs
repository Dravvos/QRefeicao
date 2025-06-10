using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRefeicao.BLL.Services.Interfaces;
using QRefeicao.DTO;

namespace QRefeicao.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardapioController : ControllerBase
    {
        private readonly ICardapioService _service;
        private readonly ITabelaGeralItemService _tabelaGeralItemService;
        private readonly ITraducaoService _traducaoService;

        public CardapioController(ICardapioService service, ITabelaGeralItemService tabelaGeralItemService, ITraducaoService traducaoService)
        {
            _service = service;
            _tabelaGeralItemService = tabelaGeralItemService;
            _traducaoService = traducaoService;
        }

        [HttpGet("{restauranteId:guid}")]
        [Authorize]
        public async Task<IActionResult> Get(Guid restauranteId)
        {
            try
            {
                var cardapio = await _service.GetCardapioByRestaurante(restauranteId);
                if (cardapio == null)
                    return NotFound();

                return Ok(cardapio);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return StatusCode(500, ex.InnerException.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CardapioDTO dto)
        {
            try
            {
                if (dto == null)
                    return UnprocessableEntity();
                dto.UsuarioInclusao = User.FindFirstValue(JwtRegisteredClaimNames.Name);
                await _service.CreateCardapio(dto);
                return StatusCode(201);
            }

            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return StatusCode(500, ex.InnerException.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] CardapioDTO dto)
        {
            try
            {
                if (dto == null)
                    return UnprocessableEntity("Cardápio não pode estar nulo");
                if (id != dto.Id)
                    return BadRequest("Id do cardápio não confere");

                dto.UsuarioAlteracao = User.FindFirstValue(JwtRegisteredClaimNames.Name);
                await _service.UpdateCardapio(dto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return StatusCode(500, ex.InnerException.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return UnprocessableEntity("Id do cardápio não pode ser vazio");

                await _service.DeleteCardapio(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return StatusCode(500, ex.InnerException.Message);
                return StatusCode(500, ex.Message);
            }
        }

        private string DesembaralharGuid(string guid)
        {
            string cleanGuid = guid.Replace("-", "");
            if (cleanGuid.Length != 32)
            {
                throw new ArgumentException("GUID inválido. Deve conter 32 caracteres hexadecimais sem hífens.");
            }

            char[] scrambled = cleanGuid.ToCharArray();
            char[] original = new char[32];

            // Desembaralha: para cada posição j, o caractere volta para a posição (j * 11) % 32
            // (pois 11 é o inverso modular de 3 em módulo 32)
            for (int j = 0; j < 32; j++)
            {
                int i = (j * 11) % 32;
                original[i] = scrambled[j];
            }

            // Opcional: reinsere os hífens no padrão padrão 8-4-4-4-12
            return string.Format("{0}-{1}-{2}-{3}-{4}",
                new string(original, 0, 8),
                new string(original, 8, 4),
                new string(original, 12, 4),
                new string(original, 16, 4),
                new string(original, 20, 12));
        }

        [HttpGet]
        [Route("[action]/{cardapioId:guid}/{idiomaId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetItems(Guid cardapioId, string idiomaId = "")
        {
            try
            {
                cardapioId = Guid.Parse(DesembaralharGuid(cardapioId.ToString()));

                if (cardapioId == Guid.Empty)
                    return UnprocessableEntity("Id do cardápio não pode ser vazio");
                var itensCardapio = await _service.GetCardapioItensByCardapio(cardapioId);
                if (itensCardapio == null || itensCardapio.Any() == false)
                    return NotFound();

                if (string.IsNullOrEmpty(idiomaId) == false)
                {
                    var idioma = await _tabelaGeralItemService.GetByIdAsync(Guid.Parse(idiomaId));
                    foreach (var item in itensCardapio)
                    {
                        var nomeTraduzido = await _traducaoService.GetTraducao(item.Nome, idioma.Sigla);
                        if (string.IsNullOrEmpty(nomeTraduzido))
                        {
                            await _traducaoService.CreateTraducao(new TraducaoDTO
                            {
                                Id = Guid.NewGuid(),
                                IdiomaOriginal = "pt",
                                TextoOriginal = item.Nome,
                                IdiomaTraduzido = idioma.Sigla
                            });                            
                        }

                        item.Nome = await _traducaoService.GetTraducao(item.Nome, idioma.Sigla);
                        if (string.IsNullOrEmpty(item.Descricao) == false)
                        {
                            var descricaoTraduzida = await _traducaoService.GetTraducao(item.Descricao, idioma.Sigla);
                            if (string.IsNullOrEmpty(descricaoTraduzida))
                            {
                                await _traducaoService.CreateTraducao(new TraducaoDTO
                                {
                                    Id = Guid.NewGuid(),
                                    IdiomaOriginal = "pt",
                                    TextoOriginal = item.Descricao,
                                    IdiomaTraduzido = idioma.Sigla
                                });
                            }
                            item.Descricao = await _traducaoService.GetTraducao(item.Descricao, idioma.Sigla);
                        }
                    }
                }

                return Ok(itensCardapio);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return StatusCode(500, ex.InnerException.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize]
        public async Task<IActionResult> CreateItem([FromBody] CardapioItemDTO dto)
        {
            try
            {
                if (dto == null)
                    return UnprocessableEntity("Item do cardápio não pode estar nulo");

                dto.UsuarioInclusao = User.FindFirstValue(JwtRegisteredClaimNames.Name);
                await _service.CreateCardapioItem(dto);
                return StatusCode(201);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return StatusCode(500, ex.InnerException.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        [Route("[action]/{id:guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateItem(Guid id, [FromBody] CardapioItemDTO dto)
        {
            try
            {
                if (dto == null)
                    return UnprocessableEntity("Item do cardápio não pode estar nulo");

                if (id != dto.Id)
                    return BadRequest("Id do item do cardápio não confere");

                dto.UsuarioAlteracao = User.FindFirstValue(JwtRegisteredClaimNames.Name);
                await _service.UpdateCardapioItem(dto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return StatusCode(500, ex.InnerException.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        [Route("[action]/{id:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return UnprocessableEntity("Id do item do cardápio não pode ser vazio");
                await _service.DeleteCardapioItem(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return StatusCode(500, ex.InnerException.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
