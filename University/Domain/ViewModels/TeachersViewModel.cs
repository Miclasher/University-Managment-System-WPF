﻿using System.Collections.ObjectModel;
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
    public class TeachersViewModel : BaseCrudViewModel, INotifyPropertyChanged
    {
        private readonly UniversityContext _context;
        private readonly IMessageBoxService _messageBoxService;
        private Teacher _selectedTeacher = null!;
        public Teacher SelectedTeacher
        {
            get => _selectedTeacher;
            set
            {
                if (!IsSaved && _selectedTeacher != null! && value != null!)
                {
                    var result = _messageBoxService.Show(
                        "You have unsaved changes. Fo you want to save them and proceed?",
                        "Warning",
                        MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            SaveTeacher();
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

                if (_selectedTeacher != value)
                {
                    _selectedTeacher = value!;
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

        public TeachersViewModel(UniversityContext context, IMessageBoxService messageBoxService)
        {
            _context = context;
            _messageBoxService = messageBoxService;

            Teachers = new ObservableCollection<Teacher>(_context.Teachers);

            AddTeacherCommand = new RelayCommand(_ => AddTeacher(), _ => true);
            DeleteTeacherCommand = new RelayCommand(_ => DeleteTeacher(), _ => true);
            SaveTeacherCommand = new RelayCommand(_ => SaveTeacher(), _ => true);
            FirstNameChangedCommand = new RelayCommand(FirstNameChanged, _ => true);
            LastNameChangedCommand = new RelayCommand(LastNameChanged, _ => true);
        }

        public override void SaveChanges()
        {
            SaveTeacher();
        }

        private void LastNameChanged(object? obj)
        {
            if (SelectedTeacher == null!)
            {
                return;
            }

            var control = obj as TextBox;

            if (control!.Text != SelectedTeacher.LastName)
            {
                IsSaved = false;
            }
        }

        private void FirstNameChanged(object? obj)
        {
            if (SelectedTeacher == null!)
            {
                return;
            }

            var control = obj as TextBox;

            if (control!.Text != SelectedTeacher.FirstName)
            {
                IsSaved = false;
            }
        }

        private void SaveTeacher()
        {
            ArgumentNullException.ThrowIfNull(SelectedTeacher);

            if (string.IsNullOrWhiteSpace(SelectedTeacher.FirstName))
            {
                throw new InvalidOperationException("Name cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(SelectedTeacher.LastName))
            {
                throw new InvalidOperationException("Surname cannot be empty.");
            }

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

            if (SelectedTeacher.Groups.Count != 0)
            {
                throw new InvalidOperationException("You cannot delete a teacher who is assigned to a group.\n" +
                                                    "Group(s) this teacher is assigned to: " +
                                                    $"{string.Join(',', SelectedTeacher.Groups.Select(e => e.Name))}");
            }

            var result = _messageBoxService.Show("Are you sure you want to delete the selected teacher?", "Delete Teacher", System.Windows.MessageBoxButton.YesNo);

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

            SelectedTeacher = newTeacher;

            if (SelectedTeacher != newTeacher)
            {
                return;
            }

            Teachers.Add(newTeacher);

            IsSaved = false;
        }
    }
}
