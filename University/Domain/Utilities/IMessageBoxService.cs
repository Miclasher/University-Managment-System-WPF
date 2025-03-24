using System.Windows;

namespace University.Domain.Utilities
{
    public interface IMessageBoxService
    {
        public MessageBoxResult Show(string message, string title, MessageBoxButton button);
    }
}
