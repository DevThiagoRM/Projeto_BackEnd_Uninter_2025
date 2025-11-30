using SistemaHospitalar_API.Application.Dtos.Consulta;
using SistemaHospitalar_API.Domain.Entities;

namespace SistemaHospitalar_API.Application.Constructors.Services
{
    public interface IConsultaService
    {
        Task<IEnumerable<VisualizarConsultaDto>> ObterConsultas();
        Task<VisualizarConsultaDto?> ObterConsultasPorId(Guid id);
        Task<List<VisualizarConsultaDto>> ObterConsultasPorMedicoId(Guid medicoId);
        Task<List<VisualizarConsultaDto>> ObterConsultasPorMedicoNome(string medicoNome);
        Task<List<VisualizarConsultaDto>> ObterConsultasPorPacienteId(Guid pacienteId);
        Task<List<VisualizarConsultaDto>> ObterConsultasPorPacienteNome(string pacienteNome);
        Task<IEnumerable<VisualizarConsultaDto>> ObterConsultasPorPeriodo(DateTime? dataInicial, DateTime? dataFinal);
        Task<VisualizarConsultaDto> CriarConsulta(CriarConsultaDto consulta);
        Task<VisualizarConsultaDto?> EditarConsulta(Guid id, EditarConsultaDto consulta);
        Task<bool> CancelarConsulta(Guid id, string motivo);
    }
}
