using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using University.DataLayer;
using University.DataLayer.Models;
using University.Domain.Commands;

namespace University.Domain.ViewModels
{
    public class TeachersViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private readonly UniversityContext _context;
        private Teacher _selectedTeacher = null!;
        public Teacher SelectedTeacher
        {
            get => _selectedTeacher;
            set
            {
                if (_selectedTeacher != value)
                {
                    _selectedTeacher = value;
                    OnPropertyChanged(nameof(SelectedTeacher));
                }
            }
        }
        public ObservableCollection<Teacher> Teachers { get; }
        public ICommand AddTeacherCommand { get; }
        public ICommand DeleteTeacherCommand { get; }
        public ICommand SaveTeacherCommand { get; }
        public TeachersViewModel(UniversityContext context)
        {
            _context = context;

            Teachers = new ObservableCollection<Teacher>(_context.Teachers.ToList());

            AddTeacherCommand = new RelayCommand(_ => AddTeacher(), _ => true);
            DeleteTeacherCommand = new RelayCommand(_ => DeleteTeacher(), _ => true);
            SaveTeacherCommand = new RelayCommand(_ => SaveTeacher(), _ => true);
        }

        private void SaveTeacher()
        {
            throw new NotImplementedException();
        }

        private void DeleteTeacher()
        {
            throw new NotImplementedException();
        }

        private void AddTeacher()
        {
            throw new NotImplementedException();
        }
    }
}
