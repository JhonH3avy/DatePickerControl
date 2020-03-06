using System;
using System.Collections.Generic;
using System.Text;

namespace DatePickerControlSandbox.Shared.ViewModels
{
    public class SimpleCommand : IInvalidateCommand
    {
        public Action<object> ActionToPerform { get; set; }

        private bool canExecute;

        public SimpleCommand(Action<object> executeAction)
        {
            ActionToPerform = executeAction;
            canExecute = true;
        }
        public bool CanExecute(object parameter)
        {
            return canExecute;
        }

        public void Execute(object parameter)
        {
            ActionToPerform.Invoke(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void InvalidateCanExecute()
        {
            canExecute = false;
        }
    }
}
