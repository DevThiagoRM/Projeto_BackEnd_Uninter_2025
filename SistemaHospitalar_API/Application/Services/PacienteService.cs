using SistemaHospitalar_API.Application.Constructors.Repositories;
using SistemaHospitalar_API.Application.Constructors.Services;
using SistemaHospitalar_API.Application.Dtos.Medico;
using SistemaHospitalar_API.Application.Dtos.Paciente;
using SistemaHospitalar_API.Domain.Entities;

namespace SistemaHospitalar_API.Application.Services
{
    public class PacienteService : IPacienteService
    {
        private readonly ILogger<PacienteService> _logger;
        private readonly IPacienteRepository _repo;

        public PacienteService(
            ILogger<PacienteService> logger,
            IPacienteRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        // ======================
        // GET
        // ======================
        public async Task<VisualizarPacienteDto> ObterPacientePorCpf(string cpf)
        {
            _logger.LogInformation("Consultando CPF: {cpf}", cpf);

            var especialidade = await _repo.ObterPacientePorCpf(cpf);

            if (especialidade == null)
            {
                _logger.LogWarning("CRM não encontrado: {cpf}", cpf);
                return null;
            }

            return new VisualizarPacienteDto
            {
                Cpf = cpf
            };
        }

        // ======================
        // CREATE
        // ======================
        public async Task<VisualizarPacienteDto> CriarPaciente(Guid id, CriarPacienteDto dto)
        {
            _logger.LogInformation("Iniciando criação de paciente para usuário ID: {id}, CPF: {cpf}", id, dto.Cpf);

            var paciente = new Paciente
            {
                Id = id,
                Cpf = dto.Cpf
            };

            var pacienteCriado = await _repo.CriarPaciente(paciente);

            _logger.LogInformation("Paciente criado com sucesso para usuário ID: {id}, CPF: {cpf}", id, pacienteCriado.Cpf);

            return new VisualizarPacienteDto
            {
                Cpf = pacienteCriado.Cpf
            };
        }

        // ======================
        // EDIT
        // ======================
        public async Task<VisualizarPacienteDto?> EditarPaciente(Guid id, EditarPacienteDto dto)
        {
            _logger.LogInformation("Iniciando edição de paciente para usuário ID: {id}", id);

            _logger.LogDebug("Novo valor de CPF recebido: {cpf}", dto.Cpf);

            var pacienteParaAtualizar = new Paciente
            {
                Cpf = dto.Cpf
            };

            var pacienteAtualizado = await _repo.EditarPaciente(id, pacienteParaAtualizar);

            if (pacienteAtualizado == null)
            {
                _logger.LogWarning("Falha ao atualizar paciente. Usuário ID não encontrado: {id}", id);
                return null;
            }

            _logger.LogInformation("Paciente atualizado com sucesso para usuário ID: {id}, CPF: {cpf}", id, pacienteAtualizado.Cpf);

            return new VisualizarPacienteDto
            {
                Cpf = pacienteAtualizado.Cpf
            };
        }

        // ======================
        // DELETE
        // ======================
        public async Task<bool> ExcluirPaciente(Guid id)
        {
            _logger.LogInformation("Iniciando exclusão de paciente para usuário ID: {id}", id);

            var excluiu = await _repo.ExcluirPaciente(id);

            if (!excluiu)
            {
                _logger.LogWarning("Falha ao excluir paciente. Usuário ID: {id} não encontrado ou já excluído", id);
                return false;
            }

            _logger.LogInformation("Paciente excluído com sucesso para usuário ID: {id}", id);
            return true;
        }

        
    }
}
