using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.DataLayer.Models
{
    public class Teacher
    {
        [Column("TEACHER_ID")]
        public Guid Id { get; set; }
        [Column("FIRST_NAME")]
        public string FirstName { get; set; } = string.Empty;
        [Column("LAST_NAME")]
        public string LastName { get; set; } = string.Empty;
        public ObservableCollection<Group> Groups { get; set; } = new ObservableCollection<Group>();
    }
}
