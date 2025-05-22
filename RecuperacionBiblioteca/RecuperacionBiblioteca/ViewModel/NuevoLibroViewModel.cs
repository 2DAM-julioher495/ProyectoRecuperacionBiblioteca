using Microsoft.Win32;
using RecuperacionBiblioteca.Model;
using RecuperacionBiblioteca.Service;
using RecuperacionBiblioteca.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RecuperacionBiblioteca.ViewModel
{
    public class NuevoLibroViewModel : INotifyPropertyChanged
    {
        private bool _checkVWindow;
        private NuevoLibroView _ventanaCrear;
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
        public RelayCommand AddLibroCommand { get; set; }
        public RelayCommand EditLibroCommand { get; set; }
        public RelayCommand LoadImageCommand { get; set; }
        public RelayCommand Cancel { get; set; }
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

        private Visibility _visibleCrear = Visibility.Visible;

        private Visibility _visibleEditar = Visibility.Hidden;

        public Visibility VisibleCrear
        {
            get => _visibleCrear;
            set
            {
                _visibleCrear = value;
                OnPropertyChanged(nameof(VisibleCrear));
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
        public NuevoLibroViewModel(NuevoLibroView _ventanaCrearThis, BibliotecaService biblioService, LibroModel libroSelectPrincipal)
        {
            _libroSeleccionado = libroSelectPrincipal;
            _ventanaCrear = _ventanaCrearThis;
            _checkVWindow = false;
            _bibliotecaService = biblioService;
            Libros = new ObservableCollection<LibroModel>();
            LoadLibroEdit();
            LoadCommand();
        }
        #endregion

        public void LoadCommand()
        {
            AddLibroCommand = new RelayCommand(
                _ => NewLibro(),
                _ => true //TODO: poder activar solo cuando no esté ninguna opción marcada
            );

            Cancel = new RelayCommand(
                _ => CloseWindow(),
                _ => true
            );

            EditLibroCommand = new RelayCommand(
                _ => EditLibro(),
                _ => LibroSeleccionado != null
            );

            LoadImageCommand = new RelayCommand(
                _ => UploadImagenLibro(),
                _ => true
            );

            //TODO: botón para cancelar selección.
        }
        public void LoadLibroEdit()
        {
            if (_libroSeleccionado != null)
            {
                Titulo = _libroSeleccionado.Titulo;
                Autor = _libroSeleccionado.Autor;
                Genero = _libroSeleccionado.Genero;
                Anio = _libroSeleccionado.Anio;
                Isbn = _libroSeleccionado.Isbn;
                Sinopsis = _libroSeleccionado.Sinopsis;
                Imagen = _libroSeleccionado.Imagen;

                VisibleCrear = Visibility.Hidden;
                VisibleEditar = Visibility.Visible;
            }
        }

        public void NewLibro()
        {
            // TODO: validar que los campos son introducidos adecuadamente.
            int idLibro = Libros.Count + 1;
            LibroModel libro = new LibroModel(idLibro, Titulo, Autor, Genero, Anio, Isbn, Sinopsis, Imagen);
            _bibliotecaService.AddLibro(libro, libroImg);
            LimpiarFormulario();
            _ventanaCrear.Close();
        }

        private byte[] libroImg;
        private bool imagenSubida = false;
        public void UploadImagenLibro()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Imágenes | *.png; *.jpg; *.jpeg;";
            try
            {
                if (openFile.ShowDialog() == true)
                {
                    string imagenSeleccionada = openFile.FileName;
                    libroImg = File.ReadAllBytes(imagenSeleccionada);
                    BitmapImage bitmap = new BitmapImage(new Uri(imagenSeleccionada));
                    Imagen = bitmap;
                    imagenSubida = true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error al cargar la imagen: {e.Message}");
            }
        }
        private void LimpiarFormulario()
        {
            Titulo = null;
            Autor = null;
            Genero = null;
            Anio = 0;
            Isbn = 0;
            Sinopsis = null;
            Imagen = null;
            LibroSeleccionado = null;

            VisibleCrear = Visibility.Visible;
            VisibleEditar = Visibility.Hidden;
        }

        public void EditLibro()
        {

            if (LibroSeleccionado != null)
            {

                _libroSeleccionado.Titulo = Titulo;
                _libroSeleccionado.Autor = Autor;
                _libroSeleccionado.Genero = Genero;
                _libroSeleccionado.Anio = Anio;
                _libroSeleccionado.Isbn = Isbn;
                _libroSeleccionado.Sinopsis = Sinopsis;
                _libroSeleccionado.Imagen = Imagen;

                _bibliotecaService.UpdateLibro(LibroSeleccionado, imagenSubida == false ? null : libroImg);

                LimpiarFormulario();
                _ventanaCrear.Close();
            }
        }

        public void CloseWindow()
        {
            _checkVWindow = false;
            LimpiarFormulario();
            _ventanaCrear.Close();
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
