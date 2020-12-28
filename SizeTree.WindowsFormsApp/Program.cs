using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddScoped<MainWindow>();
                    services.AddSizeTreeCore();
                });
            var host = builder.Build();
            using var serviceScope = host.Services.CreateScope();
            var services = serviceScope.ServiceProvider;
            Application.Run(services.GetRequiredService<MainWindow>());
        }

    }
}
