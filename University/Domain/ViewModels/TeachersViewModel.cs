﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using University.DataLayer;
using University.DataLayer.Models;
using University.Domain.Commands;

namespace University.Domain.ViewModels
{
    public class TeachersViewModel : BaseCrudViewModel, INotifyPropertyChanged
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
        public ICommand FirstNameChangedCommand { get; }
        public ICommand LastNameChangedCommand { get; }

        public TeachersViewModel(UniversityContext context)
        {
            _context = context;

            Teachers = new ObservableCollection<Teacher>(_context.Teachers.ToList());

            AddTeacherCommand = new RelayCommand(_ => AddTeacher(), _ => true);
            DeleteTeacherCommand = new RelayCommand(_ => DeleteTeacher(), _ => true);
            SaveTeacherCommand = new RelayCommand(_ => SaveTeacher(), _ => true);
            FirstNameChangedCommand = new RelayCommand(FirstNameChanged, _ => true);
            LastNameChangedCommand = new RelayCommand(LastNameChanged, _ => true);
        }

        private void LastNameChanged(object? obj)
        {
            var control = obj as TextBox;
            if (control!.Text != SelectedTeacher.LastName)
            {
                IsSaved = false;
            }
        }

        private void FirstNameChanged(object? obj)
        {
            var control = obj as TextBox;
            if (control!.Text != SelectedTeacher.FirstName)
            {
                IsSaved = false;
            }
        }

        private void SaveTeacher()
        {
            ArgumentNullException.ThrowIfNull(SelectedTeacher);

            if (SelectedTeacher.Id == Guid.Empty)
            {
                _context.Teachers.Add(SelectedTeacher);
                _context.SaveChanges();
            }
            else
            {
                _context.Teachers.Update(SelectedTeacher);
            }

            IsSaved = true;
        }

        private void DeleteTeacher()
        {
            ArgumentNullException.ThrowIfNull(SelectedTeacher);

            var result = System.Windows.MessageBox.Show("Are you sure you want to delete the selected teacher?", "Delete Teacher", System.Windows.MessageBoxButton.YesNo);

            if (result == System.Windows.MessageBoxResult.Yes)
            {
                if (SelectedTeacher.Id == Guid.Empty)
                {
                    Teachers.Remove(SelectedTeacher);
                }
                else
                {
                    _context.Teachers.Remove(SelectedTeacher);
                    _context.SaveChanges();
                    Teachers.Remove(SelectedTeacher);
                }
            }

            IsSaved = true;
        }

        private void AddTeacher()
        {
            var newTeacher = new Teacher()
            {
                FirstName = "New",
                LastName = "Teacher",
            };
            Teachers.Add(newTeacher);
            SelectedTeacher = newTeacher;

            IsSaved = false;
        }
    }
}
