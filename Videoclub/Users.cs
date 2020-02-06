using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Linq;

namespace Videoclub
{
    class Users
    {
        static SqlConnection connection = new SqlConnection("Data Source=LAPTOP-GGP\\SQLEXPRESS;Initial Catalog=Videoclub;Integrated Security=True");
        static string query;
        static SqlCommand command = new SqlCommand();
        static SqlDataReader reader;
        public static List<Users> ListaUsuarios = new List<Users>();

        public int User_ID { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }

        public Users() { }
        public Users(string email, string password, string nombre, string apellido,DateTime fechanac) //Para crear nuevo usuario
        {
            Email = email;
            Pass = password;
            Name = nombre;
            LastName = apellido;
            BirthDate = fechanac.ToString("yyyy/MM/dd");
        }
        public Users(int idUsuario,string email, string password, string nombre, string apellido, DateTime fechanac) //Para armar listado de usuarios de la BBDD
        {
            User_ID = idUsuario;
            Email = email;
            Pass = password;
            Name = nombre;
            LastName = apellido;
            BirthDate = fechanac.ToString("yyyy/MM/dd");
        }
        
        public void RegitrarUsuario()   
        {
            query = $"INSERT INTO Usuarios VALUES ('{Email}', '{Pass}', '{Name}', '{LastName}', '{BirthDate}')";
            connection.Open();
            command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();
            connection.Close();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Clear();
            Console.WriteLine("\n\tEl usuario se ha registrado exitosamente!\n");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public  List<Users> ListarUsuarios() 
        {
            ListaUsuarios.Clear();
            query = $"SELECT * FROM Usuarios";
            connection.Open();
            command = new SqlCommand(query, connection);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Users nuevo = new Users(Convert.ToInt32(reader[0]), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), Convert.ToDateTime(reader[5]));
                ListaUsuarios.Add(nuevo);
            }
            connection.Close();
            return ListaUsuarios;
        }
        public void MostrarUsuarioEmail(string email)
        {
            query = $"SELECT * FROM Usuarios WHERE Email like '{email}'";
            connection.Open();
            command = new SqlCommand(query, connection);
            reader = command.ExecuteReader();
            reader.Read();
            Console.WriteLine($"\tNombre y Apellido: {reader[3]} {reader[4]}");
            connection.Close();
        }
        public void EliminarUsuario(int UserID)
        {
            query = $"DELETE FROM Usuarios WHERE ID_Usuario = {UserID}";
            connection.Open();
            command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();
            Console.WriteLine($"\nSe ha eliminado el usuario!");
            connection.Close();
        }

    }
}
