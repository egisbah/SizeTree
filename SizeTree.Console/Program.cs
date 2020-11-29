using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SizeTree.Core.Helpers;
using System.Threading.Tasks;

namespace SizeTree.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var app = BuildApp(args);
            await app.Run(args, new System.Threading.CancellationToken());
        }
        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostBuilderContext, services) => {
                services.AddSizeTreeCore();
                services.AddSingleton<ISizeTreeApp, SizeTreeApp>();
            });
        static ISizeTreeApp BuildApp(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using IServiceScope serviceScope = host.Services.CreateScope();
            var provider = serviceScope.ServiceProvider;
            var app = provider.GetRequiredService<ISizeTreeApp>();
            return app;
        }
    }
}
