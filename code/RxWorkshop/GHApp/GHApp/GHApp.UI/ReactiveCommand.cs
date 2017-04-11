using System;
using System.Windows.Input;

namespace GHApp.UI
{
    public class ReactiveCommand : ICommand
    {
        private readonly Action<object> _action;

        public ReactiveCommand(Action<object> action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action(parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}