using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using University.Domain.ViewModels;

namespace University.Views
{
    /// <summary>
    /// Interaction logic for CoursesPage.xaml
    /// </summary>
    public partial class CoursesPage : Page
    {
        public CoursesPage(CoursesViewModel model)
        {
            InitializeComponent();

            DataContext = model;
        }

        private void NameChangedEvent(object sender, TextChangedEventArgs e)
        {
            if (DataContext is CoursesViewModel viewModel && sender is TextBox textBox)
            {
                if (viewModel.NameChangedCommand.CanExecute(textBox))
                {
                    viewModel.NameChangedCommand.Execute(textBox);
                }
            }
        }

        private void DescriptionChangedEvent(object sender, TextChangedEventArgs e)
        {
            if (DataContext is CoursesViewModel viewModel && sender is TextBox textBox)
            {
                if (viewModel.DescriptionChangedCommand.CanExecute(textBox))
                {
                    viewModel.DescriptionChangedCommand.Execute(textBox);
                }
            }
        }
    }
}
