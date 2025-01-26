using University.Domain.ViewModels;
using University.Tests.Utilities;

namespace University.Tests
{
    [TestClass]
    public class StudentsViewModelTests : CrudTestsSetup
    {
        private StudentsViewModel _viewModel = null!;

        [TestInitialize]
        public void TestSetup()
        {
            _viewModel = new StudentsViewModel(Context, new MockMessageBoxService());
        }

        [TestMethod]
        public void AddStudent()
        {
            var initialCount = _viewModel.Students.Count;

            _viewModel.AddStudentCommand.Execute(null);

            Assert.AreEqual(initialCount + 1, _viewModel.Students.Count);
            Assert.AreEqual("New", _viewModel.SelectedStudent.FirstName);
            Assert.AreEqual("Student", _viewModel.SelectedStudent.LastName);
        }

        [TestMethod]
        public void DeleteStudent()
        {
            _viewModel.AddStudentCommand.Execute(null);
            var studentToDelete = _viewModel.SelectedStudent;
            var initialCount = _viewModel.Students.Count;

            _viewModel.DeleteStudentCommand.Execute(null);

            Assert.AreEqual(initialCount - 1, _viewModel.Students.Count);
            Assert.IsFalse(_viewModel.Students.Contains(studentToDelete));
        }

        [TestMethod]
        public void SaveStudent()
        {
            _viewModel.AddStudentCommand.Execute(null);
            var studentToUpdate = _viewModel.SelectedStudent;
            studentToUpdate.FirstName = "UpdatedFirstName";
            studentToUpdate.LastName = "UpdatedLastName";

            _viewModel.SaveStudentCommand.Execute(null);

            var updatedStudent = Context.Students.First(s => s.Id == studentToUpdate.Id);
            Assert.AreEqual("UpdatedFirstName", updatedStudent.FirstName);
            Assert.AreEqual("UpdatedLastName", updatedStudent.LastName);
        }
    }
}
