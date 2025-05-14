using RecuperacionBiblioteca.Model;
using RecuperacionBiblioteca.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace RecuperacionBiblioteca.ViewModel
{
    public class BibliotecaViewModel
    {
        private readonly BibliotecaService _bibliotecaService;
        private ObservableCollection<LibroModel> _libros;

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


        #region PROPIEDADES VISIBILIDAD

        private Visibility _visibleAñadir = Visibility.Visible;

        private Visibility _visibleEditar = Visibility.Hidden;

        public Visibility VisibleAñadir
        {
            get => _visibleAñadir;
            set
            {
                _visibleAñadir = value;
                OnPropertyChanged(nameof(VisibleAñadir));
            }
        }
        public Visibility VisibleEditar
        {
            get => _visibleEditar;
            set
            {
                _visibleEditar = value;
                OnPropertyChanged(nameof(VisibleEditar));
            }
        }

        #endregion

        #region CONSTRUCTOR
        public BibliotecaViewModel()
        {
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
