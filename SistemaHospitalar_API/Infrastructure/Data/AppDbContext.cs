using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SistemaHospitalar_API.Domain.Entities;

namespace SistemaHospitalar_API.Infrastructure.Data
{
    public class AppDbContext
        : IdentityDbContext<Usuario, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Tabelas do Sistema
        public DbSet<Especialidade> Especialidades { get; set; }
        public DbSet<Consulta> Consultas { get; set; }
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /*
             * USUARIO -> MEDICO (1:1)
             */
            builder.Entity<Medico>()
                .HasKey(m => m.Id);

            builder.Entity<Medico>()
                .HasOne(m => m.Usuario)
                .WithOne(u => u.Medico)
                .HasForeignKey<Medico>(m => m.Id)
                .OnDelete(DeleteBehavior.NoAction);


            /*
             * USUARIO -> PACIENTE (1:1)
             */
            builder.Entity<Paciente>()
                .HasKey(p => p.Id);

            builder.Entity<Paciente>()
                .HasOne(p => p.Usuario)
                .WithOne(u => u.Paciente)
                .HasForeignKey<Paciente>(p => p.Id)
                .OnDelete(DeleteBehavior.NoAction);


            /*
             * ESPECIALIDADE -> MEDICO (1:N)
             */
            builder.Entity<Medico>()
                .HasOne(m => m.Especialidade)
                .WithMany()
                .HasForeignKey(m => m.EspecialidadeId)
                .OnDelete(DeleteBehavior.Restrict);


            /*
             * PACIENTE -> CONSULTA (1:N)
             */
            builder.Entity<Consulta>()
                .HasOne(c => c.Paciente)
                .WithMany()
                .HasForeignKey(c => c.PacienteId)
                .OnDelete(DeleteBehavior.NoAction);


            /*
             * MEDICO -> CONSULTA (1:N)
             */
            builder.Entity<Consulta>()
                .HasOne(c => c.Medico)
                .WithMany()
                .HasForeignKey(c => c.MedicoId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
