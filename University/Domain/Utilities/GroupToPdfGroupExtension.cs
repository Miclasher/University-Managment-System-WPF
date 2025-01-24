using University.DataLayer.Models;
using University.Domain.Models;

namespace University.Domain.Utilities
{
    public static class GroupToPdfGroupExtension
    {
        public static PdfGroup ToPdfGroup(this Group group)
        {
            return new PdfGroup
            {
                CourseName = group.Course.Name,
                Name = group.Name,
                TutorFullName = $"{group.Teacher.FirstName} {group.Teacher.LastName}",
                FullNames = group.Students.Select(s => $"{s.FirstName} {s.LastName}").ToList()
            };
        }
    }
}
