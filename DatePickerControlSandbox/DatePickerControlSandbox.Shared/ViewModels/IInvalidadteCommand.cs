using System.Windows.Input;

namespace DatePickerControlSandbox.Shared.ViewModels
{
    public interface IInvalidateCommand : ICommand
    {
        void InvalidateCanExecute();
    }
}