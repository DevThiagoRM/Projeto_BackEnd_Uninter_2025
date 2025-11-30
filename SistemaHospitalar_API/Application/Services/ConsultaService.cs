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

        // ====================================================================
        // GET ALL
        // ====================================================================
        public async Task<IEnumerable<VisualizarConsultaDto>> ObterConsultas()
        {
            _logger.LogInformation("Iniciando consulta de todas as consultas.");

            var consultas = await _repo.ObterConsultas();

            if (!consultas.Any())
            {
                _logger.LogWarning("Nenhuma consulta encontrada no banco.");
                return Enumerable.Empty<VisualizarConsultaDto>();
            }

            var resultado = consultas.Select(c => new VisualizarConsultaDto
            {
                Id = c.Id,
                NomePaciente = c.Paciente?.Usuario?.NomeCompleto ?? string.Empty,
                NomeMedico = c.Medico?.Usuario?.NomeCompleto ?? string.Empty,
                EspecialidadeMedico = c.Medico?.Especialidade?.Nome ?? string.Empty,
                HorarioConsulta = c.HorarioConsulta,
                Observacao = c.Observacao,
                Status = c.Status
            });

            _logger.LogInformation("Consulta concluída. Total de registros retornados: {count}", resultado.Count());
            return resultado;
        }

        // ====================================================================
        // GET BY ID
        // ====================================================================
        public async Task<VisualizarConsultaDto?> ObterConsultasPorId(Guid id)
        {
            _logger.LogInformation("Consultando consulta pelo ID: {id}", id);

            var c = await _repo.ObterConsultasPorId(id);
            if (c == null)
            {
                _logger.LogWarning("Consulta não encontrada para ID: {id}", id);
                return null;
            }

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

        // ====================================================================
        // GET BY MEDICO ID
        // ====================================================================
        public async Task<List<VisualizarConsultaDto>> ObterConsultasPorMedicoId(Guid medicoId)
        {
            _logger.LogInformation("Consultando consultas pelo ID do médico: {medicoId}", medicoId);

            var consultas = await _repo.ObterConsultasPorMedicoId(medicoId);
            if (!consultas.Any())
            {
                _logger.LogWarning("Nenhuma consulta encontrada para o médico ID: {medicoId}", medicoId);
                return new List<VisualizarConsultaDto>();
            }

            var resultado = consultas.Select(c => new VisualizarConsultaDto
            {
                Id = c.Id,
                NomePaciente = c.Paciente?.Usuario?.NomeCompleto ?? string.Empty,
                NomeMedico = c.Medico?.Usuario?.NomeCompleto ?? string.Empty,
                EspecialidadeMedico = c.Medico?.Especialidade?.Nome ?? string.Empty,
                HorarioConsulta = c.HorarioConsulta,
                Observacao = c.Observacao,
                Status = c.Status
            }).ToList();

            _logger.LogInformation("Consulta concluída. Total de registros retornados: {count}", resultado.Count);
            return resultado;
        }

        // ====================================================================
        // GET BY PACIENTE ID
        // ====================================================================
        public async Task<List<VisualizarConsultaDto>> ObterConsultasPorPacienteId(Guid pacienteId)
        {
            _logger.LogInformation("Consultando consultas pelo ID do paciente: {pacienteId}", pacienteId);

            var consultas = await _repo.ObterConsultasPorPacienteId(pacienteId);
            if (!consultas.Any())
            {
                _logger.LogWarning("Nenhuma consulta encontrada para o paciente ID: {pacienteId}", pacienteId);
                return new List<VisualizarConsultaDto>();
            }

            var resultado = consultas.Select(c => new VisualizarConsultaDto
            {
                Id = c.Id,
                NomePaciente = c.Paciente?.Usuario?.NomeCompleto ?? string.Empty,
                NomeMedico = c.Medico?.Usuario?.NomeCompleto ?? string.Empty,
                EspecialidadeMedico = c.Medico?.Especialidade?.Nome ?? string.Empty,
                HorarioConsulta = c.HorarioConsulta,
                Observacao = c.Observacao,
                Status = c.Status
            }).ToList();

            _logger.LogInformation("Consulta concluída. Total de registros retornados: {count}", resultado.Count);
            return resultado;
        }

        // ====================================================================
        // GET BY MEDICO NOME
        // ====================================================================
        public async Task<List<VisualizarConsultaDto>> ObterConsultasPorMedicoNome(string medicoNome)
        {
            _logger.LogInformation("Consultando consultas pelo nome do médico: {medicoNome}", medicoNome);

            var consultas = await _repo.ObterConsultasPorMedicoNome(medicoNome);
            if (!consultas.Any())
            {
                _logger.LogWarning("Nenhuma consulta encontrada para o médico: {medicoNome}", medicoNome);
                return new List<VisualizarConsultaDto>();
            }

            var resultado = consultas.Select(c => new VisualizarConsultaDto
            {
                Id = c.Id,
                NomePaciente = c.Paciente?.Usuario?.NomeCompleto ?? string.Empty,
                NomeMedico = c.Medico?.Usuario?.NomeCompleto ?? string.Empty,
                EspecialidadeMedico = c.Medico?.Especialidade?.Nome ?? string.Empty,
                HorarioConsulta = c.HorarioConsulta,
                Observacao = c.Observacao,
                Status = c.Status
            }).ToList();

            _logger.LogInformation("Consulta concluída. Total de registros retornados: {count}", resultado.Count);
            return resultado;
        }

        // ====================================================================
        // GET BY PACIENTE NOME
        // ====================================================================
        public async Task<List<VisualizarConsultaDto>> ObterConsultasPorPacienteNome(string pacienteNome)
        {
            _logger.LogInformation("Consultando consultas pelo nome do paciente: {pacienteNome}", pacienteNome);

            var consultas = await _repo.ObterConsultasPorPacienteNome(pacienteNome);
            if (!consultas.Any())
            {
                _logger.LogWarning("Nenhuma consulta encontrada para o paciente: {pacienteNome}", pacienteNome);
                return new List<VisualizarConsultaDto>();
            }

            var resultado = consultas.Select(c => new VisualizarConsultaDto
            {
                Id = c.Id,
                NomePaciente = c.Paciente?.Usuario?.NomeCompleto ?? string.Empty,
                NomeMedico = c.Medico?.Usuario?.NomeCompleto ?? string.Empty,
                EspecialidadeMedico = c.Medico?.Especialidade?.Nome ?? string.Empty,
                HorarioConsulta = c.HorarioConsulta,
                Observacao = c.Observacao,
                Status = c.Status
            }).ToList();

            _logger.LogInformation("Consulta concluída. Total de registros retornados: {count}", resultado.Count);
            return resultado;
        }

        // ====================================================================
        // GET BY PERIODO
        // ====================================================================
        public async Task<IEnumerable<VisualizarConsultaDto>> ObterConsultasPorPeriodo(DateTime? dataInicial, DateTime? dataFinal)
        {
            _logger.LogInformation("Consultando consultas por período. DataInicial={dataInicial}, DataFinal={dataFinal}", dataInicial, dataFinal);

            if (dataInicial.HasValue && dataFinal.HasValue && dataFinal < dataInicial)
            {
                _logger.LogWarning("Data final menor que data inicial: DataInicial={dataInicial}, DataFinal={dataFinal}", dataInicial, dataFinal);
                throw new ArgumentException("Data final não pode ser menor que a data inicial.");
            }

            var consultas = await _repo.ObterConsultasPorPeriodo(dataInicial, dataFinal);
            if (!consultas.Any())
            {
                _logger.LogWarning("Nenhuma consulta encontrada no período informado.");
                return Enumerable.Empty<VisualizarConsultaDto>();
            }

            var resultado = consultas.Select(c => new VisualizarConsultaDto
            {
                Id = c.Id,
                NomePaciente = c.Paciente?.Usuario?.NomeCompleto ?? string.Empty,
                NomeMedico = c.Medico?.Usuario?.NomeCompleto ?? string.Empty,
                EspecialidadeMedico = c.Medico?.Especialidade?.Nome ?? string.Empty,
                HorarioConsulta = c.HorarioConsulta,
                Observacao = c.Observacao,
                Status = c.Status
            });

            _logger.LogInformation("Consulta concluída. Total de registros retornados: {count}", resultado.Count());
            return resultado;
        }

        // ====================================================================
        // POST
        // ====================================================================
        public async Task<VisualizarConsultaDto> CriarConsulta(CriarConsultaDto dto)
        {
            _logger.LogInformation("Iniciando criação de consulta. MédicoID={medicoId}, PacienteID={pacienteId}, Horario={horario}", dto.MedicoId, dto.PacienteId, dto.HorarioConsulta);

            // Valida médico ocupado
            var consultasMedico = await _repo.ObterConsultasPorMedicoId(dto.MedicoId);
            if (consultasMedico.Any(c => c.HorarioConsulta == dto.HorarioConsulta && c.Status))
            {
                _logger.LogWarning("Falha ao criar consulta: médico ocupado no horário {horario}", dto.HorarioConsulta);
                throw new InvalidOperationException("O médico já possui uma consulta nesse horário.");
            }

            // Valida paciente ocupado
            var consultasPaciente = await _repo.ObterConsultasPorPacienteId(dto.PacienteId);
            if (consultasPaciente.Any(c => c.HorarioConsulta == dto.HorarioConsulta && c.Status))
            {
                _logger.LogWarning("Falha ao criar consulta: paciente ocupado no horário {horario}", dto.HorarioConsulta);
                throw new InvalidOperationException("O paciente já possui uma consulta nesse horário.");
            }

            // Criação da consulta
            var consulta = new Consulta
            {
                PacienteId = dto.PacienteId,
                MedicoId = dto.MedicoId,
                HorarioConsulta = dto.HorarioConsulta,
                Status = true
            };

            var criada = await _repo.CriarConsulta(consulta);

            _logger.LogInformation("Consulta criada com sucesso. ID={id}", criada.Id);

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

        // ====================================================================
        // PUT
        // ====================================================================
        public async Task<VisualizarConsultaDto?> EditarConsulta(Guid id, EditarConsultaDto dto)
        {
            _logger.LogInformation("Iniciando atualização de consulta ID={id}", id);

            // Valida médico ocupado
            var consultasMedico = await _repo.ObterConsultasPorMedicoId(dto.MedicoId);
            if (consultasMedico.Any(c => c.Id != id && c.HorarioConsulta == dto.HorarioConsulta && c.Status))
            {
                _logger.LogWarning("Falha ao atualizar consulta: médico ocupado no horário {horario}", dto.HorarioConsulta);
                throw new InvalidOperationException("O médico já possui uma consulta nesse horário.");
            }

            // Valida paciente ocupado
            var consultasPaciente = await _repo.ObterConsultasPorPacienteId(dto.PacienteId);
            if (consultasPaciente.Any(c => c.Id != id && c.HorarioConsulta == dto.HorarioConsulta && c.Status))
            {
                _logger.LogWarning("Falha ao atualizar consulta: paciente ocupado no horário {horario}", dto.HorarioConsulta);
                throw new InvalidOperationException("O paciente já possui uma consulta nesse horário.");
            }

            var consulta = new Consulta
            {
                PacienteId = dto.PacienteId,
                MedicoId = dto.MedicoId,
                HorarioConsulta = dto.HorarioConsulta,
                Observacao = dto.Observacao ?? string.Empty
            };

            var editada = await _repo.EditarConsulta(id, consulta);
            if (editada == null)
            {
                _logger.LogWarning("Consulta não encontrada para atualização. ID={id}", id);
                return null;
            }

            _logger.LogInformation("Consulta atualizada com sucesso. ID={id}", id);

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

        // ====================================================================
        // CANCELAR CONSULTA
        // ====================================================================
        public async Task<bool> CancelarConsulta(Guid id, string motivo)
        {
            _logger.LogInformation("Tentando cancelar consulta ID={id}", id);

            if (string.IsNullOrWhiteSpace(motivo))
            {
                _logger.LogWarning("Falha ao cancelar consulta ID={id}: motivo não informado", id);
                throw new ArgumentException("É necessário informar o motivo do cancelamento.");
            }

            var cancelou = await _repo.CancelarConsulta(id, motivo);

            if (!cancelou)
            {
                _logger.LogWarning("Falha ao cancelar consulta ID={id}", id);
                return false;
            }

            _logger.LogInformation("Consulta cancelada com sucesso. ID={id}", id);
            return true;
        }
    }
}
