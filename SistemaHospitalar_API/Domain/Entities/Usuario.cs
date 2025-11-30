using Microsoft.AspNetCore.Identity;

namespace SistemaHospitalar_API.Domain.Entities
{
    public class Usuario : IdentityUser<Guid>
    {
        // Dados Usuário
        public string NomeCompleto { get; set; } = string.Empty;
        public string NomeExibicao { get; set; } = string.Empty;
        public DateTime? DataNascimento { get; set; }
        public bool AlterarSenhaNoProximoLogin { get; set; } = true;

        // Navegações
        public Medico? Medico { get; set; }
        public Paciente? Paciente { get; set; }

        public bool Status { get; set; }

    }
}
