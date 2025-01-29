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
                if (!IsSaved && _selectedStudent != null!)
                {
                    var result = _messageBoxService.Show(
                        "You have unsaved changes. Fo you want to save them and proceed?",
                        "Warning",
                        MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            SaveStudent();
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

            Students = new ObservableCollection<Student>(_context.Students.Include(e => e.Group));
            Groups = new ObservableCollection<Group>(_context.Groups);

            AddStudentCommand = new RelayCommand(_ => AddStudent(), _ => true);
            DeleteStudentCommand = new RelayCommand(_ => ConfirmAndDeleteStudent(), _ => true);
            SaveStudentCommand = new RelayCommand(_ => SaveStudent(), _ => true);
            FirstNameChangedCommand = new RelayCommand(FirstNameChanged, _ => true);
            LastNameChangedCommand = new RelayCommand(LastNameChanged, _ => true);
            GroupChangedCommand = new RelayCommand(_ => GroupChanged(), _ => true);
        }

        public override void SaveChanges()
        {
            SaveStudent();
        }

        private void GroupChanged()
        {
            IsSaved = false;
        }

        private void LastNameChanged(object? obj)
        {
            if (SelectedStudent == null!)
            {
                return;
            }

            var control = obj as TextBox;

            if (control!.Text != SelectedStudent.LastName)
            {
                IsSaved = false;
            }
        }

        private void FirstNameChanged(object? obj)
        {
            if (SelectedStudent == null!)
            {
                return;
            }

            var control = obj as TextBox;

            if (control!.Text != SelectedStudent.FirstName)
            {
                IsSaved = false;
            }
        }

        private void SaveStudent()
        {
            ArgumentNullException.ThrowIfNull(SelectedStudent);

            if (string.IsNullOrWhiteSpace(SelectedStudent.FirstName))
            {
                throw new InvalidOperationException("Name cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(SelectedStudent.LastName))
            {
                throw new InvalidOperationException("Surname cannot be empty.");
            }

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
