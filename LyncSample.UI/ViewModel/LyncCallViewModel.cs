using LyncSample.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace LyncSample.UI.ViewModel
{
    public class LyncCallViewModel : INotifyPropertyChanged
    {
        internal const string textSave = "Enter a phonenumber to start a call";

        public LyncCallViewModel()
        {
            PhoneNumber = textSave;
            ForeGroundColor = Brushes.Gray;
            FontStyle = FontStyles.Italic;
            startCall = new Call(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                var propertyCanged = PropertyChanged;
                if (propertyCanged != null)
                    propertyCanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private Brush foregroundColor;
        public Brush ForeGroundColor
        {
            get { return foregroundColor; }
            set
            {
                foregroundColor = value;
                OnPropertyChanged(nameof(ForeGroundColor));
            }
        }

        private FontStyle fontStyle;
        public FontStyle FontStyle
        {
            get { return fontStyle; }
            set
            {
                fontStyle = value;
                OnPropertyChanged(nameof(FontStyle));
            }
        }

        private string phoneNumber;
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                phoneNumber = value;
                SetForeGroundColorAndFontStyle();
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        private Call startCall;
        public Call StartCall
        {
            get { return startCall; }
        }

        private void SetForeGroundColorAndFontStyle()
        {
            if (string.Compare(phoneNumber, textSave) == 0)
            {
                ForeGroundColor = Brushes.Gray;
                FontStyle = FontStyles.Italic;
            }
            else
            {
                ForeGroundColor = Brushes.Black;
                FontStyle = FontStyles.Normal;
            }
        }

        public class Call : ICommand
        {
            private LyncCallViewModel ViewModel;
            public Call(LyncCallViewModel viewModel)
            {
                ViewModel = viewModel;
                viewModel.PropertyChanged += (s, e) =>
                {
                    if (CanExecuteChanged != null)
                    {
                        var canExecuteCanged = CanExecuteChanged;
                        if (canExecuteCanged != null)
                            canExecuteCanged(this, new EventArgs());
                    }
                };
            }

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return !string.IsNullOrWhiteSpace(ViewModel.PhoneNumber)
                    && string.Compare(ViewModel.PhoneNumber, LyncCallViewModel.textSave, true) != 0;
            }

            public void Execute(object parameter)
            {
                try
                {
                    var phoneNumber = new PhoneNumber(ViewModel.PhoneNumber);
                    if (LyncCall.IsSignedIn)
                        LyncCall.Call(phoneNumber);
                }
                catch (Exception exception)
                {
                    if (exception.GetType() == typeof(InvalidPhoneNumberException))
                    {
                        // Do Sth.
                    }

                    if (exception.GetType() == typeof(NoSuccessfulCallException))
                    {
                        // Do Sth else.
                    }
                }
            }
        }
    }
}
