using System;
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
using Microsoft.Win32;
using System.Web;
using System.Data;
using ClosedXML.Excel;
using System.Windows;
using System.Globalization;



namespace RecuperacionBiblioteca.Service
{
    public class BibliotecaService
    {
        //Conexión a la BD.
        private string connectionString = ConfigurationManager.ConnectionStrings["Conexion_App"].ConnectionString;
        private ObservableCollection<LibroModel> _listaLibros {  get; set; }
        private ObservableCollection<LibroModel> _listaLibrosAndFav {  get; set; }
        private ObservableCollection<LibroModel> _listaFav {  get; set; }

        public ObservableCollection<LibroModel> GetAllLibros(String TextBox=null)
        {
            _listaLibros = new ObservableCollection<LibroModel>();
            LibroModel libro = null;

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                string query = "";
                if (TextBox == null)
                {
                    query = @"SELECT IdLibro, Titulo, Autor, Genero, Anio, ISBN, Sinopsis, Imagen FROM Libros";
                } else
                {
                    query = @"SELECT IdLibro, Titulo, Autor, Genero, Anio, ISBN, Sinopsis, Imagen FROM Libros
                                WHERE Titulo LIKE @findTitulo OR Autor LIKE @findAutor OR Genero LIKE @findGenero";
                }

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    if (TextBox != null)
                    {
                        cmd.Parameters.AddWithValue("@findTitulo", TextBox);
                        cmd.Parameters.AddWithValue("@findAutor", TextBox);
                        cmd.Parameters.AddWithValue("@findGenero", TextBox);
                    }
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

        public void AddLibro(LibroModel libro, byte[] libroImg)
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

        public void DeleteLibro(LibroModel libro)
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

        public void ExportAllLibros()
        {
            SaveFileDialog savefiledialog = new SaveFileDialog();
            savefiledialog.Title = "Guardar reporte";
            savefiledialog.Filter = "Archivos Excel (*.xlsx)|*.xlsx";
            savefiledialog.FileName = "Reporte_libros.xlsx";

            if (savefiledialog.ShowDialog() == true)
            {
                string ruta = savefiledialog.FileName;

                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    conexion.Open();
                    string query = @"SELECT IdLibro, Titulo, Autor, Genero, Anio, Isbn, Sinopsis FROM Libros";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                var hoja = wb.Worksheets.Add(dt, "Libros");
                                hoja.Columns().AdjustToContents();
                                hoja.Row(1).Style.Font.Bold = true;
                                var lastColumn = hoja.LastColumnUsed().ColumnNumber();
                                var lastRow = hoja.LastRowUsed().RowNumber();
                                hoja.Range(1, 1, 1, lastColumn).Style.Fill.SetBackgroundColor(XLColor.BabyBlue);
                                hoja.Range(2, 1, lastRow, lastColumn).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                                wb.SaveAs(ruta);
                            }
                        }
                    }
                }
                MessageBox.Show($"Report guardado en: \n{ruta}", "Generación correcta.", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public ObservableCollection<LibroModel> GetAllLibrosAndFav(UsuarioModel usuario)
        {
            _listaLibrosAndFav = new ObservableCollection<LibroModel>();
            LibroModel libro = null;

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                string query = @"SELECT l.IdLibro, l.Titulo, l.Autor, l.Genero, l.Anio, l.ISBN, l.Sinopsis, l.Imagen,
                                CASE WHEN f.Id IS NOT NULL THEN 1 ELSE 0
                                END AS EsFavorito, f.FechaGuardado
                                FROM Libros l LEFT JOIN Favoritos f ON l.IdLibro = f.IdLibro AND f.IdUsuario=@IdUsuario
                                ORDER BY l.Titulo";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@IdUsuario", usuario.Id);
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
                            bool isFav = false;
                            int favorito = Convert.ToInt32(reader["EsFavorito"]);
                            if (favorito == 1)
                            {
                                isFav = true;
                            }
                            libro = new LibroModel(libroId, titulo, autor, genero, anio, isbn, sinopsis, imagenLibro, isFav);
                            _listaLibrosAndFav.Add(libro);
                        }
                    }
                }
            }
            return _listaLibrosAndFav;
        }

        public void UpdateLibro(LibroModel libroSeleccionado, byte[] imagenLibro)
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

        internal void AddFav(LibroModel libro, UsuarioModel usuario)
        {
            DateTime horaActual = DateTime.UtcNow;
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                string query = "INSERT INTO Favoritos (IdUsuario, IdLibro, FechaGuardado) VALUES (@idUsuario, @idLibro, @fecha)";
                
                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@idUsuario", usuario.Id);
                    cmd.Parameters.AddWithValue("@idLibro", libro.Id);
                    cmd.Parameters.AddWithValue("@fecha", horaActual);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        internal void DeleteFav(LibroModel libroSeleccionado, UsuarioModel usuario)
        {
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                string query = "DELETE FROM Favoritos WHERE IdLibro=@idLibro AND IdUsuario=@idUsuario";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@idLibro", libroSeleccionado.Id);
                    cmd.Parameters.AddWithValue("@idUsuario", usuario.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        internal void ExportFav(UsuarioModel usuario)
        {
            SaveFileDialog savefiledialog = new SaveFileDialog();
            savefiledialog.Title = "Guardar reporte";
            savefiledialog.Filter = "Archivos Excel (*.xlsx)|*.xlsx";
            savefiledialog.FileName = "Reporte_.Favoritosxlsx";

            if (savefiledialog.ShowDialog() == true)
            {
                string ruta = savefiledialog.FileName;

                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    conexion.Open();
                    string query = @"SELECT l.IdLibro, l.Titulo, l.Autor, l.Genero, l.Anio, l.Isbn, l.Sinopsis FROM Libros l 
                                    JOIN Favoritos f ON l.IdLibro = f.IdLibro
                                    WHERE f.IdUsuario=@idUsuario";

                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@idUsuario", usuario.Id);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                var hoja = wb.Worksheets.Add(dt, "Libros");
                                hoja.Columns().AdjustToContents();
                                hoja.Row(1).Style.Font.Bold = true;
                                var lastColumn = hoja.LastColumnUsed().ColumnNumber();
                                var lastRow = hoja.LastRowUsed().RowNumber();
                                hoja.Range(1, 1, 1, lastColumn).Style.Fill.SetBackgroundColor(XLColor.BabyBlue);
                                hoja.Range(2, 1, lastRow, lastColumn).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                                wb.SaveAs(ruta);
                            }
                        }
                    }
                }
                MessageBox.Show($"Report guardado en: \n{ruta}", "Generación correcta.", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        internal ObservableCollection<LibroModel> ShowFav(UsuarioModel usuario)
        {
            _listaFav = new ObservableCollection<LibroModel>();
            LibroModel libro = null;
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                string sql = @"SELECT l.IdLibro, l.Titulo, l.Autor,l.Genero , l.Anio, l.ISBN, l.Sinopsis, l.Imagen, 
                                CASE WHEN f.Id IS NOT NULL THEN 1 ELSE 0 END AS EsFavorito
                                FROM Libros l JOIN Favoritos f ON l.IdLibro = f.IdLibro
                                WHERE f.IdUsuario = @idUsuario";
                using (SqlCommand cmd = new SqlCommand(sql, conexion))
                {
                    cmd.Parameters.AddWithValue("@idUsuario", usuario.Id);
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
                            bool isFav = false;
                            int favorito = Convert.ToInt32(reader["EsFavorito"]);
                            if (favorito == 1)
                            {
                                isFav = true;
                            }
                            libro = new LibroModel(libroId, titulo, autor, genero, anio, isbn, sinopsis, imagenLibro, isFav);
                            _listaFav.Add(libro);
                        }
                    }
                }
            }
            return _listaFav;
        }

        internal void FindLibro(LibroModel libroSeleccionado, UsuarioModel usuario)
        {
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                string sql = "SELECT ";
            }
        }
    }
}
