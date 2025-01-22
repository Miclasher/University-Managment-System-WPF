using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using University.DataLayer.Models;
using System.Threading.Tasks;
using University.DataLayer;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;
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
        public GroupsViewModel(UniversityContext context)
        {
            _context = context;

            Groups = new ObservableCollection<Group>(_context.Groups.Include(e => e.Teacher).OrderBy(g => g.Name).ToList());
            Teachers = new ObservableCollection<Teacher>(_context.Teachers.OrderBy(e => e.FirstName).ToList());

            AddGroupCommand = new RelayCommand(_ => AddGroup(), _ => true);
            SaveGroupCommand = new RelayCommand(g => SaveGroup(g), _ => true);
            ClearGroupCommand = new RelayCommand(g => ClearGroup(g), _ => true);
            DeleteGroupCommand = new RelayCommand(g => DeleteGroup(g), _ => true);
            ImportToGroupCommand = new RelayCommand(g => ImportToGroup(g), _ => true);
            ExportGroupCommand = new RelayCommand(g => ExportGroup(g), _ => true);
        }

        private void ImportToGroup(object? g)
        {
            throw new NotImplementedException();
        }

        private void ExportGroup(object? g)
        {
            throw new NotImplementedException();
        }

        private void DeleteGroup(object? g)
        {
            throw new NotImplementedException();
        }

        private void SaveGroup(object? g)
        {
            throw new NotImplementedException();
        }

        private void ClearGroup(object? g)
        {
            throw new NotImplementedException();
        }

        private void AddGroup()
        {
            throw new NotImplementedException();
        }
    }
}
