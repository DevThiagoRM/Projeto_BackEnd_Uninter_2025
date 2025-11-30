using SistemaHospitalar_API.Domain.Entities;

namespace SistemaHospitalar_API.Application.Constructors.Repositories
{
    public interface IEspecialidadeRepository
    {
        Task<IEnumerable<Especialidade>> ObterEspecialidades();
        Task<Especialidade?> ObterEspecialidadePorId(int id);
        Task<Especialidade?> ObterEspecialidadePorNome(string nome);
        Task<Especialidade> CriarEspecialidade(Especialidade especialidade);
        Task<Especialidade?> EditarEspecialidade(int id, Especialidade especialidade);
        Task<bool> ExcluirEspecialidade(int id);
    }
}
