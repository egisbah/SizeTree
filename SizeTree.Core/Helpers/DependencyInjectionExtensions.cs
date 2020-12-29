using Microsoft.Extensions.DependencyInjection;
using SizeTree.Core.Services;

namespace SizeTree.Core.Helpers
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddSizeTreeCore (this IServiceCollection sc)
        {
            sc.AddScoped<IFileService, FileService>();
            sc.AddScoped<IOutputService, OutputService>();
            return sc;
        }
    }
}
