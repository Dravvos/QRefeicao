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

        public CardapioController(ICardapioService service)
        {
            _service = service;
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
                return Ok();
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
                return Ok();
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
                return Ok();
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

        [HttpGet]
        [Route("[action]/{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetItems(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return UnprocessableEntity("Id do cardápio não pode ser vazio");
                var itensCardapio = await _service.GetCardapioItensByCardapio(id);
                if (itensCardapio == null || itensCardapio.Any() == false)
                    return NotFound();

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
                return Ok();
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
                return Ok();
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
                return Ok();
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
