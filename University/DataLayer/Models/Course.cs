using System.ComponentModel.DataAnnotations.Schema;

namespace University.DataLayer.Models
{
    internal class Course
    {
        [Column("COURSE_ID")]
        public Guid Id { get; set; }
        [Column("NAME")]
        public string Name { get; set; } = string.Empty;
        [Column("DESCRIPTION")]
        public string Description { get; set; } = string.Empty;
        public List<Group> Groups { get; set; } = new List<Group>();
    }
}
