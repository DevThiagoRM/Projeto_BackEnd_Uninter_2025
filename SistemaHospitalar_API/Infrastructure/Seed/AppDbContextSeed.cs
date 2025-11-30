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
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var usuarioService = services.GetRequiredService<IUsuarioService>();
            var especialidadeService = services.GetRequiredService<IEspecialidadeService>();

            // ======================
            // SEED ROLES (CRÍTICO - PARA AUTORIZAÇÃO)
            // ======================
            await SeedRolesAsync(roleManager);

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
            // SEED USUÁRIOS ADMIN (PRIMEIRO)
            // ======================
            if (await userManager.FindByEmailAsync("admin@hospital.com") == null)
            {
                var adminUser = new Usuario
                {
                    NomeCompleto = "Administrador Sistema",
                    NomeExibicao = "Admin",
                    Email = "admin@hospital.com",
                    UserName = "admin@hospital.com",
                    DataNascimento = new DateTime(1985, 1, 1),
                    PhoneNumber = "11999999999"
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "ADMIN");
                }
            }

            // ======================
            // SEED MÉDICO
            // ======================
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

            // ======================
            // SEED PACIENTE
            // ======================
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
            // SEED RECEPCIONISTA
            // ======================
            if (await userManager.FindByEmailAsync("recepcionista@hospital.com") == null)
            {
                var recepcionistaUser = new Usuario
                {
                    NomeCompleto = "Ana Recepcionista",
                    NomeExibicao = "Ana",
                    Email = "recepcionista@hospital.com",
                    UserName = "recepcionista@hospital.com",
                    DataNascimento = new DateTime(1992, 3, 15),
                    PhoneNumber = "11977777777"
                };

                var result = await userManager.CreateAsync(recepcionistaUser, "Recepcao123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(recepcionistaUser, "RECEPCAO");
                }
            }

            // ======================
            // SEED CONSULTAS
            // ======================
            var medico = await usuarioService.ObterUsuarioPorEmail("medico1@hospital.com");
            var paciente = await usuarioService.ObterUsuarioPorEmail("paciente1@gmail.com");

            if (medico != null && paciente != null)
            {
                var consultaService = services.GetRequiredService<IConsultaService>();

                // Limpar consultas existentes para evitar conflitos
                var consultasExistentes = await consultaService.ObterConsultas();

                // Criar apenas se não houver consultas
                if (!consultasExistentes.Any())
                {
                    await consultaService.CriarConsulta(new CriarConsultaDto
                    {
                        MedicoId = medico.Id,
                        PacienteId = paciente.Id,
                        HorarioConsulta = DateTime.Now.AddDays(1).AddHours(10), // Amanhã 10:00
                        Observacao = "Consulta de rotina"
                    });

                    await consultaService.CriarConsulta(new CriarConsultaDto
                    {
                        MedicoId = medico.Id,
                        PacienteId = paciente.Id,
                        HorarioConsulta = DateTime.Now.AddDays(2).AddHours(14), // Depois de amanhã 14:00
                        Observacao = "Retorno"
                    });
                }
            }
        }

        // ======================
        // MÉTODO PARA CRIAR ROLES
        // ======================
        private static async Task SeedRolesAsync(RoleManager<IdentityRole<Guid>> roleManager)
        {
            string[] roles = { "ADMIN", "MEDICO", "PACIENTE", "RECEPCAO" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                    Console.WriteLine($"Role '{role}' criada com sucesso.");
                }
                else
                {
                    Console.WriteLine($"Role '{role}' já existe.");
                }
            }
        }

        private static async Task<VisualizarEspecialidadeDto?> ObterEspecialidadePorNome(this IEspecialidadeService service, string nome)
        {
            var todas = await service.ObterEspecialidades();
            return todas.FirstOrDefault(e => e.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));
        }
    }
}