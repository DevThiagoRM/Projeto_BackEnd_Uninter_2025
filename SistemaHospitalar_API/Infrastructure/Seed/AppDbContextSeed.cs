using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SistemaHospitalar_API.Application.Constructors.Services;
using SistemaHospitalar_API.Application.Dtos.Consulta;
using SistemaHospitalar_API.Application.Dtos.Especialidade;
using SistemaHospitalar_API.Application.Dtos.Medico;
using SistemaHospitalar_API.Application.Dtos.Paciente;
using SistemaHospitalar_API.Application.Dtos.Usuario;
using SistemaHospitalar_API.Domain.Entities;

namespace SistemaHospitalar_API.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<Usuario>>();
            var usuarioService = services.GetRequiredService<IUsuarioService>();
            var especialidadeService = services.GetRequiredService<IEspecialidadeService>();

            // ======================
            // SEED ESPECIALIDADES
            // ======================
            var especialidades = new[]
            {
                "Cardiologia",
                "Dermatologia",
                "Neurologia",
                "Ortopedia",
                "Pediatria"
            };

            foreach (var nome in especialidades)
            {
                var existe = await especialidadeService.ObterEspecialidadePorNome(nome);
                if (existe == null)
                {
                    await especialidadeService.CriarEspecialidade(new CriarEspecialidadeDto { Nome = nome });
                }
            }

            // ======================
            // SEED USUÁRIOS
            // ======================
            // Médico
            if (await userManager.FindByEmailAsync("medico1@hospital.com") == null)
            {
                await usuarioService.CriarUsuario(new CriarUsuarioDto
                {
                    NomeCompleto = "Dr. João Silva",
                    NomeExibicao = "Dr. João",
                    Email = "medico1@hospital.com",
                    Password = "Senha123!",
                    DataNascimento = new DateTime(1980, 5, 12),
                    PhoneNumber = "11999999999",
                    IsMedico = true,
                    Medico = new CriarMedicoDto
                    {
                        CRM = "123456",
                        EspecialidadeId = 1
                    }
                });
            }

            // Paciente
            if (await userManager.FindByEmailAsync("paciente1@gmail.com") == null)
            {
                await usuarioService.CriarUsuario(new CriarUsuarioDto
                {
                    NomeCompleto = "Maria Souza",
                    NomeExibicao = "Maria",
                    Email = "paciente1@gmail.com",
                    Password = "Senha123!",
                    DataNascimento = new DateTime(1990, 8, 20),
                    PhoneNumber = "11988888888",
                    IsPaciente = true,
                    Paciente = new CriarPacienteDto
                    {
                        Cpf = "12345678901"
                    }
                });
            }

            // ======================
            // SEED CONSULTAS
            // ======================
            var medico = await usuarioService.ObterUsuarioPorEmail("medico1@hospital.com");
            var paciente = await usuarioService.ObterUsuarioPorEmail("paciente1@gmail.com");

            if (medico != null && paciente != null)
            {
                var consultaService = services.GetRequiredService<IConsultaService>();

                await consultaService.CriarConsulta(new CriarConsultaDto
                {
                    MedicoId = medico.Id,
                    PacienteId = paciente.Id,
                    HorarioConsulta = DateTime.Now.AddDays(1),
                    Observacao = "Consulta de rotina"
                });

                await consultaService.CriarConsulta(new CriarConsultaDto
                {
                    MedicoId = medico.Id,
                    PacienteId = paciente.Id,
                    HorarioConsulta = DateTime.Now.AddDays(2).AddHours(2),
                    Observacao = "Retorno"
                });
            }
        }

        private static async Task<VisualizarEspecialidadeDto?> ObterEspecialidadePorNome(this IEspecialidadeService service, string nome)
        {
            var todas = await service.ObterEspecialidades();
            return todas.FirstOrDefault(e => e.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));
        }
    }
}
