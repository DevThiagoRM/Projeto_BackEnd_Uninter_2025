using SistemaHospitalar_API.Application.Dtos.Usuario;
using SistemaHospitalar_API.Domain.Entities;

namespace SistemaHospitalar_API.Application.Constructors.Services
{
    public interface IUsuarioService
    {
        Task<IEnumerable<VisualizarUsuarioDto>> ObterUsuarios();
        Task<VisualizarUsuarioDto?> ObterUsuarioPorId(Guid id);
        Task<VisualizarUsuarioDto?> ObterUsuarioPorEmail(string email);
        Task<VisualizarUsuarioDto> CriarUsuario(CriarUsuarioDto dto);
        Task<VisualizarUsuarioDto?> EditarUsuario(Guid id, EditarUsuarioDto dto);
        Task<bool> ExcluirUsuario(Guid id);
    }
}
