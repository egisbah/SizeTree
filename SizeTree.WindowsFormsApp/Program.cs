using Microsoft.Extensions.DependencyInjection;
using SizeTree.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SizeTree.WindowsFormsApp
{
    static class Program
    {
        public static IServiceProvider ServiceProvider { get; set; }
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ConfigureServices();
            Application.Run((Form)ServiceProvider.GetService(typeof(MainWindow)));
        }

        private static void ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddSizeTreeCore();
            services.AddScoped(typeof(MainWindow));
            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
