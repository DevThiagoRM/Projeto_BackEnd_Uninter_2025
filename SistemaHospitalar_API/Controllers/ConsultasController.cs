using Microsoft.AspNetCore.Mvc;
using SistemaHospitalar_API.Application.Constructors.Services;
using SistemaHospitalar_API.Application.Dtos.Consulta;

namespace SistemaHospitalar_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultasController : ControllerBase
    {
        private readonly IConsultaService _service;

        public ConsultasController(IConsultaService service)
        {
            _service = service;
        }

        // ============================================================================
        // GET ALL
        // ============================================================================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VisualizarConsultaDto>>> ObterConsultas()
        {
            var consultas = await _service.ObterConsultas();
            return Ok(consultas);
        }

        // ============================================================================
        // GET BY ID
        // ============================================================================
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<VisualizarConsultaDto>> ObterConsultaPorId(Guid id)
        {
            var consulta = await _service.ObterConsultasPorId(id);
            if (consulta == null)
                return NotFound(new { message = "Consulta não encontrada." });

            return Ok(consulta);
        }

        // ============================================================================
        // GET BY MEDICO ID
        // ============================================================================
        [HttpGet("medico/{medicoId:guid}")]
        public async Task<ActionResult<IEnumerable<VisualizarConsultaDto>>> ObterConsultasPorMedicoId(Guid medicoId)
        {
            var consultas = await _service.ObterConsultasPorMedicoId(medicoId);
            return Ok(consultas);
        }

        // ============================================================================
        // GET BY PACIENTE ID
        // ============================================================================
        [HttpGet("paciente/{pacienteId:guid}")]
        public async Task<ActionResult<IEnumerable<VisualizarConsultaDto>>> ObterConsultasPorPacienteId(Guid pacienteId)
        {
            var consultas = await _service.ObterConsultasPorPacienteId(pacienteId);
            return Ok(consultas);
        }

        // ============================================================================
        // GET BY MEDICO NOME
        // ============================================================================
        [HttpGet("medico/nome/{nome}")]
        public async Task<ActionResult<IEnumerable<VisualizarConsultaDto>>> ObterConsultasPorMedicoNome(string nome)
        {
            var consultas = await _service.ObterConsultasPorMedicoNome(nome);
            return Ok(consultas);
        }

        // ============================================================================
        // GET BY PACIENTE NOME
        // ============================================================================
        [HttpGet("paciente/nome/{nome}")]
        public async Task<ActionResult<IEnumerable<VisualizarConsultaDto>>> ObterConsultasPorPacienteNome(string nome)
        {
            var consultas = await _service.ObterConsultasPorPacienteNome(nome);
            return Ok(consultas);
        }

        // ============================================================================
        // POST
        // ============================================================================
        [HttpPost]
        public async Task<ActionResult<VisualizarConsultaDto>> CriarConsulta([FromBody] CriarConsultaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var criada = await _service.CriarConsulta(dto);
            return CreatedAtAction(nameof(ObterConsultaPorId), new { id = criada.Id }, criada);
        }

        // ============================================================================
        // PUT
        // ============================================================================
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<VisualizarConsultaDto>> EditarConsulta(Guid id, [FromBody] EditarConsultaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var editada = await _service.EditarConsulta(id, dto);
            if (editada == null)
                return NotFound(new { message = "Consulta não encontrada." });

            return Ok(editada);
        }

        // ============================================================================
        // CANCELAR CONSULTA
        // ============================================================================
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> CancelarConsulta(Guid id, [FromQuery] string motivo)
        {
            if (string.IsNullOrWhiteSpace(motivo))
                return BadRequest(new { message = "É necessário informar o motivo do cancelamento." });

            var cancelado = await _service.CancelarConsulta(id, motivo);
            if (!cancelado)
                return NotFound(new { message = "Consulta não encontrada." });

            return NoContent();
        }
    }
}
