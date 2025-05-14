using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace RecuperacionBiblioteca.Model
{
    public class LibroModel : INotifyPropertyChanged
    {
        private int _id;
        private string _titulo;
        private string _autor;
        private string _genero;
        private int _anio;
        private long _isbn;
        private string _sinopsis;
        private BitmapImage _imagen;

        public int Id 
        { 
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
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

        public LibroModel(int id, string titulo, string autor, string genero, int anio, long isbn, string sinopsis, BitmapImage imagen)
        {
            _id = id;
            _titulo = titulo;
            _autor = autor;
            _genero = genero;
            _anio = anio;
            _isbn = isbn;
            _sinopsis = sinopsis;
            Imagen = imagen;
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
