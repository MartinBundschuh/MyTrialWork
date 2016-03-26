using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LyncSample;
using System.Xaml;

namespace LyncSampleUser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string textSave = "Enter a phonenumber to start a call";

        public MainWindow()
        {
            InitializeComponent();
            textBoxPhoneNumber.CaretIndex = 0;
        }

        private void buttonCall_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var phoneNumber = new PhoneNumber(textBoxPhoneNumber.Text);
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

        private void textBoxPhoneNumber_GotFocus(object sender, RoutedEventArgs e)
        {
            if (string.Compare(textBoxPhoneNumber.Text, textSave) == 0)
            {                
                textBoxPhoneNumber.Foreground = Brushes.Black;
                textBoxPhoneNumber.Text = string.Empty;
                textBoxPhoneNumber.FontStyle = FontStyles.Normal;
            }
        }

        private void textBoxPhoneNumber_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxPhoneNumber.Text))
            {
                textBoxPhoneNumber.Text = textSave;
                textBoxPhoneNumber.Foreground = Brushes.Gray;
                textBoxPhoneNumber.FontStyle = FontStyles.Italic;
            }
        }
    }
}
