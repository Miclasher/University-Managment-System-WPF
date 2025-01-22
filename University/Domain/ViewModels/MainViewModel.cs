using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using University.DataLayer;
using University.DataLayer.Models;
using University.Domain.Commands;

namespace University.Domain.ViewModels
{
    public class MainViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private readonly UniversityContext _context;
        private Course _selectedCourse = null!;
        private Group _selectedGroup = null!;
        public ObservableCollection<Course> Courses { get; }
        public ObservableCollection<Group> Groups => SelectedCourse?.Groups!;
        public ObservableCollection<Student> Students => SelectedGroup?.Students!;
        public Course SelectedCourse
        {
            get => _selectedCourse;
            set
            {
                if (_selectedCourse != value)
                {
                    _selectedCourse = value;
                    OnPropertyChanged(nameof(SelectedCourse));
                    OnPropertyChanged(nameof(Groups));
                }
            }
        }

        public Group SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                if (_selectedGroup != value)
                {
                    _selectedGroup = value;
                    OnPropertyChanged(nameof(SelectedGroup));
                    OnPropertyChanged(nameof(Students));
                }
            }
        }
        public ICommand NavigateToGroupListCommand { get; }
        public ICommand NavigateToStudentListCommand { get; }
        public ICommand NavigateToTeacherListCommand { get; }

        public MainViewModel(UniversityContext context)
        {
            _context = context;

            Courses = new ObservableCollection<Course>(_context.Courses
                .Include(c => c.Groups)
                .ThenInclude(g => g.Students)
                .ToList());

            NavigateToGroupListCommand = new RelayCommand(_ => NavigateToGroupList(), _ => true);
            NavigateToStudentListCommand = new RelayCommand(_ => NavigateToStudentList(), _ => true);
            NavigateToTeacherListCommand = new RelayCommand(_ => NavigateToTeacherList(), _ => true);
        }

        private void NavigateToTeacherList()
        {
            throw new NotImplementedException();
        }

        private void NavigateToStudentList()
        {
            throw new NotImplementedException();
        }

        private void NavigateToGroupList()
        {
            throw new NotImplementedException();
        }
    }
}
