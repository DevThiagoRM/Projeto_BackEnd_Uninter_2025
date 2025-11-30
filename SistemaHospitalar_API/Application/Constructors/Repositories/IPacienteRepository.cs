namespace SistemaHospitalar_API.Application.Constructors.Repositories
{
    public interface IPacienteRepository
    {
        Task<IEnumerable<Paciente>> ObterPacientes();
        Task<Paciente?> ObterPacientePorId(Guid id);
        Task<Paciente> CriarPaciente(Paciente paciente);
        Task<Paciente?> EditarPaciente(Guid id, Paciente paciente);
        Task<bool> ExcluirPaciente(Guid id);
    }
}
