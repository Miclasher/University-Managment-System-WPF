using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using University.DataLayer;
using University.DataLayer.Models;
using University.Domain.Commands;

namespace University.Domain.ViewModels
{
    public class StudentsViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private readonly UniversityContext _context;
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

        public StudentsViewModel(UniversityContext context)
        {
            _context = context;

            Students = new ObservableCollection<Student>(_context.Students.Include(e => e.Group).ToList());
            Groups = new ObservableCollection<Group>(_context.Groups.ToList());

            AddStudentCommand = new RelayCommand(_ => AddStudent(), _ => true);
            DeleteStudentCommand = new RelayCommand(_ => DeleteStudent(), _ => true);
            SaveStudentCommand = new RelayCommand(_ => SaveStudent(), _ => true);
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
        }

        private void DeleteStudent()
        {
            ArgumentNullException.ThrowIfNull(SelectedStudent);

            var result = System.Windows.MessageBox.Show("Are you sure you want to delete the selected student?", "Delete Student", System.Windows.MessageBoxButton.YesNo);

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
        }

        private void AddStudent()
        {
            var NewStudent = new Student()
            {
                FirstName = "New",
                LastName = "Student",
                Group = Groups.FirstOrDefault()!
            };
            Students.Add(NewStudent);
            SelectedStudent = NewStudent;
        }
    }
}
