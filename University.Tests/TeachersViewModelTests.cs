using University.Domain.ViewModels;
using University.Tests.Utilities;

namespace University.Tests
{
    [TestClass]
    public class TeachersViewModelTests : CrudTestsSetup
    {
        private TeachersViewModel _viewModel = null!;

        [TestInitialize]
        public void TestSetup()
        {
            _viewModel = new TeachersViewModel(Context, new MockMessageBoxService());
        }

        [TestMethod]
        public void AddTeacher()
        {
            var initialCount = _viewModel.Teachers.Count;

            _viewModel.AddTeacherCommand.Execute(null);

            Assert.AreEqual(initialCount + 1, _viewModel.Teachers.Count);
            Assert.AreEqual("New", _viewModel.SelectedTeacher.FirstName);
            Assert.AreEqual("Teacher", _viewModel.SelectedTeacher.LastName);
        }

        [TestMethod]
        public void DeleteTeacher()
        {
            _viewModel.AddTeacherCommand.Execute(null);
            var teacherToDelete = _viewModel.SelectedTeacher;
            var initialCount = _viewModel.Teachers.Count;

            _viewModel.DeleteTeacherCommand.Execute(null);

            Assert.AreEqual(initialCount - 1, _viewModel.Teachers.Count);
            Assert.IsFalse(_viewModel.Teachers.Contains(teacherToDelete));
        }

        [TestMethod]
        public void SaveTeacher()
        {
            _viewModel.AddTeacherCommand.Execute(null);
            var teacherToUpdate = _viewModel.SelectedTeacher;
            teacherToUpdate.FirstName = "UpdatedFirstName";
            teacherToUpdate.LastName = "UpdatedLastName";

            _viewModel.SaveTeacherCommand.Execute(null);

            var updatedTeacher = Context.Teachers.First(t => t.Id == teacherToUpdate.Id);
            Assert.AreEqual("UpdatedFirstName", updatedTeacher.FirstName);
            Assert.AreEqual("UpdatedLastName", updatedTeacher.LastName);
        }
    }
}
