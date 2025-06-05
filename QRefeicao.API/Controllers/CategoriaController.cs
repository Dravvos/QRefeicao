using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public CategoriaController(ICategoriaService service)
        {
            _service = service;
        }

        [HttpGet("{restauranteId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategoriasByRestaurante(Guid restauranteId)
        {
            try
            {
                var categorias = await _service.GetCategoriasByRestaurante(restauranteId);
                if (categorias == null || categorias.Any() == false)
                    return NotFound();

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

        [HttpPut("{id}")]
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

                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
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

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _service.DeleteCategoria(id);
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
