using SistemaHospitalar_API.Application.Dtos.Especialidade;
using SistemaHospitalar_API.Domain.Entities;

namespace SistemaHospitalar_API.Application.Constructors.Services
{
    public interface IEspecialidadeService
    {
        Task<IEnumerable<VisualizarEspecialidadeDto>> ObterEspecialidades();
        Task<VisualizarEspecialidadeDto?> ObterEspecialidadePorId(int id);
        Task<VisualizarEspecialidadeDto> CriarEspecialidade(CriarEspecialidadeDto dto);
        Task<VisualizarEspecialidadeDto?> EditarEspecialidade(int id, EditarEspecialidadeDto dto);
        Task<bool> ExcluirEspecialidade(int id);
    }
}
