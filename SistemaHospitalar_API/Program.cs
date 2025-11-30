using Microsoft.EntityFrameworkCore;
using SistemaHospitalar_API.Application.Constructors.Repositories;
using SistemaHospitalar_API.Application.Constructors.Services;
using SistemaHospitalar_API.Application.Services;
using SistemaHospitalar_API.Infrastructure.Data;
using SistemaHospitalar_API.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

//=======================================================
// DbContext
//=======================================================
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApiConnectionString"));
});

//=======================================================
// Controllers
//=======================================================
builder.Services.AddControllers();

//=======================================================
// Repositories
//=======================================================
builder.Services.AddScoped<IEspecialidadeRepository, EspecialidadeRepository>();


//=======================================================
// Services
//=======================================================
builder.Services.AddScoped<IEspecialidadeService, EspecialidadeService>();

//=======================================================
// Swagger
//=======================================================
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "API Sistema Hospitalar",
        Version = "v1",
        Description = "API para gerenciamento de consultas, médicos, pacientes e especialidades."
    });

    // Habilitar XML Documentation (opcional, mas recomendado)
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);
});


builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

//=======================================================
// Enviroment
//=======================================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
