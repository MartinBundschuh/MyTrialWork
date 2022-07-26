using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace LyncSample.UI.ViewModel
{
    public partial class LyncCallViewModel : INotifyPropertyChanged
    {
        internal const string TextSave = "Enter a phone number to start a call";
        
        private Brush _foregroundColor;
        private FontStyle _fontStyle;
        private string _phoneNumber;

        public LyncCallViewModel()
        {
            PhoneNumber = TextSave;
            ForeGroundColor = Brushes.Gray;
            FontStyle = FontStyles.Italic;
            StartCall = new Call(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var propertyChanged = PropertyChanged;
            propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Brush ForeGroundColor
        {
            get => _foregroundColor;
            set
            {
                _foregroundColor = value;
                OnPropertyChanged(nameof(ForeGroundColor));
            }
        }
        
        public FontStyle FontStyle
        {
            get => _fontStyle;
            set
            {
                _fontStyle = value;
                OnPropertyChanged(nameof(FontStyle));
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                SetForeGroundColorAndFontStyle();
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        public Call StartCall { get; }

        private void SetForeGroundColorAndFontStyle()
        {
            if (string.CompareOrdinal(_phoneNumber, TextSave) == 0)
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
    }
}
