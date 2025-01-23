using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.DataLayer;
using University.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;
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
            throw new NotImplementedException();
        }

        private void DeleteStudent()
        {
            throw new NotImplementedException();
        }

        private void AddStudent()
        {
            throw new NotImplementedException();
        }
    }
}
