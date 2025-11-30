using SistemaHospitalar_API.Domain.Entities;

public class Paciente
{
    public Guid Id { get; set; }   // mesmo id do usuario

    public string Cpf { get; set; } = string.Empty;

    // Navegação
    public Usuario? Usuario { get; set; }
}
