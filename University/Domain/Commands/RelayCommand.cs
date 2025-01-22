using System.Windows.Input;

namespace University.Domain.Commands
{
    public class RelayCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private Action<object?> ExecuteAction { get; }
        private Predicate<object?> CanExecutePredicate { get; }

        public RelayCommand(Action<object?> execute, Predicate<object?> canExecute)
        {
            ExecuteAction = execute ?? throw new ArgumentNullException(nameof(execute));
            CanExecutePredicate = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        public bool CanExecute(object? parameter)
        {
            return CanExecutePredicate(parameter);
        }

        public void Execute(object? parameter)
        {
            ExecuteAction(parameter);
        }
    }
}
