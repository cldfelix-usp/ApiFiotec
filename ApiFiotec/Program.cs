
using ApiFiotec.Contracts;
using ApiFiotec.Infraestruture.Data;
using ApiFiotec.Repositories;
using ApiFiotec.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiFiotec
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddAutoMapper(typeof(Program).Assembly);
            
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "InfoDengue API",
                    Version = "v1",
                    Description = "API para consulta de dados epidemiológicos de dengue, zika e chikungunya",
                    Contact = new OpenApiContact
                    {
                        Name = "Administrador",
                        Email = "admin@example.com"
                    }
                });
                
                // Inclui os comentários XML para a documentação do Swagger (opcional)
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (System.IO.File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            });



            // Configurar banco de dados
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Configurar CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
            
            // Configuração de cache 
            builder.Services.AddResponseCaching();

            // Add services to the container.
            // Adicionar servi�os ao container
            builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                })
                .AddJsonOptions(options =>
                {
                    // remove null values
                    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                    //options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                    //options.JsonSerializerOptions.WriteIndented = true;

                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddHttpClient<IInfoDengueService, InfoDengueService>();
            
            builder.Services.AddScoped<IEstadosRepository, EstadosRepository>();
            builder.Services.AddScoped<IEstadosService, EstadosService>();

            builder.Services.AddScoped<IMunicipiosRepository, MunicipiosRepository>();
            builder.Services.AddScoped<IMunicipiosService, MunicipiosService>();

            builder.Services.AddScoped<ISolicitanteRepository, SolicitanteRepository>();
            builder.Services.AddScoped<ISolicitanteService, SolicitanteService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "InfoDengue API v1"));
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors("AllowAll");

            app.MapControllers();

            app.Run();
        }
    }
}
