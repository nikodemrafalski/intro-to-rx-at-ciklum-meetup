using System;
using System.Windows.Input;

namespace WpfClient
{
    public class ActionCommand : ICommand
    {
        private readonly Action<object> _action;

        public ActionCommand(Action<object> action)
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