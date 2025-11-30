using SistemaHospitalar_API.Domain.Entities;

public class Medico
{
    public Guid Id { get; set; }   // mesmo id do usuario

    public string CRM { get; set; } = string.Empty;

    // Especialidade
    public int EspecialidadeId { get; set; }
    public Especialidade Especialidade { get; set; }

    // Navegação
    public Usuario Usuario { get; set; }
}
