using Microsoft.AspNetCore.Mvc;
using SistemaHospitalar_API.Application.Constructors.Services;
using SistemaHospitalar_API.Application.Dtos.Usuario;

namespace SistemaHospitalar_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _service;
        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(
            IUsuarioService service,
            ILogger<UsuariosController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // ============================================================================
        // GET ALL
        // ============================================================================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _service.ObterUsuarios();
            return Ok(usuarios);
        }

        // ============================================================================
        // GET BY ID
        // ============================================================================
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var usuario = await _service.ObterUsuarioPorId(id);

            if (usuario == null)
                return NotFound(new { message = "Usuário não encontrado." });

            return Ok(usuario);
        }

        // ============================================================================
        // GET BY EMAIL
        // ============================================================================
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var usuario = await _service.ObterUsuarioPorEmail(email);

            if (usuario == null)
                return NotFound(new { message = "Usuário não encontrado." });

            return Ok(usuario);
        }

        // ============================================================================
        // POST - CREATE
        // ============================================================================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CriarUsuarioDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var usuarioCriado = await _service.CriarUsuario(dto);
                return CreatedAtAction(nameof(GetById), new { id = usuarioCriado.Id }, usuarioCriado);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Erro de validação ao criar usuário.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao criar usuário.");
                return StatusCode(500, new { message = "Erro interno ao criar usuário." });
            }
        }

        // ============================================================================
        // PUT - UPDATE
        // ============================================================================
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] EditarUsuarioDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var usuarioAtualizado = await _service.EditarUsuario(id, dto);

                if (usuarioAtualizado == null)
                    return NotFound(new { message = "Usuário não encontrado." });

                return Ok(usuarioAtualizado);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Erro de validação ao atualizar usuário.");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao atualizar usuário.");
                return StatusCode(500, new { message = "Erro interno ao atualizar usuário." });
            }
        }

        // ============================================================================
        // DELETE
        // ============================================================================
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Recebida requisição para excluir (soft delete) o usuário ID: {id}", id);

            try
            {
                var excluiu = await _service.ExcluirUsuario(id);

                if (!excluiu)
                {
                    _logger.LogWarning("Soft delete não realizado. Usuário ID: {id} não encontrado.", id);
                    return NotFound(new { message = "Usuário não encontrado para exclusão." });
                }

                _logger.LogInformation("Soft delete realizado com sucesso para o usuário ID: {id}", id);
                return NoContent(); // 204
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao tentar realizar soft delete do usuário ID: {id}", id);
                return StatusCode(500, new { message = "Erro interno ao excluir usuário." });
            }
        }

    }
}
