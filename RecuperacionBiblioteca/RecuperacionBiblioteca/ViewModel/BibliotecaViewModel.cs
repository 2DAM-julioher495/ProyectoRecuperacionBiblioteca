using Microsoft.IdentityModel.Tokens;
using RecuperacionBiblioteca.Model;
using RecuperacionBiblioteca.Service;
using RecuperacionBiblioteca.View;
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
    public class BibliotecaViewModel : INotifyPropertyChanged
    {
        private readonly BibliotecaService _bibliotecaService;
        private ObservableCollection<LibroModel> _libros;
        private UsuarioModel _usuario;

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
        public RelayCommand ShowFavCommand {  get; set; }
        public RelayCommand AddFavCommand { get; set; }
        public RelayCommand DeleteFavCommand { get; set; }
        public RelayCommand ReportAllCommand {  get; set; }
        public RelayCommand ReportFavCommand { get; set; }
        public RelayCommand UnselectCommand { get; set; }

        #endregion

        #region PROPIEDADES NUEVO LIBRO
        private int _idLibro;
        private string _titulo;
        private string _autor;
        private string _genero;
        private int _anio;
        private long _isbn;
        private string _sinopsis;
        private string _favorito;
        private BitmapImage _imagen;
        private bool _isFav;

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

        public string Favorito
        {
            get => _favorito;
            set
            {
                _favorito = value;
                OnPropertyChanged(nameof(Favorito));
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

        public bool IsFav
        {
            get => _isFav;
            set
            {
                _isFav = value;
                OnPropertyChanged(nameof(IsFav));
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
        public BibliotecaViewModel(BibliotecaView bibliotecaView, UsuarioModel usuario)
        {
            _usuario = usuario;
            _bibliotecaService = new BibliotecaService();
            Libros = new ObservableCollection<LibroModel>();
            LoadData();
            LoadCommand();
        }
        #endregion

        public void LoadData()
        {
            Libros = _bibliotecaService.GetAllLibrosAndFav(_usuario);
        }

        #region FUNCIONES COMANDOS
        public void LoadCommand()
        {
            ShowFavCommand = new RelayCommand(
                _ => ShowFav(),
                _ => true
            );

            AddFavCommand = new RelayCommand(
                _ => AddFav(),
                _ => LibroSeleccionado != null
            );

            DeleteFavCommand = new RelayCommand(
                _ => DeleteFav(),
                _ => LibroSeleccionado != null
            );

            ReportAllCommand = new RelayCommand(
                _ => ReportAll(),
                _ => CheckLista()
            );

            ReportFavCommand = new RelayCommand(
                _ => ReportFav(),
                _ => CheckLista()
            );

            UnselectCommand = new RelayCommand(
                _ => Unselect(),
                _ => true
                );
        }

        public void ShowFav()
        {
            Libros = _bibliotecaService.ShowFav(_usuario);
        }

        public void AddFav()
        {
            if (LibroSeleccionado.IsFav == false)
            {
                _bibliotecaService.AddFav(LibroSeleccionado, _usuario);
                MessageBox.Show("Se ha añadido a favoritos correctamente.", "Éxito", MessageBoxButton.OK);
                LoadData();
            } else
            {
                MessageBox.Show("Error, este libro ya está marcado como favorito", "Error", MessageBoxButton.OK);
            }
        } 

        public void DeleteFav()
        {
            if (LibroSeleccionado.IsFav == true)
            {
                _bibliotecaService.DeleteFav(LibroSeleccionado, _usuario);
                MessageBox.Show("Desmarcado como favorito correctamente.", "Éxito", MessageBoxButton.OK);
                LoadData();
            } else
            {
                MessageBox.Show("Error, este libro NO está marcado como favorito", "Error", MessageBoxButton.OK);
            }
        }

        public void ReportAll()
        {
            _bibliotecaService.ExportAllLibros();
        }

        public void ReportFav()
        {
            if (CheckListExistsFav())
            {
                _bibliotecaService.ExportFav(_usuario);
            } else
            {
                MessageBox.Show("No hay ningún libro agregado a favoritos.", "Error", MessageBoxButton.OK);
            }
        }

        public void Unselect()
        {
            Titulo = null;
            Autor = null;
            Genero = null;
            Anio = 0;
            Isbn = 0;
            Sinopsis = null;
            Imagen = null;
            LibroSeleccionado = null;

            LoadData();

        }

        public bool CheckLista()
        {
            bool check = false;

            if (!Libros.IsNullOrEmpty())
            {
                check = true;
            }

            return check;
        }

        public bool CheckListExistsFav()
        {
            bool check = (Libros.Where(libro => libro.IsFav)).Any() ? true : false;

            return check;
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
