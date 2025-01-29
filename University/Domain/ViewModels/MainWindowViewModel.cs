using System.Windows;
using System.Windows.Input;
using University.DataLayer;
using University.Domain.Commands;
using University.Domain.Utilities;
using University.Views;

namespace University.Domain.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly UniversityContext _context;
        private readonly IMessageBoxService _messageBoxService;
        private static MainWindow MainWindow => (MainWindow)App.Current.MainWindow;

        private BaseViewModel _currentViewModel;
        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                if (_currentViewModel != value)
                {
                    _currentViewModel = value;
                    OnPropertyChanged(nameof(CurrentViewModel));
                }
            }
        }

        public ICommand NavigateToMainCommand { get; }
        public ICommand NavigateToGroupListCommand { get; }
        public ICommand NavigateToStudentListCommand { get; }
        public ICommand NavigateToTeacherListCommand { get; }
        public ICommand NavigateToCoursesListCommand { get; }

        public MainWindowViewModel(UniversityContext context, IMessageBoxService messageBoxService)
        {
            _context = context;
            _messageBoxService = messageBoxService;

            _currentViewModel = new MainViewModel(_context);

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
                var viewModel = new MainViewModel(_context);

                CurrentViewModel = viewModel;
                MainWindow!.MainFrame.Navigate(new MainPage(viewModel));
            }
        }

        private void NavigateToCoursesList()
        {
            if (CanNavigate())
            {
                var viewModel = new CoursesViewModel(_context, _messageBoxService);

                CurrentViewModel = viewModel;
                MainWindow!.MainFrame.Navigate(new CoursesPage(viewModel));
            }
        }

        private void NavigateToTeacherList()
        {
            if (CanNavigate())
            {
                var viewModel = new TeachersViewModel(_context, _messageBoxService);

                CurrentViewModel = viewModel;
                MainWindow!.MainFrame.Navigate(new TeachersPage(viewModel));
            }
        }

        private void NavigateToStudentList()
        {
            if (CanNavigate())
            {
                var viewModel = new StudentsViewModel(_context, _messageBoxService);

                CurrentViewModel = viewModel;
                MainWindow!.MainFrame.Navigate(new StudentsPage(viewModel));
            }
        }

        private void NavigateToGroupList()
        {
            if (CanNavigate())
            {
                var viewModel = new GroupsViewModel(_context, _messageBoxService);

                CurrentViewModel = viewModel;
                MainWindow!.MainFrame.Navigate(new GroupsPage(viewModel));
            }
        }

        private bool CanNavigate()
        {
            if (CurrentViewModel is BaseCrudViewModel viewModel)
            {
                if (!viewModel.IsSaved)
                {
                    var result =
                        MessageBox.Show("There are unsaved changes. Do you wish to save and proceed to other page?",
                            "Unsaved changes warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (result == MessageBoxResult.No)
                    {
                        return false;
                    }

                    viewModel.SaveChanges();
                }
            }

            return true;
        }
    }
}
