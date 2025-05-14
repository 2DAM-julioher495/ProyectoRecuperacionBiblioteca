using RecuperacionBiblioteca.ViewModel;
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
using System.Windows.Shapes;

namespace RecuperacionBiblioteca.View
{
    /// <summary>
    /// Lógica de interacción para LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        private LoginViewModel _loginViewModel;
        public LoginView()
        {
            InitializeComponent();
            _loginViewModel = new LoginViewModel(this);
            this.DataContext = _loginViewModel;
        }

        private void PB_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _loginViewModel.Password = ((PasswordBox)sender).Password;
        }
    }
}
