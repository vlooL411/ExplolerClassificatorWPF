using System;
using System.Windows.Input;

namespace ExplolerClassificatorWPF
{
    public class DelegateCommand : ICommand
    {
        Action<object> _execute;
        Func<object, bool> _canExecute;
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        public void Execute(object parameter) => _execute.Invoke(parameter);
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute.Invoke(parameter);
    }
}