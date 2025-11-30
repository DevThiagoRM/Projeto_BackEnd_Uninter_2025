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

        // Auditoria Usuário
        public DateTime CriadoEm { get; set; }
        public Guid CriadoPorId { get; set; }

        public DateTime? UltimaAlteracao { get; set; }
        public Guid AlteradoPorId { get; set; }

        public DateTime? ExcluidoEm { get; set; }
        public Guid ExcluidoPorId { get; set; }

        public bool Status { get; set; }

    }
}
