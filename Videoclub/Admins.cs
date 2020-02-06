using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Linq;

namespace Videoclub
{
    class Admins

        /*Los administradores tienen tres niveles:
        Nivel 0: Puede crear administradores nivel 1 o nivel 2. Puede modificar la tabla de usuarios y la de películas. (Master/master123)
        Nivel 1: Puede modificar la tabla de usuarios y la de películas
        Nivel 2: Puede modificar la tabla de películas.
        */
    {
        static SqlConnection connection = new SqlConnection("Data Source=LAPTOP-GGP\\SQLEXPRESS;Initial Catalog=Videoclub;Integrated Security=True");
        static string query;
        static SqlCommand command = new SqlCommand();
        static SqlDataReader reader;
        public static List<Admins> ListaAdmins = new List<Admins>();

        public int ID_Admin { get; set; }
        public string Admin_ID { get; set; }
        public string Pass { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Level { get; set; }
        public Admins() { }
        public Admins(int ID_Admin_tabla, string Administrador_ID, string pass, string nombre, string apellido, int nivel)
        {
            ID_Admin = ID_Admin_tabla;
            Admin_ID = Administrador_ID;
            Pass = pass;
            Name = nombre;
            LastName = apellido;
            Level = nivel;
        }
        public Admins(string Administrador_ID, string pass, string nombre, string apellido, int nivel)
        {
            Admin_ID = Administrador_ID;
            Pass = pass;
            Name = nombre;
            LastName = apellido;
            Level = nivel;
        }

        public List<Admins> ListarAdmins()
        {
            ListaAdmins.Clear();
            query = $"SELECT * FROM Administradores";
            connection.Open();
            command = new SqlCommand(query, connection);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Admins nuevo = new Admins(Convert.ToInt32(reader[0]), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), Convert.ToInt32(reader[5]));
                ListaAdmins.Add(nuevo);
            }
            connection.Close();
            return ListaAdmins;
        }
        public void GenerrNuevoAdmin(string IDadmin, string pass, string nombre, string apellido, int nivel)
        {
            query = $"INSERT INTO Administradores VALUES ('{IDadmin}', '{pass}', '{nombre}', '{apellido}', '{nivel}')";
            connection.Open();
            command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();
            connection.Close();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Clear();
            Console.WriteLine($"\nSe ha agregado un nuevo administrador nivel {nivel}\n");
            Console.ForegroundColor = ConsoleColor.White;
        }

    }
}
