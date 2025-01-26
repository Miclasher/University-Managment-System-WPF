using System.Windows.Controls;
using University.Domain.ViewModels;

namespace University.Views
{
    /// <summary>
    /// Interaction logic for TeachersPage.xaml
    /// </summary>
    public partial class TeachersPage : Page
    {
        public TeachersPage(TeachersViewModel model)
        {
            InitializeComponent();

            DataContext = model;
        }

        private void FirstNameChangedEvent(object sender, TextChangedEventArgs e)
        {
            if (DataContext is TeachersViewModel viewModel && sender is TextBox textBox)
            {
                if (viewModel.FirstNameChangedCommand.CanExecute(textBox))
                {
                    viewModel.FirstNameChangedCommand.Execute(textBox);
                }
            }
        }

        private void LastNameChangedEvent(object sender, TextChangedEventArgs e)
        {
            if (DataContext is TeachersViewModel viewModel && sender is TextBox textBox)
            {
                if (viewModel.LastNameChangedCommand.CanExecute(textBox))
                {
                    viewModel.LastNameChangedCommand.Execute(textBox);
                }
            }
        }
    }
}
