using SistemaHospitalar_API.Application.Dtos.Consulta;
using SistemaHospitalar_API.Domain.Entities;

namespace SistemaHospitalar_API.Application.Constructors.Repositories
{
    public interface IConsultaRepository
    {
        Task<IEnumerable<Consulta>> ObterConsultas();
        Task<Consulta?> ObterConsultasPorId(Guid id);
        Task<List<Consulta>> ObterConsultasPorMedicoId(Guid medicoId);
        Task<List<Consulta>> ObterConsultasPorMedicoNome(string medicoNome);
        Task<List<Consulta>> ObterConsultasPorPacienteId(Guid pacienteId);
        Task<List<Consulta>> ObterConsultasPorPacienteNome(string pacienteNome);
        Task<Consulta> CriarConsulta(Consulta consulta);
        Task<Consulta?> EditarConsulta(Guid id, Consulta consulta);
        Task<bool> CancelarConsulta(Guid id, string motivo);
    }
}
