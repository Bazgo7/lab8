using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ConsoleApp1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Настройка Serilog
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                // Создание хоста с dependency injection
                var host = CreateHostBuilder(args).Build();

                // Применение миграций базы данных
                using (var scope = host.Services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<Lab8DbContext>();
                    context.Database.Migrate();
                }

                // Запуск контроллера
                using (var scope = host.Services.CreateScope())
                {
                    var controller = scope.ServiceProvider.GetRequiredService<TriangleController>();
                    await controller.ProcessTriangleAsync();
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Произошла критическая ошибка");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices((context, services) =>
                {
                    // Настройка подключения к базе данных
                    services.AddDbContext<Lab8DbContext>(options =>
                        options.UseSqlServer("Server=DESKTOP-LVGNPOO; Database=lab8; Trusted_Connection=True; TrustServerCertificate=True"));

                    // Регистрация сервисов
                    services.AddScoped<IDatabaseService, DatabaseService>();
                    services.AddScoped<IUserInterface, UserInterface>();
                    services.AddScoped<IEmailService, EmailService>();
                    services.AddScoped<TriangleController>();
                });
    }
}
