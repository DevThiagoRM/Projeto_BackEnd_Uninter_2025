using SistemaHospitalar_API.Application.Dtos.Medico;

namespace SistemaHospitalar_API.Application.Constructors.Services
{
    public interface IMedicoService
    {
        Task<VisualizarMedicoDto?> ObterMedicoPorCRM(string crm);
        Task<VisualizarMedicoDto> CriarMedico(Guid id, CriarMedicoDto dto);
        Task<VisualizarMedicoDto?> EditarMedico(Guid id, EditarMedicoDto dto);
        Task<bool> ExcluirMedico(Guid id);
    }
}
