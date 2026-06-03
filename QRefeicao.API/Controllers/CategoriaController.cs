using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRefeicao.BLL.Services;
using QRefeicao.BLL.Services.Interfaces;
using QRefeicao.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace QRefeicao.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _service;
        private readonly ITraducaoService _traducaoService;
        private readonly ITabelaGeralItemService _tabelaGeralItemService;

        public CategoriaController(ICategoriaService service, ITraducaoService traducaoService, ITabelaGeralItemService tabelaGeralItemService)
        {
            _service = service;
            _traducaoService = traducaoService;
            _tabelaGeralItemService = tabelaGeralItemService;
        }

        [HttpGet("{restauranteId:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategoriasByRestaurante([FromRoute]Guid restauranteId, [FromQuery]string idiomaId = "")
        {
            try
            {
                var categorias = await _service.GetCategoriasByRestaurante(restauranteId);
                if (categorias == null || categorias.Any() == false)
                    return NotFound();

                if (string.IsNullOrEmpty(idiomaId) == false && idiomaId != "undefined")
                {
                    var idioma = await _tabelaGeralItemService.GetByIdAsync(Guid.Parse(idiomaId));
                    if (idioma == null)
                        return BadRequest("Idioma inválido");
                    foreach (var item in categorias)
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
                            item.Nome = await _traducaoService.GetTraducao(item.Nome, idioma.Sigla);
                        }
                        else
                            item.Nome = nomeTraduzido;

                    }
                }
                return Ok(categorias);
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
        public async Task<IActionResult> Create([FromBody] CategoriaDTO dto)
        {
            try
            {
                if (dto == null)
                    return UnprocessableEntity("Categoria não pode estar nula");

                dto.UsuarioInclusao = User.FindFirstValue(JwtRegisteredClaimNames.Name);
                await _service.CreateCategoria(dto);
                return StatusCode(201, dto);
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
        public async Task<IActionResult> Put(Guid id, [FromBody] CategoriaDTO dto)
        {
            try
            {
                if (dto == null)
                    return UnprocessableEntity("Categoria não pode estar nula");

                if (id != dto.Id)
                    return BadRequest("Id da categoria não confere");

                dto.UsuarioAlteracao = User.FindFirstValue(JwtRegisteredClaimNames.Name);
                await _service.UpdateCategoria(dto);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NoContent();
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

        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _service.DeleteCategoria(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NoContent();
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
