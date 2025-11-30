using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SistemaHospitalar_API.Domain.Entities;

namespace SistemaHospitalar_API.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        // Tabelas do Sistema

        public DbSet<Especialidade> Especialidades;
        public DbSet<Consulta> Consultas;
        public DbSet<Medico> Medicos;
        public DbSet<Paciente> Pacientes;


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Médico
            builder.Entity<Medico>()
                .HasKey(m => m.Id);

            builder.Entity<Medico>()
                .HasOne(m => m.Usuario)
                .WithOne(u => u.Medico)
                .HasForeignKey<Medico>(m => m.Id);

            // Paciente
            builder.Entity<Paciente>()
                .HasKey(p => p.Id);

            builder.Entity<Paciente>()
                .HasOne(p => p.Usuario)
                .WithOne(u => u.Paciente)
                .HasForeignKey<Paciente>(p => p.Id);
        }
    }
}
