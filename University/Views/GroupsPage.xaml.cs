using System.Windows.Controls;
using University.Domain.ViewModels;

namespace University.Views
{
    /// <summary>
    /// Interaction logic for GroupsPage.xaml
    /// </summary>
    public partial class GroupsPage : Page
    {
        public GroupsPage(GroupsViewModel model)
        {
            InitializeComponent();

            DataContext = model;
        }

        private void NameChangedEvent(object sender, TextChangedEventArgs e)
        {
            if (DataContext is GroupsViewModel viewModel && sender is TextBox textBox)
            {
                if (viewModel.NameChangedCommand.CanExecute(textBox))
                {
                    viewModel.NameChangedCommand.Execute(textBox);
                }
            }
        }

        private void TeacherChangedEvent(object? sender, EventArgs eventArgs)
        {
            if (DataContext is GroupsViewModel viewModel && sender is ComboBox comboBox)
            {
                if (viewModel.TeacherChangedCommand.CanExecute(comboBox))
                {
                    viewModel.TeacherChangedCommand.Execute(comboBox);
                }
            }
        }

        private void CourseChangedEvent(object? sender, EventArgs eventArgs)
        {
            if (DataContext is GroupsViewModel viewModel && sender is ComboBox comboBox)
            {
                if (viewModel.CourseChangedCommand.CanExecute(comboBox))
                {
                    viewModel.CourseChangedCommand.Execute(comboBox);
                }
            }
        }
    }
}
