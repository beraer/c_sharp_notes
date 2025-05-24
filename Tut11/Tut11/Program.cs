using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Tut11;

public class Program
{
    public static void Main(string[] args)  
    {  
        var builder = WebApplication.CreateBuilder(args);  
  
        builder.Services.AddDbContext<AppDbContext>(options =>  
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));  
        builder.Services.AddControllers();  
        builder.Services.AddEndpointsApiExplorer();  
        builder.Services.AddSwaggerGen(c =>  
        {  
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "EfPostgresDemo API", Version = "v1" });  
        });    var app = builder.Build();  
        app.MapGet("/", () => "Hello SQL!");  
  
        app.UseSwagger();  
        app.UseSwaggerUI();  
        // Just a test route:  
        app.MapGet("/users", async (AppDbContext db) => await db.Users.ToListAsync());  
  
        app.Run();  
    }
}