using SistemaHospitalar_API.Application.Dtos.Paciente;

namespace SistemaHospitalar_API.Application.Constructors.Services
{
    public interface IPacienteService
    {
        Task<VisualizarPacienteDto> CriarPaciente(Guid id, CriarPacienteDto dto);
        Task<VisualizarPacienteDto?> EditarPaciente(Guid id, EditarPacienteDto dto);
        Task<bool> ExcluirPaciente(Guid id);
    }
}
