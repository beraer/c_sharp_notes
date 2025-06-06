using Microsoft.EntityFrameworkCore;
using APBDtut12.Data;
using Microsoft.OpenApi.Models;
using Tutorial12.Data;
using Tutorial12.Services;

namespace Tutorial12;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddControllers();
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());
        builder.Services.AddAuthorization();
        builder.Services.AddScoped<ITripService, TripService>();
        builder.Services.AddScoped<IClientService, ClientService>();


        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tutorial12 API", Version = "v1" });
        });

        var app = builder.Build();

        app.MapControllers();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }
        
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tutorial12 API v1");
            c.RoutePrefix = string.Empty; 
        });

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.Run();
    }
}