using System.Windows;
using University.Domain.ViewModels;
using University.Views;

namespace University
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel model, MainViewModel pageModel)
        {
            InitializeComponent();

            this.MainFrame.Navigate(new MainPage(pageModel));

            DataContext = model;
        }
    }
}