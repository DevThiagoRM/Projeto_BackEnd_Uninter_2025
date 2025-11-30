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

        // GET: api/especialidades
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var especialidades = await _service.ObterEspecialidades();
            return Ok(especialidades);
        }

        // GET: api/especialidades/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var especialidade = await _service.ObterEspecialidadePorId(id);

            if (especialidade == null)
                return NotFound(new { message = "Especialidade não encontrada." });

            return Ok(especialidade);
        }

        // POST: api/especialidades
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CriarEspecialidadeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var nova = await _service.CriarEspecialidade(dto);
                return CreatedAtAction(nameof(GetById), new { id = nova.Id }, nova);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Erro ao criar especialidade.");
                return Conflict(new { message = ex.Message });
            }
        }

        // PUT: api/especialidades/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] EditarEspecialidadeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var atualizada = await _service.EditarEspecialidade(id, dto);
                return Ok(atualizada);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Erro ao atualizar especialidade.");
                return NotFound(new { message = ex.Message });
            }
        }

        // DELETE: api/especialidades/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var excluiu = await _service.ExcluirEspecialidade(id);

            if (!excluiu)
                return NotFound(new { message = "Especialidade não encontrada." });

            return NoContent();
        }
    }
}
