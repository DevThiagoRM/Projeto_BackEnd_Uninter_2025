using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SistemaHospitalar_API.Application.Constructors.Services;
using SistemaHospitalar_API.Application.Dtos.Especialidade;

namespace SistemaHospitalar_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspecialidadesController : ControllerBase
    {
        private readonly IEspecialidadeService _service;
        private readonly ILogger<EspecialidadesController> _logger;

        public EspecialidadesController(
            IEspecialidadeService service,
            ILogger<EspecialidadesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // ========================================================================
        // GET ALL
        // ========================================================================
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VisualizarEspecialidadeDto>>> ObterEspecialidades()
        {
            try
            {
                _logger.LogInformation("Iniciando obtenção de todas as especialidades.");
                var especialidades = await _service.ObterEspecialidades();
                _logger.LogInformation("Consulta de especialidades concluída com sucesso.");
                return Ok(especialidades);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter especialidades.");
                return StatusCode(500, new { message = "Erro interno ao obter especialidades." });
            }
        }

        // ========================================================================
        // GET BY ID
        // ========================================================================
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<VisualizarEspecialidadeDto?>> ObterEspecialidadePorId(int id)
        {
            try
            {
                _logger.LogInformation("Obtendo especialidade por ID: {Id}", id);
                var especialidade = await _service.ObterEspecialidadePorId(id);

                if (especialidade == null)
                {
                    _logger.LogWarning("Especialidade não encontrada para ID: {Id}", id);
                    return NotFound(new { message = "Especialidade não encontrada." });
                }

                _logger.LogInformation("Especialidade encontrada para ID: {Id}", id);
                return Ok(especialidade);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter especialidade por ID: {Id}", id);
                return StatusCode(500, new { message = "Erro interno ao obter especialidade." });
            }
        }

        // ========================================================================
        // POST
        // ========================================================================
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<ActionResult<VisualizarEspecialidadeDto>> CriarEspecialidade([FromBody] CriarEspecialidadeDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Dados inválidos ao criar especialidade: {ModelState}", ModelState);
                    return BadRequest(ModelState);
                }

                var nova = await _service.CriarEspecialidade(dto);
                _logger.LogInformation("Especialidade criada com sucesso: {Id}", nova.Id);
                return CreatedAtAction(nameof(ObterEspecialidadePorId), new { id = nova.Id }, nova);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Erro ao criar especialidade.");
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao criar especialidade.");
                return StatusCode(500, new { message = "Erro interno ao criar especialidade." });
            }
        }

        // ========================================================================
        // PUT
        // ========================================================================
        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<VisualizarEspecialidadeDto?>> EditarEspecialidade(int id, [FromBody] EditarEspecialidadeDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Dados inválidos ao editar especialidade: {Id}, {ModelState}", id, ModelState);
                    return BadRequest(ModelState);
                }

                var atualizada = await _service.EditarEspecialidade(id, dto);
                if (atualizada == null)
                {
                    _logger.LogWarning("Especialidade não encontrada para edição: {Id}", id);
                    return NotFound(new { message = "Especialidade não encontrada." });
                }

                _logger.LogInformation("Especialidade editada com sucesso: {Id}", id);
                return Ok(atualizada);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Erro ao atualizar especialidade: {Id}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao editar especialidade: {Id}", id);
                return StatusCode(500, new { message = "Erro interno ao editar especialidade." });
            }
        }

        // ========================================================================
        // DELETE
        // ========================================================================
        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> ExcluirEspecialidade(int id)
        {
            try
            {
                _logger.LogInformation("Tentativa de exclusão de especialidade ID: {Id}", id);
                var excluiu = await _service.ExcluirEspecialidade(id);

                if (!excluiu)
                {
                    _logger.LogWarning("Especialidade não encontrada para exclusão: {Id}", id);
                    return NotFound(new { message = "Especialidade não encontrada." });
                }

                _logger.LogInformation("Especialidade excluída com sucesso: {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir especialidade: {Id}", id);
                return StatusCode(500, new { message = "Erro interno ao excluir especialidade." });
            }
        }
    }
}
