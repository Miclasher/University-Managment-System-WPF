using System.Windows.Input;
using University.DataLayer;
using University.Domain.Commands;
using University.Views;

namespace University.Domain.ViewModels
{
    public enum PageType
    {
        MainPage,
        CoursesList,
        GroupList,
        TeacherList,
        StudentList
    }

    public class MainWindowViewModel : BaseViewModel
    {
        private readonly UniversityContext _context;
        private static MainWindow MainWindow => (MainWindow)App.Current.MainWindow;

        private PageType _currentPage;
        public PageType CurrentPage
        {
            get => _currentPage;
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    OnPropertyChanged(nameof(CurrentPage));
                }
            }
        }

        public ICommand NavigateToMainCommand { get; }
        public ICommand NavigateToGroupListCommand { get; }
        public ICommand NavigateToStudentListCommand { get; }
        public ICommand NavigateToTeacherListCommand { get; }
        public ICommand NavigateToCoursesListCommand { get; }

        public MainWindowViewModel(UniversityContext context)
        {
            _context = context;
            NavigateToMainCommand = new RelayCommand(_ => NavigateToMainPage(), _ => true);
            NavigateToGroupListCommand = new RelayCommand(_ => NavigateToGroupList(), _ => true);
            NavigateToStudentListCommand = new RelayCommand(_ => NavigateToStudentList(), _ => true);
            NavigateToTeacherListCommand = new RelayCommand(_ => NavigateToTeacherList(), _ => true);
            NavigateToCoursesListCommand = new RelayCommand(_ => NavigateToCoursesList(), _ => true);
        }

        private void NavigateToMainPage()
        {
            CurrentPage = PageType.MainPage;

            MainWindow!.MainFrame.Navigate(new MainPage(new MainViewModel(_context)));
        }

        private void NavigateToCoursesList()
        {
            CurrentPage = PageType.CoursesList;

            MainWindow!.MainFrame.Navigate(new CoursesPage(new CoursesViewModel(_context)));
        }

        private void NavigateToTeacherList()
        {
            CurrentPage = PageType.TeacherList;

            MainWindow!.MainFrame.Navigate(new TeachersPage(new TeachersViewModel(_context)));
        }

        private void NavigateToStudentList()
        {
            CurrentPage = PageType.StudentList;

            MainWindow!.MainFrame.Navigate(new StudentsPage(new StudentsViewModel(_context)));
        }

        private void NavigateToGroupList()
        {
            CurrentPage = PageType.GroupList;

            MainWindow!.MainFrame.Navigate(new GroupsPage(new GroupsViewModel(_context)));
        }
    }
}
