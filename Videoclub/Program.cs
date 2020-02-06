using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Linq;

namespace Videoclub
{
    class Program
    {
        static SqlConnection connection = new SqlConnection("Data Source=LAPTOP-GGP\\SQLEXPRESS;Initial Catalog=Videoclub2;Integrated Security=True");
        //static string query;
        static SqlCommand command = new SqlCommand();
        //static SqlDataReader reader;
        static Admins nuevoAdmin = new Admins();
        static List<Admins> ListaAdministradores = new List<Admins>();
        static Users nuevoUsuario = new Users();
        static List<Users> ListaUsuarios = new List<Users>();
        static Movies nuevaPelicula = new Movies();
        static List<Movies> ListaFiltrada = new List<Movies>();
        static List<Movies> ListaCompleta = new List<Movies>();
        static Rental nuevoAlquiler = new Rental();
        static List<Rental> ListaAlquiler = new List<Rental>();

        static void Main(string[] args)
        {
            Console.Title = "^^^^ BBK VIDEOCLUB ^^^^";
            Console.WindowHeight = 35;
            Console.WindowWidth = 150;
            string inputMenu1 = "";
            do
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                /*Console.WriteLine("    ***** BIENVENIDO AL VIDECOCLUB BBK ***** " +
                    "\n\t1.   Ingresar (usuarios registrados) " +
                    "\n\t2.   Registrarse (nuevo usuario) " +
                    "\n\t9.   Ingresar como Administrador" +
                    "\n\t0.   Salir");
                */
                Console.WriteLine("\t*********************************************************" +
                    "\n\t|\t    <<< BIENVENIDO AL VIDECOCLUB BBK >>> \t|" +
                    "\n\t|\tMenu Principal \t\t\t\t\t|" +
                    "\n\t|\t1.   Ingresar (usuarios registrados) \t\t|" +
                    "\n\t|\t2.   Registrarse (nuevo usuario) \t\t|" +
                    "\n\t|\t9.   Ingresar como Administrador \t\t|" +
                    "\n\t|\t0.   Salir \t\t\t\t\t|" +
                    "\n\t*********************************************************");

                Console.ForegroundColor = ConsoleColor.White;
                inputMenu1 = Console.ReadLine();

                switch (inputMenu1)
                {
                    case "1":
                        Console.Clear();
                        ListaUsuarios = nuevoUsuario.ListarUsuarios();
                        string email = ValidarUsuario();
                        if (email == "")
                            break;
                        if (ValidarPass(email))
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Clear();
                            Console.WriteLine("\n\tLog in exitoso!\n");
                            Console.ForegroundColor = ConsoleColor.White;
                            MenuUsuario(email);
                        }
                        break;
                    case "2":
                        string emailNuevo = ValidarNuevoEmail();
                        string claveNuevo = ValidarNuevoPass();
                        Console.WriteLine("Ingrese su Nombre");
                        string nombreNuevo = Console.ReadLine();
                        Console.WriteLine("Ingrese su Apellido");
                        string apellidoNuevo = Console.ReadLine();
                        Console.WriteLine("Ingrese su Fecha de Nacimiento (dd/MM/YYYY)");
                        string inputFecha = Console.ReadLine();
                        DateTime fechaNac;
                        if (DateTime.TryParse(inputFecha, out fechaNac))
                        {
                            fechaNac = Convert.ToDateTime(inputFecha);
                            Users nuevoUsuario = new Users(emailNuevo, claveNuevo, nombreNuevo, apellidoNuevo, fechaNac);
                            nuevoUsuario.RegitrarUsuario();
                        }
                        break;
                    case "9":
                        ListaAdministradores = nuevoAdmin.ListarAdmins();
                        string AdminID = ValidarAdminID();
                        if (ValidarAdminPass(AdminID))
                        {
                            Console.Clear();
                            MenuAdministrador(AdminID);
                        }
                        break;
                    case "0":
                        Console.Clear();
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nHa ingresado un valor incorrecto. Presione Enter para continuar");
                        Console.Beep(1400, 400);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.ReadKey();
                        break;
                }
                Console.Clear();
            } while (inputMenu1 != "0");
            //Environment.Exit(1);
        }
        public static string ValidarNuevoEmail()
        {
            bool ok = false;
            string email = "";
            do
            {
                Console.WriteLine("Ingrese su correo electrónico");
                email = Console.ReadLine();
                if (email.IndexOf("@") < 0 || email.IndexOf(".") < 0 || email.IndexOf(",") > 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Ha ingreado un formato incorrecto.\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    ok = true;
                }

            } while (!ok);
            return email;
        }
        public static string ValidarNuevoPass()
        {
            bool ok = false;
            bool numok = false;
            int num;
            string pass = "";
            do
            {
                Console.WriteLine("Ingrese una contraseña. Mínimo 6 caracteres. Al menos 1 número");
                pass = Console.ReadLine();
                for (int i = 0; i < pass.Length; i++)
                {
                    if (int.TryParse(pass.Substring(i, 1), out num))
                        numok = true;
                }
                if (pass.Length > 5 && numok)
                    ok = true;
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Ha ingreado un formato incorrecto.\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }

            } while (!ok);
            return pass;
        }
        public static string ValidarUsuario()
        {
            string email;
            bool ok = false;
            do
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("INGRESO DE USUARIO REGISTRADO");
                Console.ResetColor();
                Console.WriteLine("\nIngrese su email \n(Para volver al Menu Principal ingrese '0')");
                email = Console.ReadLine();
                if (email == "0")
                {
                    email = "";
                    ok = true;
                }
                else
                {
                    foreach (Users usuario in ListaUsuarios)
                    {
                        if (usuario.Email == email)
                        {
                            ok = true;
                        }
                    }
                    if (!ok)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("El email ingresado no se encuentra en la Base de Datos.\n");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            } while (!ok);
            return email;
        }
        public static bool ValidarPass(string correo)
        {
            string email = correo;
            string pass;
            int pos = 0;
            bool ok = false;
            foreach (Users usuario in ListaUsuarios)
            {
                if (usuario.Email == email)
                {
                    pos = ListaUsuarios.IndexOf(usuario);
                }
            }
            do
            {
                Console.WriteLine("\nIngrese su contraseña");
                pass = Console.ReadLine();
                if (ListaUsuarios.ElementAt(pos).Pass == pass)
                    ok = true;

                if (!ok)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("La contraseña no coincide con la BBDD");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            } while (!ok);
            return true;
        }
        public static int GetAge(string correo)
        {
            ListaUsuarios = nuevoUsuario.ListarUsuarios();
            string email = correo;
            int edad = 0;
            int pos = 0;
            foreach (Users usuario in ListaUsuarios)
            {
                if (usuario.Email == email)
                {
                    pos = ListaUsuarios.IndexOf(usuario);
                }
            }
            DateTime fecha = Convert.ToDateTime(ListaUsuarios.ElementAt(pos).BirthDate);
            int now = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            int fnac = int.Parse(fecha.ToString("yyyyMMdd"));
            edad = (now - fnac) / 10000;
            return edad;
        }
        public static int GetID(string correo)
        {
            int UserID = 0;
            ListaUsuarios = nuevoUsuario.ListarUsuarios();
            foreach (Users users in ListaUsuarios)
            {
                if (users.Email == correo)
                    UserID = users.User_ID;
            }
            return UserID;
        }
        public static void MostrarListaPeliculas(string correo)
        {
            int edad = GetAge(correo);
            int count = 1;
            ListaFiltrada = nuevaPelicula.ListarFiltrado(edad);
            foreach (Movies movies in ListaFiltrada)
            {
                Console.WriteLine($"({count})- Título: {movies.Title} - Estado: {movies.Status}");
                count++;
            }
        }
        public static void MostrarListaPeliculasDisp(string correo)
        {
            int edad = GetAge(correo);
            int count = 1;
            ListaFiltrada = nuevaPelicula.ListarFiltrado(edad);
            foreach (Movies movies in ListaFiltrada)
            {
                if (movies.Status == "Disponible")
                    Console.WriteLine($"({count})- Título: {movies.Title} - Estado: {movies.Status}");
                count++;
            }
        }
        public static void RegistrarReserva(int elegida, int usuario)
        {
            int ID_movie;
            ID_movie = ListaFiltrada.ElementAt(Convert.ToInt32(elegida) - 1).Movie_ID;
            nuevaPelicula.Reservar(ID_movie);
            nuevoAlquiler.Reservar(ID_movie, usuario);
        }
        public static void DevolverPelicula(int ID_reserva, int ID_Usuario)
        {
            int ID_movie = 0;
            foreach (Rental rental in ListaAlquiler)
            {
                if (rental.Rental_ID == ID_reserva)
                    ID_movie = rental.Movie_ID;
            }
            nuevaPelicula.Devolver(ID_movie);
            nuevoAlquiler.Devolver(ID_reserva);
        }
        public static void MostrarListaAlquiler()
        {
            foreach (Rental rental in ListaAlquiler)
            {
                string title = nuevaPelicula.GetName(rental.Movie_ID);
                if (Convert.ToDateTime(rental.Date_taken) < DateTime.Now.AddDays(-3))
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"ID {rental.Rental_ID}-{title} - Fecha de alquiler: {Convert.ToDateTime(rental.Date_taken).ToShortDateString()}  -  Fecha máxima de devolución: {Convert.ToDateTime(rental.Date_taken).AddDays(3).ToShortDateString()}");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                    Console.WriteLine($"ID {rental.Rental_ID}-{title} - Fecha de alquiler: {Convert.ToDateTime(rental.Date_taken).ToShortDateString()}  -  Fecha máxima de devolución: {Convert.ToDateTime(rental.Date_taken).AddDays(3).ToShortDateString()}");
            }
        }
        public static void MenuUsuario(string correo)
        {
            string inputMenu = "";
            int ID_User = GetID(correo);
            do
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("   MENU DE USUARIO: " +
                    "\n\t1. Ver películas para mi" +
                    "\n\t2. Alquilar una película" +
                    "\n\t3. Ver mis películas alquiladas" +
                    "\n\t0. Salir/Log-out");
                Console.ForegroundColor = ConsoleColor.White;
                inputMenu = Console.ReadLine();
                switch (inputMenu)
                {
                    case "1":
                        string inputMovie = "";
                        Console.Clear();
                        Console.WriteLine("LISTADO DE PELÍCULAS PARA TI\n");
                        MostrarListaPeliculas(correo);
                        do
                        {
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine("\nPara ver la sinopsis de una película ingrese su número. \nPara voler al Menu de Usuario, ingrese '0'");
                            Console.ResetColor();
                            inputMovie = Console.ReadLine();
                            if (inputMovie == "0")
                                Console.Clear();
                            else
                            {
                                Console.WriteLine($"{ListaFiltrada.ElementAt(Convert.ToInt32(inputMovie) - 1).Movie_ID} - " +
                                    $"{ListaFiltrada.ElementAt(Convert.ToInt32(inputMovie) - 1).Title}" +
                                    $"\n{ListaFiltrada.ElementAt(Convert.ToInt32(inputMovie) - 1).Sinopsis}");
                            }
                        } while (inputMovie != "0");
                        break;
                    case "2":
                        Console.Clear();
                        Console.WriteLine("LISTADO DE PELÍCULAS DISPONIBLES PARA TI\n");
                        MostrarListaPeliculasDisp(correo);
                        string chosen = "";
                        bool reservar = false;
                        do
                        {
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine("\nIngrese la película que desea alquilar. \nPara voler al Menu de Usuario, ingrese '0'");
                            Console.ResetColor();
                            chosen = Console.ReadLine();
                            if (chosen == "0") { }
                            else
                            {
                                try
                                {
                                    int aux = ListaFiltrada.ElementAt(Convert.ToInt32(chosen) - 1).Movie_ID;
                                    foreach (Movies movie in ListaFiltrada)
                                    {
                                        if (aux == movie.Movie_ID)
                                            if (movie.Status == "Disponible")
                                                reservar = true;
                                    }
                                }
                                catch
                                {
                                    Console.Beep(1400, 300);
                                    Console.ForegroundColor = ConsoleColor.DarkRed;
                                    Console.WriteLine("Ha ingresado un valor incorrecto. Presione Enter para continuar");
                                    Console.ResetColor();
                                }
                            }
                            if (reservar)
                                RegistrarReserva(Convert.ToInt32(chosen), ID_User);

                            Console.ReadKey();
                            Console.Clear();
                        } while (!reservar && chosen != "0");
                        break;
                    case "3":
                        string inputDevol = "";
                        do
                        {
                            ListaAlquiler = nuevoAlquiler.ListarAlquileres(ID_User);
                            Console.Clear();
                            Console.WriteLine("LISTADO DE TUS PELÍCULAS ALQUILADAS\n");
                            MostrarListaAlquiler();
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine("\nPara realizar una devolución, ingrese el número de ID. \nPara volver al Menu de Usuario ingrese '0'");
                            Console.ResetColor();
                            int ID_Devol;
                            inputDevol = Console.ReadLine();

                            if (inputDevol == "0") { }
                            else if (inputDevol != "0")
                            {
                                ID_Devol = Convert.ToInt32(inputDevol);
                                DevolverPelicula(ID_Devol, ID_User);
                            }
                            else
                            {
                                Console.Beep(1400, 300);
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.WriteLine("Ha ingresado un valor incorrecto. Presione Enter para continuar");
                                Console.ResetColor();
                            }
                            Console.ReadKey();
                            Console.Clear();
                        } while (inputDevol != "0");
                        break;
                    case "0":
                        Console.Clear();
                        break;
                    default:
                        Console.Beep(1400, 300);
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Ha ingresado una opción incorrecta. Presione Enter para continuar");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
            } while (inputMenu != "0");
        }
        public static string ValidarAdminID()
        {
            string AdminID;
            bool ok = false;
            do
            {
                Console.WriteLine("\nIngrese su ID de Administrador");
                AdminID = Console.ReadLine();
                foreach (Admins admin in ListaAdministradores)
                {
                    if (admin.Admin_ID == AdminID)
                    {
                        ok = true;
                    }
                }
                if (!ok)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("El ID ingresado es incorrecto");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            } while (!ok);
            return AdminID;
        }
        public static bool ValidarAdminPass(string AdminID)
        {
            string Admin_ID = AdminID;
            string pass;
            int pos = 0;
            bool ok = false;
            foreach (Admins admin in ListaAdministradores)
            {
                if (admin.Admin_ID == Admin_ID)
                {
                    pos = ListaAdministradores.IndexOf(admin);
                }
            }
            do
            {
                Console.WriteLine("\nIngrese su contraseña");
                pass = Console.ReadLine();
                if (ListaAdministradores.ElementAt(pos).Pass == pass)
                    ok = true;

                if (!ok)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("La contraseña es incorrecta");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            } while (!ok);
            return true;
        }
        public static int GetLevel(string idAdmin)
        {
            int IDlevel = 9;
            foreach (Admins admin in ListaAdministradores)
            {
                if (admin.Admin_ID == idAdmin)
                    IDlevel = admin.Level;
            }
            return IDlevel;
        }
        public static void MenuAdministrador(string idAdmin)
        {
            string inputMenu = "";
            int nivel = GetLevel(idAdmin);

            do
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("          <<< MENU DE ADMINISTRADOR >>>           ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine("\t1.  Crear un nuevo Administrador" +
                    "\n\t2.  Eliminar un usuario de la base de datos" +
                    "\n\t3.  Agregar una película" +
                    "\n\t4.  Eliminar una película" +
                    "\n\t0.  Volver al menu principal");
                inputMenu = Console.ReadLine();

                switch (inputMenu)
                {
                    case "1":
                        if (nivel == 0)
                        {
                            Console.WriteLine("Ingrese un alias para usar como ID");
                            string IDadmin = Console.ReadLine();
                            string Pass = ValidarNuevoPass();
                            Console.WriteLine("Ingrese el nombre");
                            string nombre = Console.ReadLine();
                            Console.WriteLine("Ingrese el apellido");
                            string apellido = Console.ReadLine();
                            int nivelAsignado = 9;
                            do
                            {
                                Console.WriteLine("Ingrese el nivel que se le asignará. Solo puede ser nivel 1 o nivel 2");
                                string inputNivel = Console.ReadLine();
                                if (inputNivel == "1")
                                    nivelAsignado = Convert.ToInt32(inputNivel);
                                else if (inputNivel == "2")
                                    nivelAsignado = Convert.ToInt32(inputNivel);
                                else
                                    Console.WriteLine("El valor ingresado no es correcto");
                            } while (nivelAsignado != 1 && nivelAsignado != 2);

                            nuevoAdmin.GenerrNuevoAdmin(IDadmin, Pass, nombre, apellido, nivelAsignado);
                        }
                        else
                            Console.WriteLine("Usted no tiene permiso para realizar esta acción. Contacte a un Administrador nivel 0");
                        break;
                    case "2":
                        if (nivel == 0 || nivel == 1)
                        {
                            Console.WriteLine("Ingrese el e-mail del usuario que desea eliminar");
                            string EraseEmail = Console.ReadLine();
                            nuevoUsuario.MostrarUsuarioEmail(EraseEmail);
                            int EraseUserID = GetID(EraseEmail);
                            Console.WriteLine("Para confirmar la eliminación del usuario, ingrese 'ok'");
                            string confirma = Console.ReadLine();
                            if (confirma.ToLower() == "ok")
                                if (!nuevoAlquiler.ComprobarRentalUser(EraseUserID))
                                    nuevoUsuario.EliminarUsuario(EraseUserID);
                                else
                                {
                                    Console.Beep(1400, 400);
                                    Console.ForegroundColor = ConsoleColor.DarkRed;
                                    Console.WriteLine("El usuario tiene películas pendientes de devolución. No se puede eliminar");
                                    Console.ResetColor();
                                    Console.ReadKey();
                                }
                            else
                                Console.WriteLine("No se ha modificado el usuario");
                        }
                        else
                            Console.WriteLine("Usted no tiene permiso para realizar esta acción. Contacte a un Administrador nivel 0 o 1");
                        break;
                    case "3":
                        if (nivel >= 0 && nivel < 3)
                            nuevaPelicula.AddMovie();
                        break;
                    case "4":
                        if (nivel == 0 || nivel == 1)
                        {
                            ListaCompleta = nuevaPelicula.ListarTodo();
                            bool ok = false;
                            foreach (Movies movies in ListaCompleta)
                            {
                                Console.WriteLine($"ID {movies.Movie_ID} - Título {movies.Title}  -  Estado: {movies.Status}");
                            }
                            Console.WriteLine("\nIndique el ID de la película que desea eliminar. No se podrá eliminar si el estado es 'Alquilada'");
                            int input = Convert.ToInt32(Console.ReadLine());
                            foreach (Movies movie in ListaCompleta)
                            {
                                if (movie.Movie_ID == input)
                                    if (movie.Status == "Disponible")
                                        ok = true;
                            }
                            if (!ok)
                            {
                                Console.Beep(1500, 400);
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.WriteLine("No se puede eliminar la película seleccionada");
                                Console.ResetColor();
                                Console.ReadKey();
                            }
                            else
                                nuevaPelicula.EraseMovie(input);
                        }
                        break;
                    case "0":
                        Console.Clear();
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Ha ingresado una opción incorrecta!");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
            } while (inputMenu != "0");
        }
    }
}
