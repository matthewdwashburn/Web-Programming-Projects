using CloudStorage.Services;

namespace CloudStorage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSingleton<IImageTableStorage, ImageTableStorage>();
            builder.Services.AddSingleton<IUserNameProvider, UserNameProvider>();
            builder.Services.AddSingleton<IBlobServiceClientProvider, BlobServiceClientProvider>();
            builder.Services.AddSingleton<IConnectionStringProvider, ConnectionStringProvider>();
            builder.Services.AddSingleton<KeyVaultProvider>();
            builder.Services.AddSingleton<SecretProvider>();

            var app = builder.Build();

            app.UseCors(policy =>
                policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
            );

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Services.GetRequiredService<IImageTableStorage>().StartupAsync();

            app.Run();
        }
    }
}