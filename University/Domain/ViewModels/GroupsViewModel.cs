using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using University.DataLayer;
using University.DataLayer.Models;
using University.Domain.Commands;

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
        public ICommand ImportToGroupCommand { get; }
        public ICommand ExportGroupCommand { get; }
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
            ImportToGroupCommand = new RelayCommand(_ => ImportToGroup(), _ => true);
            ExportGroupCommand = new RelayCommand(_ => ExportGroup(), _ => true);
        }

        private void ImportToGroup()
        {
            throw new NotImplementedException();
        }

        private void ExportGroup()
        {
            throw new NotImplementedException();
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
            var NewGroup = new Group()
            {
                Name = "New Group",
                Teacher = Teachers.First(),
                Course = Courses.First()
            };
            Groups.Add(NewGroup);
            SelectedGroup = NewGroup;
        }
    }
}
