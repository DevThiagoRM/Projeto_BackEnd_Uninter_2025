using SistemaHospitalar_API.Application.Constructors.Repositories;
using SistemaHospitalar_API.Application.Constructors.Services;
using SistemaHospitalar_API.Application.Dtos.Medico;
using SistemaHospitalar_API.Application.Dtos.Paciente;

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

        public async Task<VisualizarPacienteDto> CriarPaciente(Guid id, CriarPacienteDto dto)
        {
            var paciente = new Paciente
            {
                Id = id,
                Cpf = dto.Cpf
            };

            var pacienteCriado = await _repo.CriarPaciente(paciente);

            return new VisualizarPacienteDto
            {
                Cpf = pacienteCriado.Cpf
            };
        }

        public async Task<VisualizarPacienteDto?> EditarPaciente(Guid id, EditarPacienteDto dto)
        {
            // Cria uma entidade parcial para atualização
            var pacienteParaAtualizar = new Paciente
            {
                Cpf = dto.Cpf
            };

            var pacienteAtualizado = await _repo.EditarPaciente(id, pacienteParaAtualizar);

            if (pacienteAtualizado == null)
                return null;

            return new VisualizarPacienteDto
            {
                Cpf = pacienteAtualizado.Cpf
            };
        }

        public async Task<bool> ExcluirPaciente(Guid id)
        {
            return await _repo.ExcluirPaciente(id);
        }
    }
}
