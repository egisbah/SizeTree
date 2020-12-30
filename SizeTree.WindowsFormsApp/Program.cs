using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using SizeTree.Core.Helpers;
using System;
using System.Windows.Forms;

namespace SizeTree.WindowsFormsApp
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var builder = new HostBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<MainWindow>();
                services.AddSizeTreeCore();
                var serilogLogger = new LoggerConfiguration()
                    .WriteTo.File($"{AppDomain.CurrentDomain.BaseDirectory}\\Logs\\log-ui.txt", rollingInterval: RollingInterval.Day)
                    .CreateLogger();
                services.AddLogging(x =>
                {
                    x.SetMinimumLevel(LogLevel.Information);
                    x.AddSerilog(logger: serilogLogger, dispose: true);
                });
            });

            var host = builder.Build();

            using var serviceScope = host.Services.CreateScope();
            var services = serviceScope.ServiceProvider;
            try
            {
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                var mainWindow = services.GetRequiredService<MainWindow>();
                Application.Run(mainWindow);
                Console.WriteLine("Running");
            }
            catch
            {
                Console.WriteLine("Failed to run");
            }
        }

    }
}
