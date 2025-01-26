using System.Windows.Input;
using University.DataLayer;
using University.Domain.Commands;
using University.Views;

namespace University.Domain.ViewModels
{
    public class MainWindowViewModel
    {
        private readonly UniversityContext _context;
        private static MainWindow MainWindow => (MainWindow)App.Current.MainWindow;

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
            MainWindow!.MainFrame.Navigate(new MainPage(new MainViewModel(_context)));
        }

        private void NavigateToTeacherList()
        {
            MainWindow!.MainFrame.Navigate(new TeachersPage(new TeachersViewModel(_context)));
        }

        private void NavigateToStudentList()
        {
            MainWindow!.MainFrame.Navigate(new StudentsPage(new StudentsViewModel(_context)));
        }

        private void NavigateToGroupList()
        {
            MainWindow!.MainFrame.Navigate(new GroupsPage(new GroupsViewModel(_context)));
        }
    }
}
