using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Linq;

namespace Videoclub
{
    class Rental
    {
        static SqlConnection connection = new SqlConnection("Data Source=LAPTOP-GGP\\SQLEXPRESS;Initial Catalog=Videoclub;Integrated Security=True");
        static string query;
        static SqlCommand command = new SqlCommand();
        static SqlDataReader reader;
        public static List<Rental> ListaAlquileres = new List<Rental>();

        public int Rental_ID { get; set; }
        public int Movie_ID { get; set; }
        public int User_ID { get; set; }
        public string Date_taken { get; set; }
        public string Date_return { get; set; }

        public Rental()
        {
        }
        public Rental(int ID_alquiler, int ID_movie, int ID_user, string rental_date, string return_date)
        {
            Rental_ID = ID_alquiler;
            User_ID = ID_user;
            Movie_ID = ID_movie;
            Date_taken = rental_date;
            Date_return = return_date;
        }
        public Rental(int ID_movie, int ID_user, string rental_date, string return_date)
        {
            User_ID = ID_user;
            Movie_ID = ID_movie;
            Date_taken = rental_date;
            Date_return = return_date;
        }

        public void Reservar(int ID_movie, int ID_user)
        {
            query = $"INSERT INTO Alquiler (ID_Pelicula, ID_Usuario, Fecha_Alq) VALUES ({ID_movie}, {ID_user},'{DateTime.Now.ToString("yyyy/MM/dd")}')";
            connection.Open();
            command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();
            connection.Close();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\tSe ha registrado el alquiler correctamente\n Tienes 3 días para devolver la película");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public void Devolver(int ID_Alquiler)
        {
            query = $"UPDATE Alquiler SET Fecha_Dev = '{DateTime.Now.ToString("yyyy/MM/dd")}' WHERE ID_Alquiler = {ID_Alquiler}";
            connection.Open();
            command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();
            connection.Close();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\nSe ha devuelto la película correctamente");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public List<Rental> ListarAlquileres(int User_ID)
        {
            ListaAlquileres.Clear();
            query = $"SELECT * FROM Alquiler WHERE ID_Usuario = {User_ID} and Fecha_dev IS NULL";
            connection.Open();
            command = new SqlCommand(query, connection);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Rental nuevo = new Rental(Convert.ToInt32(reader[0]), Convert.ToInt32(reader[1]), Convert.ToInt32(reader[2]), reader[3].ToString(), reader[4].ToString());
                ListaAlquileres.Add(nuevo);
            }
            connection.Close();
            return ListaAlquileres;
        }
        public bool ComprobarRentalUser(int UserID)
        {
            query = $"SELECT * FROM Alquiler WHERE ID_Usuario = {UserID} and Fecha_dev IS NULL";
            connection.Open();
            command = new SqlCommand(query, connection);
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                connection.Close();
                return true;
            }
            else
            {
                connection.Close();
                return false;
            }
        }
    }
}
