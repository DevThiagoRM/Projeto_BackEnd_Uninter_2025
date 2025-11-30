using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SistemaHospitalar_API.Application.Constructors.Repositories;
using SistemaHospitalar_API.Application.Constructors.Services;
using SistemaHospitalar_API.Application.Services;
using SistemaHospitalar_API.Domain.Entities;
using SistemaHospitalar_API.Infrastructure.Data;
using SistemaHospitalar_API.Infrastructure.Persistence.Repositories;
using SistemaHospitalar_API.Infrastructure.Seed;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//=======================================================
// JWT Configuration
//=======================================================
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"] ?? "SuperSecretKey@2025HospitalarAPI");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        ClockSkew = TimeSpan.Zero
    };
});

//=======================================================
// DbContext
//=======================================================
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApiConnectionString"));
});

//=======================================================
// Identity
//=======================================================
builder.Services
    .AddIdentity<Usuario, IdentityRole<Guid>>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = true;

        // Configurações de lockout (opcional)
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
        options.Lockout.MaxFailedAccessAttempts = 5;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

//=======================================================
// Controllers
//=======================================================
builder.Services.AddControllers();

//=======================================================
// Repositories
//=======================================================
builder.Services.AddScoped<IEspecialidadeRepository, EspecialidadeRepository>();
builder.Services.AddScoped<IConsultaRepository, ConsultaRepository>();

//=======================================================
// Services
//=======================================================
builder.Services.AddScoped<IEspecialidadeService, EspecialidadeService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IConsultaService, ConsultaService>();
builder.Services.AddScoped<IAuthService, AuthService>();

//=======================================================
// Swagger com suporte a JWT
//=======================================================
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Sistema Hospitalar",
        Version = "v1",
        Description = "API para gerenciamento de consultas, médicos, pacientes e especialidades."
    });

    // Configuração do JWT no Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    // Habilitar XML Documentation
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);
});

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

//=======================================================
// SEED INICIAL DO BANCO DE DADOS
//=======================================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        var userManager = services.GetRequiredService<UserManager<Usuario>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        // Aplicar migrations automaticamente (opcional)
        await context.Database.MigrateAsync();

        // Executar seed
        await AppDbContextSeed.SeedAsync(context, userManager, roleManager);

        Console.WriteLine("Seed inicial aplicado com sucesso!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro durante o seed: {ex.Message}");
    }
}

//=======================================================
// Middleware Pipeline
//=======================================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// IMPORTANTE: Authentication antes de Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();