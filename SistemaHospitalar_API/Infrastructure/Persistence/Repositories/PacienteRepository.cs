using SistemaHospitalar_API.Application.Constructors.Repositories;

namespace SistemaHospitalar_API.Infrastructure.Persistence.Repositories
{
    public class PacienteRepository : IPacienteRepository
    {
        public Task<Paciente> CriarPaciente(Paciente paciente)
        {
            throw new NotImplementedException();
        }

        public Task<Paciente?> EditarPaciente(Guid id, Paciente paciente)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExcluirPaciente(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Paciente?> ObterPacientePorId(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Paciente>> ObterPacientes()
        {
            throw new NotImplementedException();
        }
    }
}
