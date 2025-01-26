using University.Domain.ViewModels;
using University.Tests.Utilities;

namespace University.Tests
{
    [TestClass]
    public class GroupsViewModelTests : CrudTestsSetup
    {
        private GroupsViewModel _viewModel = null!;

        [TestInitialize]
        public void TestSetup()
        {
            _viewModel = new GroupsViewModel(Context, new MockMessageBoxService());
        }

        [TestMethod]
        public void AddGroup()
        {
            var initialCount = _viewModel.Groups.Count;

            _viewModel.AddGroupCommand.Execute(null);

            Assert.AreEqual(initialCount + 1, _viewModel.Groups.Count);
            Assert.AreEqual("New Group", _viewModel.SelectedGroup.Name);
        }

        [TestMethod]
        public void DeleteGroup()
        {
            _viewModel.AddGroupCommand.Execute(null);
            var groupToDelete = _viewModel.SelectedGroup;
            var initialCount = _viewModel.Groups.Count;

            _viewModel.DeleteGroupCommand.Execute(null);

            Assert.AreEqual(initialCount - 1, _viewModel.Groups.Count);
            Assert.IsFalse(_viewModel.Groups.Contains(groupToDelete));
        }

        [TestMethod]
        public void SaveGroup()
        {
            _viewModel.AddGroupCommand.Execute(null);
            var groupToUpdate = _viewModel.SelectedGroup;
            groupToUpdate.Name = "UpdatedGroupName";

            _viewModel.SaveGroupCommand.Execute(null);

            var updatedGroup = Context.Groups.First(g => g.Id == groupToUpdate.Id);
            Assert.AreEqual("UpdatedGroupName", updatedGroup.Name);
        }
    }
}
