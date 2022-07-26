using System;
using System.Windows.Input;
using LyncSample.Data;

namespace LyncSample.UI.ViewModel;

public partial class LyncCallViewModel
{
    public class Call : ICommand
    {
        private readonly LyncCallViewModel _viewModel;
            
        public Call(LyncCallViewModel viewModel)
        {
            _viewModel = viewModel;
            viewModel.PropertyChanged += (_, _) =>
            {
                var canExecuteChanged = CanExecuteChanged;
                canExecuteChanged?.Invoke(this, EventArgs.Empty);
            };
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) =>
            !string.IsNullOrWhiteSpace(_viewModel.PhoneNumber)
            && string.Compare(_viewModel.PhoneNumber, TextSave, StringComparison.OrdinalIgnoreCase) != 0;

        public void Execute(object parameter)
        {
            try
            {
                var phoneNumber = new PhoneNumber(_viewModel.PhoneNumber);
                if (LyncCall.IsSignedIn)
                {
                    LyncCall.Call(phoneNumber);
                }
            }
            catch (Exception exception)
            {
                if (exception.GetType() == typeof(InvalidPhoneNumberException))
                {
                    // Do sth.
                }

                if (exception.GetType() == typeof(NoSuccessfulCallException))
                {
                    // Do sth else.
                }
            }
        }
    }
}