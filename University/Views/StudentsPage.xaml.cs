using System.Windows.Controls;
using University.Domain.ViewModels;

namespace University.Views
{
    /// <summary>
    /// Interaction logic for StudentsPage.xaml
    /// </summary>
    public partial class StudentsPage : Page
    {
        public StudentsPage(StudentsViewModel model)
        {
            InitializeComponent();

            DataContext = model;
        }
        private void FirstNameChangedEvent(object sender, TextChangedEventArgs e)
        {
            if (DataContext is StudentsViewModel viewModel && sender is TextBox textBox)
            {
                if (viewModel.FirstNameChangedCommand.CanExecute(textBox))
                {
                    viewModel.FirstNameChangedCommand.Execute(textBox);
                }
            }
        }

        private void LastNameChangedEvent(object sender, TextChangedEventArgs e)
        {
            if (DataContext is StudentsViewModel viewModel && sender is TextBox textBox)
            {
                if (viewModel.LastNameChangedCommand.CanExecute(textBox))
                {
                    viewModel.LastNameChangedCommand.Execute(textBox);
                }
            }
        }

        private void GroupChangedEvent(object? sender, EventArgs e)
        {
            if (DataContext is StudentsViewModel viewModel && sender is ComboBox comboBox)
            {
                if (viewModel.GroupChangedCommand.CanExecute(comboBox))
                {
                    viewModel.GroupChangedCommand.Execute(comboBox);
                }
            }
        }
    }
}
