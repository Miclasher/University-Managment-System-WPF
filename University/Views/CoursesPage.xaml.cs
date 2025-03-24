using System.Windows.Controls;
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
