using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using University.DataLayer;
using University.DataLayer.Models;
using University.Domain.Commands;
using University.Domain.Utilities;

namespace University.Domain.ViewModels
{
    public class GroupsViewModel : BaseCrudViewModel, INotifyPropertyChanged
    {
        private readonly UniversityContext _context;
        private readonly IMessageBoxService _messageBoxService;
        private Group _selectedGroup = null!;
        public ICommand AddGroupCommand { get; }
        public ICommand ClearGroupCommand { get; }
        public ICommand DeleteGroupCommand { get; }
        public ICommand SaveGroupCommand { get; }
        public ICommand ImportStudentsCommand { get; }
        public ICommand ExportStudentsCommand { get; }
        public ICommand ExportGroupToPdfCommand { get; }
        public ICommand NameChangedCommand { get; }
        public ICommand TeacherChangedCommand { get; }
        public ICommand CourseChangedCommand { get; }
        public Group SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                if (!IsSaved && _selectedGroup != null! && value != null!)
                {
                    var result = _messageBoxService.Show(
                        "You have unsaved changes. Do you want to save and proceed?",
                        "Warning",
                        MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            SaveGroup();
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

                if (value != _selectedGroup)
                {
                    _selectedGroup = value;
                    OnPropertyChanged(nameof(SelectedGroup));
                }
            }
        }
        public ObservableCollection<Group> Groups { get; }
        public ObservableCollection<Teacher> Teachers { get; }
        public ObservableCollection<Course> Courses { get; }

        public GroupsViewModel(UniversityContext context, IMessageBoxService messageBoxService)
        {
            _context = context;
            _messageBoxService = messageBoxService;

            Groups = new ObservableCollection<Group>(_context.Groups.Include(e => e.Teacher).OrderBy(g => g.Name));
            Teachers = new ObservableCollection<Teacher>(_context.Teachers.OrderBy(e => e.FirstName));
            Courses = new ObservableCollection<Course>(_context.Courses.OrderBy(e => e.Name));

            AddGroupCommand = new RelayCommand(_ => AddGroup(), _ => true);
            SaveGroupCommand = new RelayCommand(_ => SaveGroup(), _ => true);
            ClearGroupCommand = new RelayCommand(_ => ClearGroup(), _ => true);
            DeleteGroupCommand = new RelayCommand(_ => DeleteGroup(), _ => true);
            ImportStudentsCommand = new RelayCommand(_ => ImportStudents(), _ => true);
            ExportStudentsCommand = new RelayCommand(_ => ExportStudents(), _ => true);
            ExportGroupToPdfCommand = new RelayCommand(_ => ExportGroupToPdf(), _ => true);
            NameChangedCommand = new RelayCommand(NameChanged, _ => true);
            TeacherChangedCommand = new RelayCommand(_ => TeacherChanged(), _ => true);
            CourseChangedCommand = new RelayCommand(_ => CourseChanged(), _ => true);
        }

        public override void SaveChanges()
        {
            SaveGroup();
        }

        private void CourseChanged()
        {
            IsSaved = false;
        }

        private void TeacherChanged()
        {
            IsSaved = false;
        }

        private void NameChanged(object? obj)
        {
            if (SelectedGroup == null!)
            {
                return;
            }

            var control = obj as TextBox;

            if (control!.Text != SelectedGroup.Name)
            {
                IsSaved = false;
            }
        }

        private void ExportGroupToPdf()
        {
            ArgumentNullException.ThrowIfNull(SelectedGroup);

            if (SelectedGroup.Students.Count == 0)
            {
                throw new InvalidOperationException("Group has no students to export.");
            }

            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = SelectedGroup.Name;
            dialog.DefaultExt = ".pdf";
            dialog.Filter = "Document (.pdf)|*.pdf";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                var filename = dialog.FileName;
                using var fs = new FileStream(filename, FileMode.OpenOrCreate);

                PdfExporter.ExportPdfGroup(fs, SelectedGroup);
            }
        }

        private void ImportStudents()
        {
            ArgumentNullException.ThrowIfNull(SelectedGroup);

            if (SelectedGroup.Students.Count > 0)
            {
                throw new InvalidOperationException("Group has students. Clear group to import students.");
            }

            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = SelectedGroup.Name;
            dialog.DefaultExt = ".csv";
            dialog.Filter = "Table (.csv)|*.csv";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                var filename = dialog.FileName;
                using var fs = new FileStream(filename, FileMode.Open);
                var students = CsvHandler.ImportStudents(fs);

                foreach (var student in students)
                {
                    SelectedGroup.Students.Add(student);
                }

                SaveGroup();
            }

            IsSaved = true;
        }

        private void ExportStudents()
        {
            ArgumentNullException.ThrowIfNull(SelectedGroup);

            if (SelectedGroup.Students.Count == 0)
            {
                throw new InvalidOperationException("Group has no students to export.");
            }

            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = SelectedGroup.Name;
            dialog.DefaultExt = ".csv";
            dialog.Filter = "Table (.csv)|*.csv";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                var filename = dialog.FileName;
                using var fs = new FileStream(filename, FileMode.OpenOrCreate);

                CsvHandler.ExportStudents(fs, SelectedGroup);
            }
        }

        private void DeleteGroup()
        {
            ArgumentNullException.ThrowIfNull(SelectedGroup);

            var result = _messageBoxService.Show("Are you sure you want to delete the selected group?", "Delete Group", System.Windows.MessageBoxButton.YesNo);
            if (result == System.Windows.MessageBoxResult.Yes)
            {
                IsSaved = true;

                if (SelectedGroup.Id == Guid.Empty)
                {
                    Groups.Remove(SelectedGroup);
                    return;
                }

                if (SelectedGroup.Students.Count == 0)
                {
                    _context.Groups.Remove(SelectedGroup);
                    _context.SaveChanges();

                    Groups.Remove(SelectedGroup);
                }
                else
                {
                    _messageBoxService.Show("Group has students. Please clear the group first.", "Delete Group", System.Windows.MessageBoxButton.OK);
                }
            }
        }

        private void SaveGroup()
        {
            ArgumentNullException.ThrowIfNull(SelectedGroup);

            if (string.IsNullOrWhiteSpace(SelectedGroup.Name))
            {
                throw new InvalidOperationException("Group name cannot be empty.");
            }

            if (SelectedGroup.Id == Guid.Empty)
            {
                _context.Groups.Add(SelectedGroup);
            }
            else
            {
                _context.Groups.Update(SelectedGroup);
            }
            _context.SaveChanges();

            IsSaved = true;
        }

        private void ClearGroup()
        {
            ArgumentNullException.ThrowIfNull(SelectedGroup);

            var result = _messageBoxService.Show("Are you sure you want to clear the selected group? (All students will be deleted)", "Clear Group", System.Windows.MessageBoxButton.YesNo);
            if (result == System.Windows.MessageBoxResult.Yes)
            {
                _context.RemoveRange(SelectedGroup.Students);
                _context.SaveChanges();

                SelectedGroup.Students.Clear();
            }
        }

        private void AddGroup()
        {
            if (!Courses.Any())
            {
                throw new InvalidOperationException("No courses available. Please add a course first.");
            }

            if (!Teachers.Any())
            {
                throw new InvalidOperationException("No tutors available. Please add a teacher first.");
            }

            var newGroup = new Group()
            {
                Name = "New Group",
                Teacher = Teachers.First(),
                Course = Courses.First()
            };
            Groups.Add(newGroup);
            SelectedGroup = newGroup;

            IsSaved = false;
        }
    }
}
