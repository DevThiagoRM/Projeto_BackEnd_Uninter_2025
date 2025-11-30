using SistemaHospitalar_API.Application.Dtos.Consulta;
using SistemaHospitalar_API.Domain.Entities;

namespace SistemaHospitalar_API.Application.Constructors.Repositories
{
    public interface IConsultaRepository
    {
        Task<IEnumerable<Consulta>> ObterConsultas();
        Task<Consulta?> ObterConsultasPorId(Guid id);
        Task<IEnumerable<Consulta>> ObterConsultasPorMedicoId(Guid medicoId);
        Task<IEnumerable<Consulta>> ObterConsultasPorMedicoNome(string medicoNome);
        Task<IEnumerable<Consulta>> ObterConsultasPorPacienteId(Guid pacienteId);
        Task<IEnumerable<Consulta>> ObterConsultasPorPacienteNome(string pacienteNome);
        Task<IEnumerable<Consulta>> ObterConsultasPorPeriodo(DateTime? dataInicial, DateTime? dataFinal);
        Task<Consulta> CriarConsulta(Consulta consulta);
        Task<Consulta?> EditarConsulta(Guid id, Consulta consulta);
        Task<bool> CancelarConsulta(Guid id, string motivo);
    }
}
