using RecuperacionBiblioteca.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecuperacionBiblioteca.Service
{
    public class LoginService
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["Conexion_App"].ConnectionString;
        public UsuarioModel GetUsuarioLogin(string correoUsuario, string passUsuario)
        {
            UsuarioModel usuario = null;
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                string query = @"SELECT IdUsuario, Nombre, Correo, Contrasena, Rol FROM Usuarios WHERE Correo = @correoUsuario";
                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@correoUsuario", correoUsuario);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string passHash = reader["Contrasena"].ToString();
    
                            if (BCrypt.Net.BCrypt.Verify(passUsuario, passHash))
                            {
                                int Id = Convert.ToInt32(reader["IdUsuario"].ToString());
                                string NombreUsuario = reader["Nombre"].ToString();
                                string CorreoUsuario = reader["Correo"].ToString();
                                string Password = passHash;
                                string RolUsuario = reader["Rol"].ToString();

                                usuario = new UsuarioModel(Id, NombreUsuario, CorreoUsuario, Password, RolUsuario);
                            }

                        }
                    }
                }
            }
            return usuario;
        }
    }
}
