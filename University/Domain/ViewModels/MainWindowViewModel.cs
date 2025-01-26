using System.Windows;
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
            if (CanNavigate())
            {
                CurrentPage = PageType.MainPage;

                MainWindow!.MainFrame.Navigate(new MainPage(new MainViewModel(_context)));
            }
        }

        private void NavigateToCoursesList()
        {
            if (CanNavigate())
            {
                CurrentPage = PageType.CoursesList;

                MainWindow!.MainFrame.Navigate(new CoursesPage(new CoursesViewModel(_context)));
            }
        }

        private void NavigateToTeacherList()
        {
            if (CanNavigate())
            {
                CurrentPage = PageType.TeacherList;

                MainWindow!.MainFrame.Navigate(new TeachersPage(new TeachersViewModel(_context)));
            }
        }

        private void NavigateToStudentList()
        {
            if (CanNavigate())
            {
                CurrentPage = PageType.StudentList;

                MainWindow!.MainFrame.Navigate(new StudentsPage(new StudentsViewModel(_context)));
            }
        }

        private void NavigateToGroupList()
        {
            if (CanNavigate())
            {
                CurrentPage = PageType.GroupList;

                MainWindow!.MainFrame.Navigate(new GroupsPage(new GroupsViewModel(_context)));
            }
        }

        private bool CanNavigate()
        {
            var content = MainWindow.MainFrame.Content;
            if (content != null)
            {
                var dataContextProperty = content.GetType().GetProperty("DataContext");
                if (dataContextProperty != null)
                {
                    var dataContext = dataContextProperty.GetValue(content);
                    if (dataContext is BaseCrudViewModel viewModel)
                    {
                        if (!viewModel.IsSaved)
                        {
                            var result = MessageBox.Show("There are unsaved changes. Do you wish to navigate to another page without saving?", "Unsaved changes warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                            if (result == MessageBoxResult.No)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}
