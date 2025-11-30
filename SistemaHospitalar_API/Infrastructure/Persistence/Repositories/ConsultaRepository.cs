using SistemaHospitalar_API.Application.Constructors.Repositories;
using SistemaHospitalar_API.Domain.Entities;

namespace SistemaHospitalar_API.Infrastructure.Persistence.Repositories
{
    public class ConsultaRepository : IConsultaRepository
    {
        public Task<Consulta> CriarConsulta(Consulta consulta)
        {
            throw new NotImplementedException();
        }

        public Task<Consulta?> EditarConsulta(Guid id, Consulta consulta)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExcluirConsulta(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Consulta>> ObterConsultas()
        {
            throw new NotImplementedException();
        }

        public Task<Consulta?> ObterConsultasPorId(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Consulta>> ObterConsultasPorMedicoId(Guid medicoId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Consulta>> ObterConsultasPorPacienteId(Guid pacienteId)
        {
            throw new NotImplementedException();
        }
    }
}
