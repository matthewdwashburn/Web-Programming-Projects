using DependencyInjection.Filters;
using DependencyInjection.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjection
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to container
            builder.Services.AddSingleton<MemoryDatabase>();
            builder.Services.AddScoped<StopwatchService>();
            builder.Services.AddSingleton<DependencyInjection.Services.ILogger>(provider => ConsoleLogger.Instance);
            builder.Services.AddScoped<RequestIdFilter>();
            builder.Services.AddScoped<StopwatchFilter>();
            builder.Services.AddTransient<RequestIdGenerator>();
            builder.Services.AddScoped<RequestLoggingFilter>();



            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();


            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseCors(policy => policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithExposedHeaders("*"));

            app.UseAuthorization();

            app.MapControllers();


            app.Run();

        }
    }
}
