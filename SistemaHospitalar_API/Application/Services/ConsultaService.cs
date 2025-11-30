using SistemaHospitalar_API.Application.Constructors.Repositories;
using SistemaHospitalar_API.Application.Constructors.Services;
using SistemaHospitalar_API.Application.Dtos.Consulta;
using SistemaHospitalar_API.Domain.Entities;

namespace SistemaHospitalar_API.Application.Services
{
    public class ConsultaService : IConsultaService
    {
        private readonly ILogger<ConsultaService> _logger;
        private readonly IConsultaRepository _repo;

        public ConsultaService(
            ILogger<ConsultaService> logger,
            IConsultaRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        // ============================================================================
        // GET ALL
        // ============================================================================
        public async Task<IEnumerable<VisualizarConsultaDto>> ObterConsultas()
        {
            var consultas = await _repo.ObterConsultas();

            return consultas.Select(c => new VisualizarConsultaDto
            {
                Id = c.Id,
                NomePaciente = c.Paciente?.Usuario?.NomeCompleto ?? string.Empty,
                NomeMedico = c.Medico?.Usuario?.NomeCompleto ?? string.Empty,
                EspecialidadeMedico = c.Medico?.Especialidade?.Nome ?? string.Empty,
                HorarioConsulta = c.HorarioConsulta,
                Observacao = c.Observacao,
                Status = c.Status
            });
        }

        // ============================================================================
        // GET BY ID
        // ============================================================================
        public async Task<VisualizarConsultaDto?> ObterConsultasPorId(Guid id)
        {
            var c = await _repo.ObterConsultasPorId(id);
            if (c == null)
                return null;

            return new VisualizarConsultaDto
            {
                Id = c.Id,
                NomePaciente = c.Paciente?.Usuario?.NomeCompleto ?? string.Empty,
                NomeMedico = c.Medico?.Usuario?.NomeCompleto ?? string.Empty,
                EspecialidadeMedico = c.Medico?.Especialidade?.Nome ?? string.Empty,
                HorarioConsulta = c.HorarioConsulta,
                Observacao = c.Observacao,
                Status = c.Status
            };
        }

        // ============================================================================
        // GET BY MEDICO ID
        // ============================================================================
        public async Task<List<VisualizarConsultaDto>> ObterConsultasPorMedicoId(Guid medicoId)
        {
            var consultas = await _repo.ObterConsultasPorMedicoId(medicoId);

            return consultas.Select(c => new VisualizarConsultaDto
            {
                Id = c.Id,
                NomePaciente = c.Paciente?.Usuario?.NomeCompleto ?? string.Empty,
                NomeMedico = c.Medico?.Usuario?.NomeCompleto ?? string.Empty,
                EspecialidadeMedico = c.Medico?.Especialidade?.Nome ?? string.Empty,
                HorarioConsulta = c.HorarioConsulta,
                Observacao = c.Observacao,
                Status = c.Status
            }).ToList();
        }

        // ============================================================================
        // GET BY PACIENTE ID
        // ============================================================================
        public async Task<List<VisualizarConsultaDto>> ObterConsultasPorPacienteId(Guid pacienteId)
        {
            var consultas = await _repo.ObterConsultasPorPacienteId(pacienteId);

            return consultas.Select(c => new VisualizarConsultaDto
            {
                Id = c.Id,
                NomePaciente = c.Paciente?.Usuario?.NomeCompleto ?? string.Empty,
                NomeMedico = c.Medico?.Usuario?.NomeCompleto ?? string.Empty,
                EspecialidadeMedico = c.Medico?.Especialidade?.Nome ?? string.Empty,
                HorarioConsulta = c.HorarioConsulta,
                Observacao = c.Observacao,
                Status = c.Status
            }).ToList();
        }

        // ============================================================================
        // GET BY MEDICO NOME
        // ============================================================================
        public async Task<List<VisualizarConsultaDto>> ObterConsultasPorMedicoNome(string medicoNome)
        {
            var consultas = await _repo.ObterConsultasPorMedicoNome(medicoNome);

            return consultas.Select(c => new VisualizarConsultaDto
            {
                Id = c.Id,
                NomePaciente = c.Paciente?.Usuario?.NomeCompleto ?? string.Empty,
                NomeMedico = c.Medico?.Usuario?.NomeCompleto ?? string.Empty,
                EspecialidadeMedico = c.Medico?.Especialidade?.Nome ?? string.Empty,
                HorarioConsulta = c.HorarioConsulta,
                Observacao = c.Observacao,
                Status = c.Status
            }).ToList();
        }

        // ============================================================================
        // GET BY PACIENTE NOME
        // ============================================================================
        public async Task<List<VisualizarConsultaDto>> ObterConsultasPorPacienteNome(string pacienteNome)
        {
            var consultas = await _repo.ObterConsultasPorPacienteNome(pacienteNome);

            return consultas.Select(c => new VisualizarConsultaDto
            {
                Id = c.Id,
                NomePaciente = c.Paciente?.Usuario?.NomeCompleto ?? string.Empty,
                NomeMedico = c.Medico?.Usuario?.NomeCompleto ?? string.Empty,
                EspecialidadeMedico = c.Medico?.Especialidade?.Nome ?? string.Empty,
                HorarioConsulta = c.HorarioConsulta,
                Observacao = c.Observacao,
                Status = c.Status
            }).ToList();
        }

        // ============================================================================
        // GET BY PERIODO
        // ============================================================================
        public async Task<IEnumerable<VisualizarConsultaDto>> ObterConsultasPorPeriodo(DateTime? dataInicial, DateTime? dataFinal)
        {
            if (dataInicial.HasValue && dataFinal.HasValue && dataFinal < dataInicial)
                throw new ArgumentException("Data final não pode ser menor que a data inicial.");

            var consultas = await _repo.ObterConsultasPorPeriodo(dataInicial, dataFinal);

            return consultas.Select(c => new VisualizarConsultaDto
            {
                Id = c.Id,
                HorarioConsulta = c.HorarioConsulta,
                NomeMedico = c.Medico?.Usuario?.NomeCompleto ?? string.Empty,
                NomePaciente = c.Paciente?.Usuario?.NomeCompleto ?? string.Empty,
                EspecialidadeMedico = c.Medico?.Especialidade?.Nome ?? string.Empty,
                Observacao = c.Observacao,
                Status = c.Status
            });
        }

        // ============================================================================
        // POST
        // ============================================================================
        public async Task<VisualizarConsultaDto> CriarConsulta(CriarConsultaDto dto)
        {
            var consulta = new Consulta
            {
                PacienteId = dto.PacienteId,
                MedicoId = dto.MedicoId,
                HorarioConsulta = dto.HorarioConsulta,
                Status = true
            };

            var criada = await _repo.CriarConsulta(consulta);

            return new VisualizarConsultaDto
            {
                Id = criada.Id,
                NomePaciente = criada.Paciente?.Usuario?.NomeCompleto ?? string.Empty,
                NomeMedico = criada.Medico?.Usuario?.NomeCompleto ?? string.Empty,
                EspecialidadeMedico = criada.Medico?.Especialidade?.Nome ?? string.Empty,
                HorarioConsulta = criada.HorarioConsulta,
                Observacao = criada.Observacao,
                Status = criada.Status
            };
        }

        // ============================================================================
        // PUT
        // ============================================================================
        public async Task<VisualizarConsultaDto?> EditarConsulta(Guid id, EditarConsultaDto dto)
        {
            var consulta = new Consulta
            {
                PacienteId = dto.PacienteId,
                MedicoId = dto.MedicoId,
                HorarioConsulta = dto.HorarioConsulta
            };

            var editada = await _repo.EditarConsulta(id, consulta);

            if (editada == null)
                return null;

            return new VisualizarConsultaDto
            {
                Id = editada.Id,
                NomePaciente = editada.Paciente?.Usuario?.NomeCompleto ?? string.Empty,
                NomeMedico = editada.Medico?.Usuario?.NomeCompleto ?? string.Empty,
                EspecialidadeMedico = editada.Medico?.Especialidade?.Nome ?? string.Empty,
                HorarioConsulta = editada.HorarioConsulta,
                Observacao = editada.Observacao,
                Status = editada.Status
            };
        }

        // ============================================================================
        // CANCELAR CONSULTA
        // ============================================================================
        public async Task<bool> CancelarConsulta(Guid id, string motivo)
        {
            return await _repo.CancelarConsulta(id, motivo);
        }


    }
}
