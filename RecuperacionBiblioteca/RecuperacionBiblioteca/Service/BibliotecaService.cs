﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.ObjectModel;
using RecuperacionBiblioteca.Model;
using Microsoft.Data.SqlClient;
using System.Windows.Media.Imaging;
using System.IO;



namespace RecuperacionBiblioteca.Service
{
    public class BibliotecaService
    {
        //Conexión a la BD.
        private string connectionString = ConfigurationManager.ConnectionStrings["Conexion_App"].ConnectionString;
        private ObservableCollection<LibroModel> _listaLibros {  get; set; }

        public ObservableCollection<LibroModel> GetAllLibros()
        {
            _listaLibros = new ObservableCollection<LibroModel>();
            LibroModel libro = null;

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                string query = @"SELECT IdLibro, Titulo, Autor, Genero, Anio, ISBN, Sinopsis, Imagen FROM Libros";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int libroId = Convert.ToInt32(reader["IdLibro"]);
                            string titulo = reader["Titulo"].ToString();
                            string autor = reader["Autor"].ToString();
                            string genero = reader["genero"].ToString();
                            int anio = Convert.ToInt32(reader["Anio"]);
                            long isbn = Convert.ToInt64(reader["ISBN"]);
                            string sinopsis = reader["Sinopsis"].ToString();
                            BitmapImage imagenLibro = null;

                            if (!string.IsNullOrEmpty(reader["Imagen"].ToString()))
                            {
                                byte[] imagenBytes = (byte[])reader["Imagen"];
                                BitmapImage bitmap = new BitmapImage();
                                using (MemoryStream ms = new MemoryStream(imagenBytes))
                                {
                                    bitmap.BeginInit();
                                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                    bitmap.StreamSource = ms;
                                    bitmap.EndInit();
                                }

                                imagenLibro = bitmap;

                            }
                            libro = new LibroModel(libroId, titulo, autor, genero, anio, isbn, sinopsis, imagenLibro);
                            _listaLibros.Add(libro);
                        }
                    }
                }
            }

            return _listaLibros;
        }

        internal void AddLibro(LibroModel libro, byte[] libroImg)
        {
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                string query = "";

                if (libroImg != null)
                {
                    query = @"INSERT INTO Libros (Titulo, Autor, Genero, Anio, ISBN, Sinopsis, Imagen) 
                            VALUES (@titulo, @autor, @genero, @anio, @isbn, @sinopsis, @imagen)";
                } else
                {
                    query = @"INSERT INTO Libros (Titulo, Autor, Genero, Anio, ISBN, Sinopsis) 
                            VALUES (@titulo, @autor, @genero, @anio, @isbn, @sinopsis)";
                }

                using (SqlCommand cmd = new SqlCommand(query, conexion)) 
                {
                    cmd.Parameters.AddWithValue("@titulo", libro.Titulo);
                    cmd.Parameters.AddWithValue("@autor", libro.Autor);
                    cmd.Parameters.AddWithValue("@genero", libro.Genero);
                    cmd.Parameters.AddWithValue("@anio", libro.Anio);
                    cmd.Parameters.AddWithValue("@isbn", libro.Isbn);
                    cmd.Parameters.AddWithValue("@sinopsis", libro.Sinopsis);

                    if (libroImg != null)
                    {
                        cmd.Parameters.AddWithValue("@imagen", libroImg);
                    }

                    cmd.ExecuteNonQuery();
                }
            }
        }

        internal void DeleteLibro(LibroModel libro)
        {
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                string query = @"DELETE FROM Libros WHERE IdLibro=@idLibro";
                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@idLibro", libro.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        internal void ExportAllLibros()
        {
            
        }

        internal void UpdateLibro(LibroModel libroSeleccionado, byte[] imagenLibro)
        {
            using (SqlConnection conexion = new SqlConnection (connectionString))
            {
                conexion.Open();
                string query = "";

                if (imagenLibro != null)
                {
                    query = @"UPDATE Libros 
                            SET Titulo=@titulo, Autor=@autor, Genero=@genero, Anio=@anio, ISBN=@isbn, Sinopsis=@sinopsis, Imagen=@imagen
                            WHERE IdLibro=@idLibro";
                } else
                {
                    query = @"UPDATE Libros 
                            SET Titulo=@titulo, Autor=@autor, Genero=@genero, Anio=@anio, ISBN=@isbn, Sinopsis=@sinopsis
                            WHERE IdLibro=@idLibro";
                }

                using (SqlCommand cmd = new SqlCommand (query, conexion))
                {
                    cmd.Parameters.AddWithValue("@titulo", libroSeleccionado.Titulo);
                    cmd.Parameters.AddWithValue("@autor", libroSeleccionado.Autor);
                    cmd.Parameters.AddWithValue("@genero", libroSeleccionado.Genero);
                    cmd.Parameters.AddWithValue("@anio", libroSeleccionado.Anio);
                    cmd.Parameters.AddWithValue("@isbn", libroSeleccionado.Isbn);
                    cmd.Parameters.AddWithValue("@sinopsis", libroSeleccionado.Sinopsis);
                    cmd.Parameters.AddWithValue("@idLibro", libroSeleccionado.Id);

                    if (imagenLibro != null)
                    {
                        cmd.Parameters.AddWithValue("@imagen", imagenLibro);
                    }

                    cmd.ExecuteNonQuery(); 
                }
            }
        }
    }
}
