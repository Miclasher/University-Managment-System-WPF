using System.Windows.Controls;
using University.Domain.ViewModels;

namespace University.Views
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage(MainViewModel model)
        {
            InitializeComponent();

            DataContext = model;
        }
    }
}
