using System.Windows;

namespace University.Domain.Utilities
{
    public class MessageBoxService : IMessageBoxService
    {
        public MessageBoxResult Show(string message, string title, MessageBoxButton button)
        {
            return MessageBox.Show(message, title, button);
        }
    }
}
