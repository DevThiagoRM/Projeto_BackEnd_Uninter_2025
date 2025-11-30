using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SistemaHospitalar_API.Application.Constructors.Services;
using SistemaHospitalar_API.Application.Dtos.Consulta;

namespace SistemaHospitalar_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultasController : ControllerBase
    {
        private readonly IConsultaService _service;
        private readonly ILogger<ConsultasController> _logger;

        public ConsultasController(IConsultaService service, ILogger<ConsultasController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // ========================================================================
        // GET ALL
        // ========================================================================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VisualizarConsultaDto>>> ObterConsultas()
        {
            try
            {
                _logger.LogInformation("Iniciando obtenção de todas as consultas.");
                var consultas = await _service.ObterConsultas();
                _logger.LogInformation("Consulta de todas as consultas concluída com sucesso.");
                return Ok(consultas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todas as consultas.");
                return StatusCode(500, new { message = "Erro interno ao obter consultas." });
            }
        }

        // ========================================================================
        // GET BY ID
        // ========================================================================
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<VisualizarConsultaDto>> ObterConsultaPorId(Guid id)
        {
            try
            {
                _logger.LogInformation("Obtendo consulta por ID: {Id}", id);
                var consulta = await _service.ObterConsultasPorId(id);
                if (consulta == null)
                {
                    _logger.LogWarning("Consulta não encontrada para ID: {Id}", id);
                    return NotFound(new { message = "Consulta não encontrada." });
                }

                _logger.LogInformation("Consulta encontrada para ID: {Id}", id);
                return Ok(consulta);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter consulta por ID: {Id}", id);
                return StatusCode(500, new { message = "Erro interno ao obter consulta." });
            }
        }

        // ========================================================================
        // GET BY MEDICO ID
        // ========================================================================
        [HttpGet("medico/{medicoId:guid}")]
        public async Task<ActionResult<IEnumerable<VisualizarConsultaDto>>> ObterConsultasPorMedicoId(Guid medicoId)
        {
            try
            {
                _logger.LogInformation("Obtendo consultas para médico ID: {MedicoId}", medicoId);
                var consultas = await _service.ObterConsultasPorMedicoId(medicoId);
                return Ok(consultas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter consultas por médico ID: {MedicoId}", medicoId);
                return StatusCode(500, new { message = "Erro interno ao obter consultas por médico." });
            }
        }

        // ========================================================================
        // GET BY PACIENTE ID
        // ========================================================================
        [HttpGet("paciente/{pacienteId:guid}")]
        public async Task<ActionResult<IEnumerable<VisualizarConsultaDto>>> ObterConsultasPorPacienteId(Guid pacienteId)
        {
            try
            {
                _logger.LogInformation("Obtendo consultas para paciente ID: {PacienteId}", pacienteId);
                var consultas = await _service.ObterConsultasPorPacienteId(pacienteId);
                return Ok(consultas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter consultas por paciente ID: {PacienteId}", pacienteId);
                return StatusCode(500, new { message = "Erro interno ao obter consultas por paciente." });
            }
        }

        // ========================================================================
        // GET BY MEDICO NOME
        // ========================================================================
        [HttpGet("medico/nome/{nome}")]
        public async Task<ActionResult<IEnumerable<VisualizarConsultaDto>>> ObterConsultasPorMedicoNome(string nome)
        {
            try
            {
                _logger.LogInformation("Obtendo consultas para médico com nome: {Nome}", nome);
                var consultas = await _service.ObterConsultasPorMedicoNome(nome);
                return Ok(consultas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter consultas por nome do médico: {Nome}", nome);
                return StatusCode(500, new { message = "Erro interno ao obter consultas por nome do médico." });
            }
        }

        // ========================================================================
        // GET BY PACIENTE NOME
        // ========================================================================
        [HttpGet("paciente/nome/{nome}")]
        public async Task<ActionResult<IEnumerable<VisualizarConsultaDto>>> ObterConsultasPorPacienteNome(string nome)
        {
            try
            {
                _logger.LogInformation("Obtendo consultas para paciente com nome: {Nome}", nome);
                var consultas = await _service.ObterConsultasPorPacienteNome(nome);
                return Ok(consultas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter consultas por nome do paciente: {Nome}", nome);
                return StatusCode(500, new { message = "Erro interno ao obter consultas por nome do paciente." });
            }
        }

        // ========================================================================
        // GET BY PERÍODO
        // ========================================================================
        [HttpGet("periodo")]
        public async Task<ActionResult<IEnumerable<VisualizarConsultaDto>>> ObterConsultasPorPeriodo(
            [FromQuery] DateTime? dataInicial,
            [FromQuery] DateTime? dataFinal)
        {
            try
            {
                _logger.LogInformation("Obtendo consultas do período {DataInicial} - {DataFinal}", dataInicial, dataFinal);
                var consultas = await _service.ObterConsultasPorPeriodo(dataInicial, dataFinal);
                return Ok(consultas);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Argumento inválido ao obter consultas por período.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao obter consultas por período.");
                return StatusCode(500, new { message = "Erro interno ao obter consultas por período." });
            }
        }

        // ========================================================================
        // POST
        // ========================================================================
        [HttpPost]
        public async Task<ActionResult<VisualizarConsultaDto>> CriarConsulta([FromBody] CriarConsultaDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Dados inválidos ao criar consulta: {ModelState}", ModelState);
                    return BadRequest(ModelState);
                }

                var criada = await _service.CriarConsulta(dto);
                _logger.LogInformation("Consulta criada com sucesso: {Id}", criada.Id);
                return CreatedAtAction(nameof(ObterConsultaPorId), new { id = criada.Id }, criada);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar consulta.");
                return StatusCode(500, new { message = "Erro interno ao criar consulta." });
            }
        }

        // ========================================================================
        // PUT
        // ========================================================================
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<VisualizarConsultaDto>> EditarConsulta(Guid id, [FromBody] EditarConsultaDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Dados inválidos ao editar consulta: {Id}, {ModelState}", id, ModelState);
                    return BadRequest(ModelState);
                }

                var editada = await _service.EditarConsulta(id, dto);
                if (editada == null)
                {
                    _logger.LogWarning("Consulta não encontrada para edição: {Id}", id);
                    return NotFound(new { message = "Consulta não encontrada." });
                }

                _logger.LogInformation("Consulta editada com sucesso: {Id}", id);
                return Ok(editada);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao editar consulta: {Id}", id);
                return StatusCode(500, new { message = "Erro interno ao editar consulta." });
            }
        }

        // ========================================================================
        // CANCELAR CONSULTA
        // ========================================================================
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> CancelarConsulta(Guid id, [FromQuery] string motivo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(motivo))
                {
                    _logger.LogWarning("Tentativa de cancelamento sem motivo para consulta: {Id}", id);
                    return BadRequest(new { message = "É necessário informar o motivo do cancelamento." });
                }

                var cancelado = await _service.CancelarConsulta(id, motivo);
                if (!cancelado)
                {
                    _logger.LogWarning("Consulta não encontrada para cancelamento: {Id}", id);
                    return NotFound(new { message = "Consulta não encontrada." });
                }

                _logger.LogInformation("Consulta cancelada com sucesso: {Id}, Motivo: {Motivo}", id, motivo);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao cancelar consulta: {Id}", id);
                return StatusCode(500, new { message = "Erro interno ao cancelar consulta." });
            }
        }
    }
}
