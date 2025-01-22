using System.Windows;
using University.Domain.ViewModels;

namespace University
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }
    }
}