using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.DataLayer.Models
{
    public class Course : EntityWithReactiveProperties
    {
        private string _name = string.Empty;
        private string _description = string.Empty;
        private ObservableCollection<Group> _groups = new ObservableCollection<Group>();

        [Column("COURSE_ID")]
        public Guid Id { get; set; }
        [Column("NAME")]
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        [Column("DESCRIPTION")]
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }
        public ObservableCollection<Group> Groups
        {
            get => _groups;
            set
            {
                if (_groups != value)
                {
                    _groups = value;
                    OnPropertyChanged(nameof(Groups));
                }
            }
        }
    }
}
