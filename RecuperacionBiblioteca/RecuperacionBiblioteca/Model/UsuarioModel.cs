using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecuperacionBiblioteca.Model
{
    public class UsuarioModel
    {
        private int _id;
        private string _nombre;
        private string _correo;
        private string _contrasena;
        private string _rol;

        public int Id 
        { 
            get => _id;
            set => _id = value;
        }
        public string Nombre 
        { 
            get => _nombre; 
            set => _nombre = value; 
        }
        public string Correo 
        { 
            get => _correo; 
            set => _correo = value; 
        }
        public string Contrasena 
        { 
            get => _contrasena; 
            set => _contrasena = value; 
        }
        public string Rol 
        { 
            get => _rol; 
            set => _rol = value; 
        }

        public UsuarioModel(int id, string nombre, string correo, string contrasena, string rol)
        {
            Id = id;
            Nombre = nombre;
            Correo = correo;
            Contrasena = contrasena;
            Rol = rol;
        }
    }
}
