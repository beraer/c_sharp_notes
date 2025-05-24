using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Tutorial11.API.Data;
using Tutorial11.API.DTOs;
using Tutorial11.API.Services;

namespace Tutorial11.API;

public class Program
{
    public static void Main(string[] args)  
    {  
        var builder = WebApplication.CreateBuilder(args);  
  
        builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
        builder.Services.AddDbContext<ApplicationDbContext>(options =>  
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));  
        builder.Services.AddControllers();  
        builder.Services.AddEndpointsApiExplorer();  
        builder.Services.AddSwaggerGen(c =>  
        {  
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "EfPostgresDemo API", Version = "v1" });  
        });    var app = builder.Build();  
        app.MapGet("/", () => "Hello SQL!");  
  
        app.UseAuthorization();
        app.UseSwagger();  
        app.UseSwaggerUI();  
        app.MapControllers();
        app.Run();  
    }
}