using Microsoft.AspNetCore.Mvc;
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

        // ============================================================================
        // GET ALL
        // ============================================================================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VisualizarEspecialidadeDto>>> ObterEspecialidades()
        {
            var especialidades = await _service.ObterEspecialidades();
            return Ok(especialidades);
        }

        // ============================================================================
        // GET BY ID
        // ============================================================================
        [HttpGet("{id:int}")]
        public async Task<ActionResult<VisualizarEspecialidadeDto?>> ObterEspecialidadePorId(int id)
        {
            var especialidade = await _service.ObterEspecialidadePorId(id);

            if (especialidade == null)
                return NotFound(new { message = "Especialidade não encontrada." });

            return Ok(especialidade);
        }

        // ============================================================================
        // POST
        // ============================================================================
        [HttpPost]
        public async Task<ActionResult<VisualizarEspecialidadeDto>> CriarEspecialidade([FromBody] CriarEspecialidadeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var nova = await _service.CriarEspecialidade(dto);
                return CreatedAtAction(nameof(ObterEspecialidadePorId), new { id = nova.Id }, nova);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Erro ao criar especialidade.");
                return Conflict(new { message = ex.Message });
            }
        }

        // ============================================================================
        // PUT
        // ============================================================================
        [HttpPut("{id:int}")]
        public async Task<ActionResult<VisualizarEspecialidadeDto?>> EditarEspecialidade(int id, [FromBody] EditarEspecialidadeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var atualizada = await _service.EditarEspecialidade(id, dto);
                if (atualizada == null)
                    return NotFound(new { message = "Especialidade não encontrada." });

                return Ok(atualizada);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Erro ao atualizar especialidade.");
                return BadRequest(new { message = ex.Message });
            }
        }

        // ============================================================================
        // DELETE
        // ============================================================================
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> ExcluirEspecialidade(int id)
        {
            var excluiu = await _service.ExcluirEspecialidade(id);

            if (!excluiu)
                return NotFound(new { message = "Especialidade não encontrada." });

            return NoContent();
        }
    }
}
