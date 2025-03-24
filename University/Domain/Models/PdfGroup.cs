namespace University.Domain.Models
{
    public class PdfGroup
    {
        public String CourseName { get; set; } = string.Empty;
        public String Name { get; set; } = string.Empty;
        public String TutorFullName { get; set; } = string.Empty;
        public IReadOnlyCollection<String> FullNames { get; set; } = new List<String>();
    }
}
