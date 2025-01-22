using University.Domain.Commands;
using University.Views;
using System.Windows.Input;
using University.DataLayer;

namespace University.Domain.ViewModels
{
    public class MainWindowViewModel
    {
        private readonly UniversityContext _context;

        public ICommand NavigateToMainCommand { get; }
        public ICommand NavigateToGroupListCommand { get; }
        public ICommand NavigateToStudentListCommand { get; }
        public ICommand NavigateToTeacherListCommand { get; }

        public MainWindowViewModel(UniversityContext context)
        {
            _context = context;
            NavigateToMainCommand = new RelayCommand(_ => NavigateToMainPage(), _ => true);
            NavigateToGroupListCommand = new RelayCommand(_ => NavigateToGroupList(), _ => true);
            NavigateToStudentListCommand = new RelayCommand(_ => NavigateToStudentList(), _ => true);
            NavigateToTeacherListCommand = new RelayCommand(_ => NavigateToTeacherList(), _ => true);
        }

        private void NavigateToMainPage()
        {
            var mainWindow = App.Current.MainWindow as MainWindow;
            mainWindow!.MainFrame.Navigate(new MainPage(new MainViewModel(_context)));
        }

        private void NavigateToTeacherList()
        {
            var mainWindow = App.Current.MainWindow as MainWindow;
            mainWindow!.MainFrame.Navigate(new TeachersPage(new TeachersViewModel(_context)));
        }

        private void NavigateToStudentList()
        {
            var mainWindow = App.Current.MainWindow as MainWindow;
            mainWindow!.MainFrame.Navigate(new StudentsPage(new StudentsViewModel(_context)));
        }

        private void NavigateToGroupList()
        {
            var mainWindow = App.Current.MainWindow as MainWindow;
            mainWindow!.MainFrame.Navigate(new GroupsPage(new GroupsViewModel(_context)));
        }

    }
}
