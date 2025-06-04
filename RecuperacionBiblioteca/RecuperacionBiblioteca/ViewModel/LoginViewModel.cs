using RecuperacionBiblioteca.Model;
using RecuperacionBiblioteca.Service;
using RecuperacionBiblioteca.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RecuperacionBiblioteca.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        #region PROPIEDADES
        private string _username;
        private string _password;
        private string _errorMessage;

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;  
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        #endregion

        #region COMANDOS
        public RelayCommand LoginCommand { get; set; }

        #endregion

        private LoginView _windowLogin;
        private LoginService _loginService;

        public LoginViewModel(LoginView loginView)
        {
            _windowLogin = loginView;
            _loginService = new LoginService();
            LoginCommand = new RelayCommand(
                _ => GoToLogin(),
                _ => CheckLogin()
            );
        }

        public void GoToLogin()
        {
            UsuarioModel usuarioLogin = _loginService.GetUsuarioLogin(Username, Password);
            if (usuarioLogin != null && usuarioLogin.Rol.Equals("usuario"))
            {
                BibliotecaView bibliotecaView = new BibliotecaView(usuarioLogin);
                bibliotecaView.Show();
                _windowLogin.Close();
            } 
            else if (usuarioLogin != null && usuarioLogin.Rol.Equals("admin"))
            {
                BibliotecaAdminView bibliotecaAdminView = new BibliotecaAdminView();
                bibliotecaAdminView.Show();
                _windowLogin.Close();
            }
            else
            {
                ErrorMessage = "Usuario o contraseña incorrectos";
            }
        }

        private bool CheckLogin()
        {
            bool check = false;
            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
            {
                check = true;
            }
            return check;
        }

        #region NOTIFICACION DE CAMBIOS
        //Para las notificaciones de cambio en la vista.
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
    }
}
