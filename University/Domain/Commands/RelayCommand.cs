using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace University.Domain.Commands
{
    public class RelayCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged = null!;
        private Action<object> _Execute { get; }
        private Predicate<object> _CanExecute { get; }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _Execute = execute;
            _CanExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object? parameter)
        {
            throw new NotImplementedException();
        }
    }
}
