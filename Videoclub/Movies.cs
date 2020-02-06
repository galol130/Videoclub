using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Linq;

namespace Videoclub
{
    class Movies
    {
        static SqlConnection connection = new SqlConnection("Data Source=LAPTOP-GGP\\SQLEXPRESS;Initial Catalog=Videoclub;Integrated Security=True");
        static string query;
        static SqlCommand command = new SqlCommand();
        static SqlDataReader reader;
        public static List<Movies> ListaCompleta = new List<Movies>();
        public static List<Movies> ListaFiltrada = new List<Movies>();

        public int Movie_ID { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string Sinopsis { get; set; }
        public int Min_Age { get; set; }

        public Movies() { }
        public Movies(int movie_ID, string title, string status, string sinopsis, int min_Age) //Para listar desde la BBDD
        {
            Movie_ID = movie_ID;
            Title = title;
            Status = status;
            Sinopsis = sinopsis;
            Min_Age = min_Age;
        }
        public Movies(string title, string status, string sinopsis, int min_Age) //Para cargar una nueva película a la BBDD
        {
            Title = title;
            Status = status;
            Sinopsis = sinopsis;
            Min_Age = min_Age;
        }

        public List<Movies> ListarTodo()
        {
            ListaCompleta.Clear();
            query = $"SELECT * FROM Peliculas";
            connection.Open();
            command = new SqlCommand(query, connection);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Movies nuevo = new Movies(Convert.ToInt32(reader[0]), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), Convert.ToInt32(reader[4]));
                ListaCompleta.Add(nuevo);
            }
            connection.Close();
            return ListaCompleta;
        }
        public List<Movies> ListarFiltrado(int age)
        {
            ListaFiltrada.Clear();
            query = $"SELECT * FROM Peliculas WHERE Edad_Min < {age}";
            connection.Open();
            command = new SqlCommand(query, connection);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Movies nuevo = new Movies(Convert.ToInt32(reader[0]), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), Convert.ToInt32(reader[4]));
                ListaFiltrada.Add(nuevo);
            }
            connection.Close();
            return ListaFiltrada;
        }
        public void Reservar(int ID_movie)
        {
            query = $"UPDATE Peliculas SET Estado='Alquilada' WHERE ID_Pelicula = {ID_movie}";
            connection.Open();
            command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void Devolver(int ID_movie)
        {
            query = $"UPDATE Peliculas SET Estado='Disponible' WHERE ID_Pelicula = {ID_movie}";
            connection.Open();
            command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
        public string GetName(int ID_movie)
        {
            string Titulo = "";
            query = $"SELECT * FROM Peliculas WHERE ID_Pelicula = {ID_movie}";
            connection.Open();
            command = new SqlCommand(query, connection);
            reader = command.ExecuteReader();
            reader.Read();
            Titulo = Convert.ToString(reader[1]);
            connection.Close();
            return Titulo;
        }
        public void AddMovie()
        {
            bool ok = false;
            do
            {
                string title = ""; string sinopsis = ""; int edadMin = 0;
                Console.WriteLine("\nIngrese el título de la película");
                title = Console.ReadLine();
                Console.WriteLine("\nIngrese la sinopsis");
                sinopsis = Console.ReadLine();
                Console.WriteLine("\nIngrese la edad mínima recomendada");
                edadMin = Convert.ToInt32(Console.ReadLine());
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine($"\n\tTítulo: {title}   -  Edad mínima recomendada: {edadMin}\n\tSinopsis: {sinopsis}");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("\n\tPara confirmar ingrese 1. Para cargar nuevamente ingrese 0");
                string input = Console.ReadLine();
                if (input == "1")
                {
                    query = $"INSERT INTO Peliculas VALUES ('{title}', 'Disponible', '{sinopsis}', '{edadMin}')";
                    connection.Open();
                    command = new SqlCommand(query, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Clear();
                    Console.WriteLine("\n\tLa película ha agregado exitosamente!\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadKey();
                    ok = true;
                }
            } while (!ok);


        }
        public void EraseMovie(int movieID)
        {
            string title = GetName(movieID);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"\tSe va a eliminar la película {movieID} - {title}.\n\tPara continuar ingrese 1. Para cancelar ingrese 9 ");
            Console.ResetColor();
            string input = Console.ReadLine();
            if (input != "1")
                Console.WriteLine("\n\tNo se ha modificado la película");
            else
            {
                query = $"DELETE FROM Peliculas WHERE ID_Pelicula = {movieID}";
                connection.Open();
                command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
                connection.Close();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Clear();
                Console.WriteLine("\n\tLa película ha sido eliminada\n");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
