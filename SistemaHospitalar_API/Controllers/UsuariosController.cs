using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SistemaHospitalar_API.Application.Constructors.Services;
using SistemaHospitalar_API.Application.Dtos.Usuario;
using SistemaHospitalar_API.Application.Services;

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

        // ========================================================================
        // GET ALL
        // ========================================================================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Iniciando obtenção de todos os usuários.");
                var usuarios = await _service.ObterUsuarios();
                _logger.LogInformation("Consulta de usuários concluída com sucesso. Total: {Count}", usuarios.Count());
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao obter todos os usuários.");
                return StatusCode(500, new { message = "Erro interno ao obter usuários." });
            }
        }

        // ========================================================================
        // GET BY ID
        // ========================================================================
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                _logger.LogInformation("Obtendo usuário por ID: {Id}", id);
                var usuario = await _service.ObterUsuarioPorId(id);

                if (usuario == null)
                {
                    _logger.LogWarning("Usuário não encontrado para ID: {Id}", id);
                    return NotFound(new { message = "Usuário não encontrado." });
                }

                _logger.LogInformation("Usuário encontrado: {Id}", id);
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao obter usuário por ID: {Id}", id);
                return StatusCode(500, new { message = "Erro interno ao obter usuário." });
            }
        }

        // ========================================================================
        // GET BY EMAIL
        // ========================================================================
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            try
            {
                _logger.LogInformation("Obtendo usuário por email: {Email}", email);
                var usuario = await _service.ObterUsuarioPorEmail(email);

                if (usuario == null)
                {
                    _logger.LogWarning("Usuário não encontrado para email: {Email}", email);
                    return NotFound(new { message = "Usuário não encontrado." });
                }

                _logger.LogInformation("Usuário encontrado por email: {Email}", email);
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao obter usuário por email: {Email}", email);
                return StatusCode(500, new { message = "Erro interno ao obter usuário." });
            }
        }

        // ========================================================================
        // POST - CREATE
        // ========================================================================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CriarUsuarioDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Dados inválidos ao criar usuário: {ModelState}", ModelState);
                    return BadRequest(ModelState);
                }

                var usuarioCriado = await _service.CriarUsuario(dto);
                _logger.LogInformation("Usuário criado com sucesso: {Id}", usuarioCriado.Id);
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

        // ========================================================================
        // PUT - UPDATE
        // ========================================================================
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] EditarUsuarioDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Dados inválidos ao atualizar usuário: {Id}, {ModelState}", id, ModelState);
                    return BadRequest(ModelState);
                }

                var usuarioAtualizado = await _service.EditarUsuario(id, dto);
                if (usuarioAtualizado == null)
                {
                    _logger.LogWarning("Usuário não encontrado para atualização: {Id}", id);
                    return NotFound(new { message = "Usuário não encontrado." });
                }

                _logger.LogInformation("Usuário atualizado com sucesso: {Id}", id);
                return Ok(usuarioAtualizado);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Erro de validação ao atualizar usuário: {Id}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao atualizar usuário: {Id}", id);
                return StatusCode(500, new { message = "Erro interno ao atualizar usuário." });
            }
        }

        // ========================================================================
        // ALTERAR SENHA
        // ========================================================================
        [HttpPost("alterar-senha")]
        public async Task<IActionResult> AlterarSenha([FromQuery] string email, [FromBody] AlterarSenhaDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Dados inválidos ao alterar senha para email: {Email}", email);
                    return BadRequest(ModelState);
                }

                var sucesso = await _service.AlterarSenha(email, dto);
                if (!sucesso)
                {
                    _logger.LogWarning("Falha ao alterar senha para email: {Email}", email);
                    return BadRequest(new { message = "Não foi possível alterar a senha." });
                }

                _logger.LogInformation("Senha alterada com sucesso para email: {Email}", email);
                return Ok(new { message = "Senha alterada com sucesso." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao alterar senha para email: {Email}", email);
                return StatusCode(500, new { message = "Erro interno ao alterar senha." });
            }
        }

        // ========================================================================
        // DELETE
        // ========================================================================
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                _logger.LogInformation("Recebida requisição para excluir (soft delete) usuário ID: {Id}", id);
                var excluiu = await _service.ExcluirUsuario(id);

                if (!excluiu)
                {
                    _logger.LogWarning("Soft delete não realizado. Usuário ID: {Id} não encontrado.", id);
                    return NotFound(new { message = "Usuário não encontrado para exclusão." });
                }

                _logger.LogInformation("Soft delete realizado com sucesso para usuário ID: {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao tentar realizar soft delete do usuário ID: {Id}", id);
                return StatusCode(500, new { message = "Erro interno ao excluir usuário." });
            }
        }
    }
}
