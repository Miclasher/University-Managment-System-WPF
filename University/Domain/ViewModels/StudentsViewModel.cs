using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using University.DataLayer;
using University.DataLayer.Models;
using University.Domain.Commands;
using University.Domain.Utilities;

namespace University.Domain.ViewModels
{
    public class StudentsViewModel : BaseCrudViewModel, INotifyPropertyChanged
    {
        private readonly UniversityContext _context;
        private readonly IMessageBoxService _messageBoxService;
        private Student _selectedStudent = null!;
        public Student SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                if (_selectedStudent != value)
                {
                    _selectedStudent = value;
                    OnPropertyChanged(nameof(SelectedStudent));
                }
            }
        }
        public ObservableCollection<Student> Students { get; }
        public ObservableCollection<Group> Groups { get; }
        public ICommand AddStudentCommand { get; }
        public ICommand DeleteStudentCommand { get; }
        public ICommand SaveStudentCommand { get; }
        public ICommand FirstNameChangedCommand { get; }
        public ICommand LastNameChangedCommand { get; }
        public ICommand GroupChangedCommand { get; }

        public StudentsViewModel(UniversityContext context, IMessageBoxService messageBoxService)
        {
            _context = context;
            _messageBoxService = messageBoxService;

            Students = new ObservableCollection<Student>(_context.Students.Include(e => e.Group).ToList());
            Groups = new ObservableCollection<Group>(_context.Groups.ToList());

            AddStudentCommand = new RelayCommand(_ => AddStudent(), _ => true);
            DeleteStudentCommand = new RelayCommand(_ => ConfirmAndDeleteStudent(), _ => true);
            SaveStudentCommand = new RelayCommand(_ => SaveStudent(), _ => true);
            FirstNameChangedCommand = new RelayCommand(FirstNameChanged, _ => true);
            LastNameChangedCommand = new RelayCommand(LastNameChanged, _ => true);
            GroupChangedCommand = new RelayCommand(_ => GroupChanged(), _ => true);
        }

        private void GroupChanged()
        {
            IsSaved = false;
        }

        private void LastNameChanged(object? obj)
        {
            var control = obj as TextBox;
            if (control!.Text != SelectedStudent.LastName)
            {
                IsSaved = false;
            }
        }

        private void FirstNameChanged(object? obj)
        {
            var control = obj as TextBox;
            if (control!.Text != SelectedStudent.FirstName)
            {
                IsSaved = false;
            }
        }

        private void SaveStudent()
        {
            ArgumentNullException.ThrowIfNull(SelectedStudent);

            if (SelectedStudent.Id == Guid.Empty)
            {
                _context.Students.Add(SelectedStudent);
                _context.SaveChanges();
            }
            else
            {
                _context.Students.Update(SelectedStudent);
            }

            IsSaved = true;
        }

        private void ConfirmAndDeleteStudent()
        {
            ArgumentNullException.ThrowIfNull(SelectedStudent);

            var result = _messageBoxService.Show("Are you sure you want to delete the selected student?", "Delete Student", System.Windows.MessageBoxButton.YesNo);

            if (result == System.Windows.MessageBoxResult.Yes)
            {
                if (SelectedStudent.Id == Guid.Empty)
                {
                    Students.Remove(SelectedStudent);
                }
                else
                {
                    _context.Students.Remove(SelectedStudent);
                    _context.SaveChanges();
                    Students.Remove(SelectedStudent);
                }
            }

            IsSaved = true;
        }

        private void AddStudent()
        {
            if (!Groups.Any())
            {
                throw new InvalidOperationException("No groups available. Please add a group first.");
            }

            var newStudent = new Student()
            {
                FirstName = "New",
                LastName = "Student",
                Group = Groups.FirstOrDefault()!
            };
            Students.Add(newStudent);
            SelectedStudent = newStudent;

            IsSaved = false;
        }
    }
}
