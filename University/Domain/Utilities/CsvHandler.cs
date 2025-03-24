using System.IO;
using University.DataLayer.Models;

namespace University.Domain.Utilities
{
    public static class CsvHandler
    {
        public static IReadOnlyCollection<Student> ImportStudents(Stream fs)
        {
            ArgumentNullException.ThrowIfNull(fs);

            using var reader = new StreamReader(fs);

            var students = new List<Student>();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line!.Split(',');
                students.Add(new Student
                {
                    FirstName = values[0],
                    LastName = values[1]
                });
            }

            return students.AsReadOnly();
        }

        public static void ExportStudents(Stream fs, Group group)
        {
            ArgumentNullException.ThrowIfNull(fs);
            ArgumentNullException.ThrowIfNull(group);

            using var writer = new StreamWriter(fs, leaveOpen: true);
            foreach (var student in group.Students)
            {
                writer.WriteLine($"{student.FirstName},{student.LastName}");
            }
        }
    }
}
