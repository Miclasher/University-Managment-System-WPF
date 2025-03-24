using Microsoft.EntityFrameworkCore;
using University.DataLayer;
using University.DataLayer.Models;

namespace University.Tests
{
    public class CrudTestsSetup
    {
        protected UniversityContext Context = null!;
        private DbContextOptions<UniversityContext> _dbContextOptions = null!;

        [TestInitialize]
        public void Setup()
        {
            var uniqueDbName = $"TestDb_{Guid.NewGuid()}";
            _dbContextOptions = new DbContextOptionsBuilder<UniversityContext>()
                .UseInMemoryDatabase(uniqueDbName)
                .Options;

            Context = new UniversityContext(_dbContextOptions);

            SeedDatabase();
        }

        [TestCleanup]
        public void CleanUp()
        {
            Context!.Database.EnsureDeleted();
            Context.Dispose();
        }

        private void SeedDatabase()
        {
            var course = new Course
            {
                Id = Guid.NewGuid(),
                Name = "Computer Science",
                Description = "Introduction to Computer Science"
            };

            var teacher = new Teacher
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe"
            };

            var group = new Group
            {
                Id = Guid.NewGuid(),
                Name = "CS101",
                Course = course,
                Teacher = teacher
            };

            var students = new List<Student>
            {
                new Student
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Alice",
                    LastName = "Smith",
                    Group = group
                },
                new Student
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Bob",
                    LastName = "Johnson",
                    Group = group
                },
                new Student
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Charlie",
                    LastName = "Brown",
                    Group = group
                }
            };

            Context.Courses.Add(course);
            Context.Teachers.Add(teacher);
            Context.Groups.Add(group);
            Context.Students.AddRange(students);

            Context.SaveChanges();
        }
    }
}
