using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using University.DataLayer;
using University.Domain.Utilities;
using University.Domain.ViewModels;

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

            services.AddDbContext<UniversityContext>(options => options.UseSqlServer(connectionString: config["DbConnectionString"]));

            services.AddScoped<IMessageBoxService, MessageBoxService>();

            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<MainViewModel>();

            services.AddTransient<MainWindow>();

            ServiceProvider = services.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();

            this.DispatcherUnhandledException += HandleException;

            base.OnStartup(e);
        }

        private void HandleException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, "Exception occured", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}
