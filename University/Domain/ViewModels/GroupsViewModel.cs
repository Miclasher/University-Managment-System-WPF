using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using University.DataLayer;
using University.DataLayer.Models;
using University.Domain.Commands;
using University.Domain.Utilities;

namespace University.Domain.ViewModels
{
    public class GroupsViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private readonly UniversityContext _context;
        private Group _selectedGroup = null!;
        public ICommand AddGroupCommand { get; }
        public ICommand ClearGroupCommand { get; }
        public ICommand DeleteGroupCommand { get; }
        public ICommand SaveGroupCommand { get; }
        public ICommand ImportStudentsCommand { get; }
        public ICommand ExportStudentsCommand { get; }
        public ICommand ExportGroupToPdfCommand { get; }
        public Group SelectedGroup
        {
            get => _selectedGroup;
            set
            {
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
        public GroupsViewModel(UniversityContext context)
        {
            _context = context;

            Groups = new ObservableCollection<Group>(_context.Groups.Include(e => e.Teacher).OrderBy(g => g.Name).ToList());
            Teachers = new ObservableCollection<Teacher>(_context.Teachers.OrderBy(e => e.FirstName).ToList());
            Courses = new ObservableCollection<Course>(_context.Courses.OrderBy(e => e.Name).ToList());

            AddGroupCommand = new RelayCommand(_ => AddGroup(), _ => true);
            SaveGroupCommand = new RelayCommand(_ => SaveGroup(), _ => true);
            ClearGroupCommand = new RelayCommand(_ => ClearGroup(), _ => true);
            DeleteGroupCommand = new RelayCommand(_ => DeleteGroup(), _ => true);
            ImportStudentsCommand = new RelayCommand(_ => ImportStudents(), _ => true);
            ExportStudentsCommand = new RelayCommand(_ => ExportStudents(), _ => true);
            ExportGroupToPdfCommand = new RelayCommand(_ => ExportGroupToPdf(), _ => true);
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

            var result = System.Windows.MessageBox.Show("Are you sure you want to delete the selected group?", "Delete Group", System.Windows.MessageBoxButton.YesNo);
            if (result == System.Windows.MessageBoxResult.Yes)
            {
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
                    System.Windows.MessageBox.Show("Group has students. Please clear the group first.", "Delete Group", System.Windows.MessageBoxButton.OK);
                }
            }
        }

        private void SaveGroup()
        {
            ArgumentNullException.ThrowIfNull(SelectedGroup);

            if (SelectedGroup.Id == Guid.Empty)
            {
                _context.Groups.Add(SelectedGroup);
            }
            else
            {
                _context.Groups.Update(SelectedGroup);
            }
            _context.SaveChanges();
        }

        private void ClearGroup()
        {
            ArgumentNullException.ThrowIfNull(SelectedGroup);

            var result = System.Windows.MessageBox.Show("Are you sure you want to clear the selected group? (All students will be deleted)", "Clear Group", System.Windows.MessageBoxButton.YesNo);
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
        }
    }
}
