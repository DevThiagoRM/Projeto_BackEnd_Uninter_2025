namespace SistemaHospitalar_API.Application.Dtos.Usuario
{
    public class AlterarSenhaDto
    {
        public string SenhaAntiga { get; set; } = string.Empty;
        public string NovaSenha { get; set; } = string.Empty;
    }
}
