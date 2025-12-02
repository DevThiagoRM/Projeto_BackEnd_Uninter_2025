namespace SistemaHospitalar_API.Application.Constructors.Repositories
{
    public interface IMedicoRepository
    {
        Task<Medico> ObterMedicoPorCRM(string crm);
        Task<Medico> CriarMedico(Medico medico);
        Task<Medico?> EditarMedico(Guid id, Medico medico);
        Task<bool> ExcluirMedico(Guid id);

    }
}
