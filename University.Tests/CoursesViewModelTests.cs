using University.Domain.ViewModels;
using University.Tests.Utilities;

namespace University.Tests
{
    [TestClass]
    public class CoursesViewModelTests : CrudTestsSetup
    {
        private CoursesViewModel _viewModel = null!;

        [TestInitialize]
        public void TestSetup()
        {
            _viewModel = new CoursesViewModel(Context, new MockMessageBoxService());
        }

        [TestMethod]
        public void AddCourse()
        {
            var initialCount = _viewModel.Courses.Count;

            _viewModel.AddCourseCommand.Execute(null);

            Assert.AreEqual(initialCount + 1, _viewModel.Courses.Count);
            Assert.AreEqual("New course", _viewModel.SelectedCourse.Name);
            Assert.AreEqual("Write description here", _viewModel.SelectedCourse.Description);
        }

        [TestMethod]
        public void DeleteCourse()
        {
            _viewModel.AddCourseCommand.Execute(null);
            var courseToDelete = _viewModel.SelectedCourse;
            var initialCount = _viewModel.Courses.Count;

            _viewModel.DeleteCourseCommand.Execute(null);

            Assert.AreEqual(initialCount - 1, _viewModel.Courses.Count);
            Assert.IsFalse(_viewModel.Courses.Contains(courseToDelete));
        }

        [TestMethod]
        public void SaveCourse()
        {
            _viewModel.AddCourseCommand.Execute(null);
            var courseToUpdate = _viewModel.SelectedCourse;
            courseToUpdate.Name = "UpdatedCourseName";
            courseToUpdate.Description = "UpdatedCourseDescription";

            _viewModel.SaveCourseCommand.Execute(null);

            var updatedCourse = Context.Courses.First(c => c.Id == courseToUpdate.Id);
            Assert.AreEqual("UpdatedCourseName", updatedCourse.Name);
            Assert.AreEqual("UpdatedCourseDescription", updatedCourse.Description);
        }
    }
}
