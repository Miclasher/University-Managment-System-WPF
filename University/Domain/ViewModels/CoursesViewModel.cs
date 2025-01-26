using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using University.DataLayer;
using University.DataLayer.Models;
using University.Domain.Commands;

namespace University.Domain.ViewModels
{
    public class CoursesViewModel : BaseCrudViewModel, INotifyPropertyChanged
    {
        private readonly UniversityContext _context;
        private Course _selectedCourse = null!;
        public Course SelectedCourse
        {
            get => _selectedCourse;
            set
            {
                if (_selectedCourse != value)
                {
                    if (_selectedCourse != null!)
                    {
                        _selectedCourse.PropertyChanged -= OnSelectedCourseChanged;
                    }
                    
                    _selectedCourse = value;

                    if (_selectedCourse != null!)
                    {
                        _selectedCourse.PropertyChanged += OnSelectedCourseChanged;
                    }

                    OnPropertyChanged(nameof(SelectedCourse));
                }
            }
        }
        public ObservableCollection<Course> Courses { get; }
        public ICommand AddCourseCommand { get; }
        public ICommand DeleteCourseCommand { get; }
        public ICommand SaveCourseCommand { get; }

        public CoursesViewModel(UniversityContext context)
        {
            _context = context;

            Courses = new ObservableCollection<Course>(_context.Courses.Include(e => e.Groups).ToList());

            AddCourseCommand = new RelayCommand(_ => AddCourse(), _ => true);
            DeleteCourseCommand = new RelayCommand(_ => DeleteCourse(), _ => true);
            SaveCourseCommand = new RelayCommand(_ => SaveCourse(), _ => true);
        }

        private void SaveCourse()
        {
            ArgumentNullException.ThrowIfNull(SelectedCourse);

            if (SelectedCourse.Id == Guid.Empty)
            {
                _context.Courses.Add(SelectedCourse);
                _context.SaveChanges();
            }
            else
            {
                _context.Courses.Update(SelectedCourse);
            }
        }

        private void DeleteCourse()
        {
            ArgumentNullException.ThrowIfNull(SelectedCourse);

            if (SelectedCourse.Groups.Count != 0)
            {
                throw new InvalidOperationException("You can't delete a course that has groups.");
            }

            var result = System.Windows.MessageBox.Show("Are you sure you want to delete the selected Course?", "Delete Course", System.Windows.MessageBoxButton.YesNo);

            if (result == System.Windows.MessageBoxResult.Yes)
            {
                if (SelectedCourse.Id == Guid.Empty)
                {
                    Courses.Remove(SelectedCourse);
                }
                else
                {
                    _context.Courses.Remove(SelectedCourse);
                    _context.SaveChanges();
                    Courses.Remove(SelectedCourse);
                }
            }
        }

        private void AddCourse()
        {
            var newCourse = new Course()
            {
                Name = "New course",
                Description = "Write description here",
            };
            Courses.Add(newCourse);
            SelectedCourse = newCourse;
        }

        private void OnSelectedCourseChanged(object? sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            IsSaved = false;
        }
    }
}
