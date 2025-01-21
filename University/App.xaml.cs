using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using University.DataLayer;

namespace University
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<App>()
                .Build();

            var services = new ServiceCollection();

            services.AddDbContext<UniversityContext>(options =>
            {
                options.UseSqlServer(connectionString: config["DbConnectionString"]);
            });

            ServiceProvider = services.BuildServiceProvider();

            base.OnStartup(e);
        }
    }
}
