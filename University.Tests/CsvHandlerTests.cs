using System.Collections.ObjectModel;
using University.DataLayer.Models;
using University.Domain.Utilities;
using University.Tests.Utilities;

namespace University.Tests
{
    [TestClass]
    public class CsvHandlerTests : CrudTestsSetup
    {
        [TestMethod]
        public void ImportStudents()
        {
            const string csvData = "John,Doe\nJane,Smith\nBob,Johnson";
            using var stream = StringToMemoryStream.Convert(csvData);

            var students = CsvHandler.ImportStudents(stream);

            Assert.AreEqual(3, students.Count);
            Assert.AreEqual("John", students.ElementAt(0).FirstName);
            Assert.AreEqual("Doe", students.ElementAt(0).LastName);
            Assert.AreEqual("Jane", students.ElementAt(1).FirstName);
            Assert.AreEqual("Smith", students.ElementAt(1).LastName);
            Assert.AreEqual("Bob", students.ElementAt(2).FirstName);
            Assert.AreEqual("Johnson", students.ElementAt(2).LastName);
        }

        [TestMethod]
        public void ExportStudents()
        {
            var group = new Group
            {
                Students = new ObservableCollection<Student>
                {
                    new Student { FirstName = "Alice", LastName = "Brown" },
                    new Student { FirstName = "Charlie", LastName = "Davis" }
                }
            };

            using var stream = new MemoryStream();

            CsvHandler.ExportStudents(stream, group);

            stream.Position = 0;
            using var reader = new StreamReader(stream);
            var result = reader.ReadToEnd();

            const string expected = "Alice,Brown\nCharlie,Davis\n";
            Assert.AreEqual(NormalizeLineEndings.Normalize(expected), NormalizeLineEndings.Normalize(result));
        }
    }
}