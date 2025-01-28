using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using University.DataLayer;
using University.DataLayer.Models;
using University.Domain.Commands;
using University.Domain.Utilities;

namespace University.Domain.ViewModels
{
    public class CoursesViewModel : BaseCrudViewModel, INotifyPropertyChanged
    {
        private readonly UniversityContext _context;
        private readonly IMessageBoxService _messageBoxService;
        private Course _selectedCourse = null!;
        public Course SelectedCourse
        {
            get => _selectedCourse;
            set
            {
                if (!IsSaved && _selectedCourse != null!)
                {
                    var result = _messageBoxService.Show(
                        "You have unsaved changes. Fo you want to save them and proceed?",
                        "Warning",
                        MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            SaveCourse();
                        }
                        catch (InvalidOperationException e)
                        {
                            _messageBoxService.Show(e.Message, "Error", MessageBoxButton.OK);
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }

                if (_selectedCourse != value)
                {
                    _selectedCourse = value;
                    OnPropertyChanged(nameof(SelectedCourse));
                }
            }
        }
        public ObservableCollection<Course> Courses { get; }
        public ICommand AddCourseCommand { get; }
        public ICommand DeleteCourseCommand { get; }
        public ICommand SaveCourseCommand { get; }
        public ICommand NameChangedCommand { get; }
        public ICommand DescriptionChangedCommand { get; }

        public CoursesViewModel(UniversityContext context, IMessageBoxService messageBoxService)
        {
            _context = context;
            _messageBoxService = messageBoxService;

            Courses = new ObservableCollection<Course>(_context.Courses.Include(e => e.Groups).ToList());

            AddCourseCommand = new RelayCommand(_ => AddCourse(), _ => true);
            DeleteCourseCommand = new RelayCommand(_ => DeleteCourse(), _ => true);
            SaveCourseCommand = new RelayCommand(_ => SaveCourse(), _ => true);
            NameChangedCommand = new RelayCommand(NameChanged, _ => true);
            DescriptionChangedCommand = new RelayCommand(DescriptionChanged, _ => true);
        }

        private void NameChanged(object? textBox)
        {
            if (SelectedCourse == null!)
            {
                return;
            }

            var control = textBox as TextBox;

            if (control!.Text != SelectedCourse.Name)
            {
                IsSaved = false;
            }
        }

        private void DescriptionChanged(object? textBox)
        {
            if (SelectedCourse == null!)
            {
                return;
            }

            var control = textBox as TextBox;

            if (control!.Text != SelectedCourse.Description)
            {
                IsSaved = false;
            }
        }

        private void SaveCourse()
        {
            ArgumentNullException.ThrowIfNull(SelectedCourse);

            if (string.IsNullOrWhiteSpace(SelectedCourse.Name))
            {
                throw new InvalidOperationException("Course name can't be empty.");
            }

            if (SelectedCourse.Id == Guid.Empty)
            {
                _context.Courses.Add(SelectedCourse);
                _context.SaveChanges();
            }
            else
            {
                _context.Courses.Update(SelectedCourse);
            }

            IsSaved = true;
        }

        private void DeleteCourse()
        {
            ArgumentNullException.ThrowIfNull(SelectedCourse);

            if (SelectedCourse.Groups.Count != 0)
            {
                throw new InvalidOperationException("You can't delete a course that has groups.");
            }

            var result = _messageBoxService.Show("Are you sure you want to delete the selected Course?", "Delete Course", System.Windows.MessageBoxButton.YesNo);

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

            IsSaved = true;
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
            IsSaved = false;
        }
    }
}
