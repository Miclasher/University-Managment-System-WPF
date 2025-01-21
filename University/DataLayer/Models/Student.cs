using System.ComponentModel.DataAnnotations.Schema;

namespace University.DataLayer.Models
{
    public class Student
    {
        [Column("STUDENT_ID")]
        public Guid Id { get; set; }
        [Column("FIRST_NAME")]
        public string FirstName { get; set; } = string.Empty;
        [Column("LAST_NAME")]
        public string LastName { get; set; } = string.Empty;
        [Column("GROUP_ID")]
        public Guid GroupId { get; set; }
        public Group Group { get; set; } = null!;
    }
}
