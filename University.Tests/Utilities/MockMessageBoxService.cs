using System.Windows;
using University.Domain.Utilities;

namespace University.Tests.Utilities
{
    internal class MockMessageBoxService : IMessageBoxService
    {
        public MessageBoxResult Show(string message, string title, MessageBoxButton button)
        {
            return MessageBoxResult.Yes;
        }
    }
}
