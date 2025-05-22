using RecuperacionBiblioteca.Model;
using RecuperacionBiblioteca.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using RecuperacionBiblioteca.View;

namespace RecuperacionBiblioteca.ViewModel
{
    public class BibliotecaAdminViewModel : INotifyPropertyChanged
    {
        private readonly BibliotecaService _bibliotecaService;
        private ObservableCollection<LibroModel> _libros;
        private NuevoLibroView _ventanaCrear;
        private BibliotecaAdminView _bibliotecaView;
        private LoginView _loginView;
        private bool _checkVWindow;

        public ObservableCollection<LibroModel> Libros
        {
            get => _libros;
            set
            {
                _libros = value;
                OnPropertyChanged(nameof(Libros));
            }
        }


        #region COMANDOS
        public RelayCommand EditLibroCommand { get; set; }
        public RelayCommand DeleteLibroCommand { get; set; }
        public RelayCommand GoToCreate {  get; set; }
        public RelayCommand LogoutCommand {  get; set; }
        public RelayCommand ReportCommand { get; set; }
        #endregion

        #region PROPIEDADES NUEVO LIBRO
        private int _idLibro;
        private string _titulo;
        private string _autor;
        private string _genero;
        private int _anio;
        private long _isbn;
        private string _sinopsis;
        private BitmapImage _imagen;

        public int IdLibro
        {
            get => _idLibro;
            set
            {
                _idLibro = value;
                OnPropertyChanged(nameof(IdLibro));
            }
        }
        public string Titulo
        {
            get => _titulo;
            set
            {
                _titulo = value;
                OnPropertyChanged(nameof(Titulo));
            }
        }
        public string Autor
        {
            get => _autor;
            set
            {
                _autor = value;
                OnPropertyChanged(nameof(Autor));
            }
        }
        public string Genero
        {
            get => _genero;
            set
            {
                _genero = value;
                OnPropertyChanged(nameof(Genero));
            }
        }
        public int Anio
        {
            get => _anio;
            set
            {
                _anio = value;
                OnPropertyChanged(nameof(Anio));
            }
        }
        public long Isbn
        {
            get => _isbn;
            set
            {
                _isbn = value;
                OnPropertyChanged(nameof(Isbn));
            }
        }
        public string Sinopsis
        {
            get => _sinopsis;
            set
            {
                _sinopsis = value;
                OnPropertyChanged(nameof(Sinopsis));
            }
        }
        public BitmapImage Imagen
        {
            get => _imagen;
            set
            {
                _imagen = value;
                OnPropertyChanged(nameof(Imagen));
            }
        }

        private LibroModel _libroSeleccionado;
        public LibroModel LibroSeleccionado
        {
            get => _libroSeleccionado;
            set
            {
                _libroSeleccionado = value;
                OnPropertyChanged(nameof(LibroSeleccionado));
            }
        }
        #endregion

        #region CONSTRUCTOR
        public BibliotecaAdminViewModel(BibliotecaAdminView biblioAdminViewThis)
        {
            _checkVWindow = false;
            _bibliotecaView = biblioAdminViewThis;
            _bibliotecaService = new BibliotecaService();
            Libros = new ObservableCollection<LibroModel>();
            LoadData();
            LoadCommand();
        }
        #endregion

        public void LoadData()
        {
            Libros = _bibliotecaService.GetAllLibros();
        }

        #region FUNCIONES COMANDOS
        public void LoadCommand()
        {
            EditLibroCommand = new RelayCommand(
                _ => CreateWindow(),
                _ => LibroSeleccionado != null
            );
            DeleteLibroCommand = new RelayCommand(
                _ => DeleteLibro(),
                _ => LibroSeleccionado != null
            );

            GoToCreate = new RelayCommand(
                _ => CreateWindow(),
                _ => !_checkVWindow
            );

            LogoutCommand = new RelayCommand(
                _ => Logout(),
                _ => true
            );

            ReportCommand = new RelayCommand(
                _ => DownloadAllLibros(),
                _ => true
            );
        }

        public void DeleteLibro()
        {
            var confirm = MessageBox.Show("¿Seguro que quieres eliminar el libro seleccionado?", "Confirmación", MessageBoxButton.OKCancel);

            if (confirm == MessageBoxResult.OK)
            {
                _bibliotecaService.DeleteLibro(LibroSeleccionado);
                LoadData();
            }
        }

        public void CreateWindow()
        {
            _ventanaCrear = new NuevoLibroView(_bibliotecaService, LibroSeleccionado);

            _ventanaCrear.ShowDialog();

            LoadData();
        }

        public void Logout()
        {
            _loginView = new LoginView();

            _loginView.Show();
            _bibliotecaView.Close();
        }

        public void DownloadAllLibros()
        {
            _bibliotecaService.ExportAllLibros();
        }

        #endregion

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
