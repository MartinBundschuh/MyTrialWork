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
using LyncSample.Data;
using System.Xaml;
using LyncSample.UI.ViewModel;

namespace LyncSample.UI
{
    /// <summary>
    /// Interaction logic for LyncCallWindow.xaml
    /// </summary>
    public partial class LyncCallWindow : Window
    {
        private LyncCallViewModel viewModel;

        public LyncCallWindow()
        {
            InitializeComponent();
            viewModel = new LyncCallViewModel();
            Loaded += (s, e) => { DataContext = viewModel; };
        }

        private void textBoxPhoneNumber_GotFocus(object sender, RoutedEventArgs e)
        {
            if (string.Compare(viewModel.PhoneNumber, LyncCallViewModel.textSave, true) == 0)
                viewModel.PhoneNumber = string.Empty;
        }

        private void textBoxPhoneNumber_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(viewModel.PhoneNumber))
                viewModel.PhoneNumber = LyncCallViewModel.textSave;
        }
    }
}
