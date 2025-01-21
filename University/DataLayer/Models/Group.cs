using System.ComponentModel.DataAnnotations.Schema;

namespace University.DataLayer.Models
{
    internal class Group
    {
        [Column("GROUP_ID")]
        public Guid Id { get; set; }
        [Column("NAME")]
        public string Name { get; set; } = string.Empty;
        [Column("COURSE_ID")]
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;
        [Column("TEACHER_ID")]
        public Guid TeacherId { get; set; }
        public Teacher Teacher { get; set; } = null!;
        public List<Student> Students { get; set; } = new List<Student>();
    }
}
