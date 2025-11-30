using Microsoft.AspNetCore.Identity;
using SistemaHospitalar_API.Domain.Entities;
using SistemaHospitalar_API.Infrastructure.Data;

namespace SistemaHospitalar_API.Infrastructure.Seed
{
    public static class AppDbContextSeed
    {
        public static async Task SeedAsync(AppDbContext context, UserManager<Usuario> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            await context.Database.EnsureCreatedAsync();

            // Criar roles padrão
            await SeedRolesAsync(roleManager);

            // Criar usuário administrador padrão
            await SeedAdminUserAsync(userManager);

            // Criar especialidades padrão
            await SeedEspecialidadesAsync(context);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole<Guid>> roleManager)
        {
            string[] roles = { "ADMIN", "MEDICO", "PACIENTE", "RECEPCAO" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }
        }

        private static async Task SeedAdminUserAsync(UserManager<Usuario> userManager)
        {
            var adminEmail = "admin@sistemahospitalar.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var admin = new Usuario
                {
                    Id = Guid.NewGuid(),
                    NomeCompleto = "Administrador do Sistema",
                    NomeExibicao = "Admin",
                    Email = adminEmail,
                    UserName = adminEmail,
                    PhoneNumber = "(11) 99999-9999",
                    DataNascimento = new DateTime(1980, 1, 1),
                    Status = true,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "ADMIN");
                }
            }
        }

        private static async Task SeedEspecialidadesAsync(AppDbContext context)
        {
            if (!context.Especialidades.Any())
            {
                var especialidades = new[]
                {
                    new Especialidade { Nome = "Clínica Geral" },
                    new Especialidade { Nome = "Cardiologia" },
                    new Especialidade { Nome = "Dermatologia" },
                    new Especialidade { Nome = "Ortopedia" },
                    new Especialidade { Nome = "Pediatria" },
                    new Especialidade { Nome = "Ginecologia" },
                    new Especialidade { Nome = "Oftalmologia" },
                    new Especialidade { Nome = "Psiquiatria" }
                };

                await context.Especialidades.AddRangeAsync(especialidades);
                await context.SaveChangesAsync();
            }
        }
    }
}
