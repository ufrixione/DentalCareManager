using System;
using Microsoft.Data.SqlClient;

namespace DentalCareManager
{
    struct Paciente
    {
        public int IdPaciente;
        public string Nombre;
        public string Apellido;
        public string Cedula;
        public string Telefono;
    }

    struct Cita
    {
        public int IdCita;
        public Paciente DatosPaciente;
        public string FechaHora;
        public string MotivoConsulta;
        public string Estado;
    }

    class Program
    {
        static Cita[] agendaCitas = new Cita[100];
        static int totalCitas = 0;

        // CADENA DE CONEXIÓN (Ajusta el Server si utilizas SQLEXPRESS u otra instancia)
        const string connectionString = "Server=localhost;Database=ConsultorioDental;Trusted_Connection=True;TrustServerCertificate=True;";

        static void Main(string[] args)
        {
            CargarDesdeBaseDatos();

            bool salir = false;
            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("======================================================");
                Console.WriteLine("   DENTAL CARE MANAGER - Consultorio SQL Server     ");
                Console.WriteLine("======================================================");
                Console.WriteLine("1. Registrar nueva cita");
                Console.WriteLine("2. Consultar agenda completa");
                Console.WriteLine("3. Buscar cita por paciente");
                Console.WriteLine("4. Modificar datos de una cita");
                Console.WriteLine("5. Cancelar (Eliminar) cita");
                Console.WriteLine("6. Sincronizar / Actualizar datos de la BD");
                Console.WriteLine("7. Salir del sistema");
                Console.WriteLine("======================================================");
                Console.Write("Seleccione una opción: ");

                string opcion = Console.ReadLine() ?? "";

                switch (opcion)
                {
                    case "1": RegistrarCita(); break;
                    case "2": MostrarAgenda(); break;
                    case "3": BuscarCita(); break;
                    case "4": ModificarCita(); break;
                    case "5": CancelarCita(); break;
                    case "6":
                        CargarDesdeBaseDatos();
                        Console.WriteLine("\nDatos sincronizados. Presione ENTER.");
                        Console.ReadLine();
                        break;
                    case "7":
                        salir = true;
                        Console.WriteLine("\nSaliendo... ¡Hasta pronto!");
                        break;
                    default:
                        Console.WriteLine("\nOpción inválida. Presione ENTER.");
                        Console.ReadLine();
                        break;
                }
            }
        }

        static void RegistrarCita()
        {
            Console.Clear();
            Console.WriteLine("--- REGISTRAR NUEVA CITA ---");

            if (totalCitas >= 100)
            {
                Console.WriteLine("La agenda interna está llena.");
                Console.ReadLine();
                return;
            }

            Paciente nuevoPaciente = new Paciente();

            // ==========================================
            // 1. CAPTURA Y VALIDACIÓN DEL NOMBRE
            // ==========================================
            while (true)
            {
                try
                {
                    Console.Write("Nombre del paciente: ");
                    string nombreInput = Console.ReadLine()!;

                    if (string.IsNullOrWhiteSpace(nombreInput))
                    {
                        throw new Exception("El nombre no puede estar vacío.");
                    }

                    // Revisamos carácter por carácter que no existan números
                    foreach (char c in nombreInput)
                    {
                        if (char.IsDigit(c))
                        {
                            throw new Exception("El nombre no puede contener números.");
                        }
                    }

                    nuevoPaciente.Nombre = nombreInput;
                    break; // Si todo está bien, rompe el bucle y pasa al apellido
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Error en Nombre]: {ex.Message} Inténtalo de nuevo.\n");
                }
            }

            // ==========================================
            // 2. CAPTURA Y VALIDACIÓN DEL APELLIDO
            // ==========================================
            while (true)
            {
                try
                {
                    Console.Write("Apellido del paciente: ");
                    string apellidoInput = Console.ReadLine()!;

                    if (string.IsNullOrWhiteSpace(apellidoInput))
                    {
                        throw new Exception("El apellido no puede estar vacío.");
                    }

                    foreach (char c in apellidoInput)
                    {
                        if (char.IsDigit(c))
                        {
                            throw new Exception("El apellido no puede contener números.");
                        }
                    }

                    nuevoPaciente.Apellido = apellidoInput;
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Error en Apellido]: {ex.Message} Inténtalo de nuevo.\n");
                }
            }

            // ==========================================
            // 3. CAPTURA DE CÉDULA (Se mantiene libre)
            // ==========================================
            Console.Write("Cédula: ");
            nuevoPaciente.Cedula = Console.ReadLine() ?? "N/A";

            // ==========================================
            // 4. CAPTURA Y VALIDACIÓN DEL TELÉFONO
            // ==========================================
            while (true)
            {
                try
                {
                    Console.Write("Teléfono: ");
                    string telefonoInput = Console.ReadLine()!;

                    if (string.IsNullOrWhiteSpace(telefonoInput))
                    {
                        throw new Exception("El teléfono no puede estar vacío.");
                    }

                    // Revisamos que solo contenga números (evitamos letras como la 'g')
                    foreach (char c in telefonoInput)
                    {
                        if (char.IsLetter(c))
                        {
                            throw new Exception("El teléfono solo debe contener números.");
                        }
                    }

                    nuevoPaciente.Telefono = telefonoInput;
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Error en Teléfono]: {ex.Message} Inténtalo de nuevo.\n");
                }
            }

            // ==========================================
            // CONTINUACIÓN DE TU LÓGICA DE HORARIOS
            // ==========================================
            string fechaInput = "";
            string horaInput = "";
            bool horarioValido = false;
            DateTime fechaFinal = DateTime.Now;
            TimeSpan horaFinal = TimeSpan.Zero;

            // Aquí continúa el resto del código que ya tenías para capturar la fecha y hora...
            while (!horarioValido)
            {
                Console.Write("Fecha de la cita (Ej. 2026-10-15): ");
                fechaInput = Console.ReadLine() ?? "";

                Console.Write("Hora de la cita (Ej. 14:30): ");
                horaInput = Console.ReadLine() ?? "";

                if (DateTime.TryParse(fechaInput, out fechaFinal) && TimeSpan.TryParse(horaInput, out horaFinal))
                {
                    if (ValidarHorarioExacto(fechaFinal.Date, horaFinal))
                    {
                        horarioValido = true;
                    }
                    else
                    {
                        Console.WriteLine("ERROR: Ya existe una cita activa en esa misma fecha y hora. Intente otra.");
                    }
                }
                else
                {
                    Console.WriteLine("ERROR: Formato de fecha u hora incorrecto. Use AAAA-MM-DD y HH:MM.");
                }
            }

            try
            {
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    conexion.Open();

                    // Mapeo exacto a las columnas reales de tu tabla Pacientes
                    string queryPaciente = @"INSERT INTO Pacientes (Cedula, Nombre, Apellido, Telefono, FechaRegistro, Activo) 
                                             OUTPUT INSERTED.IdPaciente 
                                             VALUES (@Cedula, @Nombre, @Apellido, @Tel, @FechaReg, 1);";

                    SqlCommand cmdP = new SqlCommand(queryPaciente, conexion);
                    cmdP.Parameters.AddWithValue("@Cedula", nuevoPaciente.Cedula);
                    cmdP.Parameters.AddWithValue("@Nombre", nuevoPaciente.Nombre);
                    cmdP.Parameters.AddWithValue("@Apellido", nuevoPaciente.Apellido);
                    cmdP.Parameters.AddWithValue("@Tel", nuevoPaciente.Telefono);
                    cmdP.Parameters.AddWithValue("@FechaReg", DateTime.Now);

                    int idPacienteGenerado = (int)cmdP.ExecuteScalar();

                    // Mapeo exacto a las columnas reales de tu tabla Citas
                    string queryCita = @"INSERT INTO Citas (IdPaciente, Fecha, Hora, Estado, IdDentista) 
                                         VALUES (@IdPaciente, @Fecha, @Hora, 'Activa', 1);";

                    SqlCommand cmdC = new SqlCommand(queryCita, conexion);
                    cmdC.Parameters.AddWithValue("@IdPaciente", idPacienteGenerado);
                    cmdC.Parameters.AddWithValue("@Fecha", fechaFinal.Date);
                    cmdC.Parameters.AddWithValue("@Hora", horaFinal);

                    cmdC.ExecuteNonQuery();
                }

                Console.WriteLine("\n¡Cita y Paciente registrados con éxito en SQL Server!");
                CargarDesdeBaseDatos();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[ERROR AL GUARDAR]: {ex.Message}");
            }

            Console.ReadLine();
        }

        static bool ValidarHorarioExacto(DateTime fecha, TimeSpan hora)
        {
            string query = "SELECT COUNT(*) FROM Citas WHERE Fecha = @Fecha AND Hora = @Hora AND Estado = 'Activa'";
            try
            {
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@Fecha", fecha);
                    cmd.Parameters.AddWithValue("@Hora", hora);

                    return (int)cmd.ExecuteScalar() == 0;
                }
            }
            catch { return false; }
        }

        static void MostrarAgenda()
        {
            Console.Clear();
            Console.WriteLine("--- AGENDA COMPLETA DE CITAS ---");

            int citasActivas = 0;
            for (int i = 0; i < totalCitas; i++)
            {
                if (agendaCitas[i].Estado == "Activa")
                {
                    ImprimirDetalleCita(agendaCitas[i]);
                    citasActivas++;
                }
            }

            if (citasActivas == 0) Console.WriteLine("No hay citas activas.");
            else Console.WriteLine($"\nTotal de citas activas: {citasActivas}");

            Console.WriteLine("\nPresione ENTER para regresar.");
            Console.ReadLine();
        }

        static void BuscarCita()
        {
            Console.Clear();
            Console.WriteLine("--- BUSCAR CITA POR PACIENTE ---");
            Console.Write("Ingrese el nombre del paciente: ");
            string busqueda = (Console.ReadLine() ?? "").ToLower();

            bool encontrada = false;
            for (int i = 0; i < totalCitas; i++)
            {
                if (agendaCitas[i].Estado == "Activa" && agendaCitas[i].DatosPaciente.Nombre.ToLower().Contains(busqueda))
                {
                    ImprimirDetalleCita(agendaCitas[i]);
                    encontrada = true;
                }
            }
            if (!encontrada) Console.WriteLine("\nNo se encontraron citas.");
            Console.ReadLine();
        }

        static void ModificarCita()
        {
            Console.Clear();
            Console.WriteLine("--- MODIFICAR DATOS DE CITA ---");
            Console.Write("Ingrese el ID de la cita: ");

            if (int.TryParse(Console.ReadLine(), out int idBuscado))
            {
                bool encontrada = false;
                for (int i = 0; i < totalCitas; i++)
                {
                    if (agendaCitas[i].IdCita == idBuscado && agendaCitas[i].Estado == "Activa")
                    {
                        encontrada = true;
                        Console.WriteLine("1. Modificar Fecha/Hora");
                        Console.Write("Seleccione una opción: ");
                        string opc = Console.ReadLine() ?? "";

                        if (opc == "1")
                        {
                            Console.Write("Nueva Fecha (Ej. 2026-10-15): ");
                            string nuevaFechaInput = Console.ReadLine() ?? "";

                            Console.Write("Nueva Hora (Ej. 14:30): ");
                            string nuevaHoraInput = Console.ReadLine() ?? "";

                            if (DateTime.TryParse(nuevaFechaInput, out DateTime nFecha) && TimeSpan.TryParse(nuevaHoraInput, out TimeSpan nHora))
                            {
                                if (ValidarHorarioExacto(nFecha.Date, nHora))
                                {
                                    try
                                    {
                                        using (SqlConnection conexion = new SqlConnection(connectionString))
                                        {
                                            conexion.Open();
                                            string query = "UPDATE Citas SET Fecha = @Fecha, Hora = @Hora WHERE IdCita = @Id";
                                            SqlCommand cmd = new SqlCommand(query, conexion);
                                            cmd.Parameters.AddWithValue("@Fecha", nFecha.Date);
                                            cmd.Parameters.AddWithValue("@Hora", nHora);
                                            cmd.Parameters.AddWithValue("@Id", idBuscado);
                                            cmd.ExecuteNonQuery();
                                        }
                                        Console.WriteLine("Fecha y hora modificadas con éxito.");
                                        CargarDesdeBaseDatos();
                                    }
                                    catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
                                }
                                else Console.WriteLine("Horario ocupado en el sistema.");
                            }
                            else Console.WriteLine("Formatos incorrectos.");
                        }
                        break;
                    }
                }
                if (!encontrada) Console.WriteLine("Cita no encontrada o inactiva.");
            }
            Console.ReadLine();
        }

        static void CancelarCita()
        {
            Console.Clear();
            Console.WriteLine("--- CANCELAR CITA ---");
            Console.Write("Ingrese el ID de la cita: ");

            if (int.TryParse(Console.ReadLine(), out int idBuscado))
            {
                try
                {
                    using (SqlConnection conexion = new SqlConnection(connectionString))
                    {
                        conexion.Open();
                        string query = "UPDATE Citas SET Estado = 'Cancelada' WHERE IdCita = @Id";
                        SqlCommand cmd = new SqlCommand(query, conexion);
                        cmd.Parameters.AddWithValue("@Id", idBuscado);
                        int filas = cmd.ExecuteNonQuery();

                        if (filas > 0) Console.WriteLine("Cita cancelada correctamente en SQL Server.");
                        else Console.WriteLine("Cita no encontrada.");
                    }
                    CargarDesdeBaseDatos();
                }
                catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
            }
            Console.ReadLine();
        }

        static void CargarDesdeBaseDatos()
        {
            totalCitas = 0;
            string query = @"SELECT C.IdCita, P.IdPaciente, P.Nombre, P.Apellido, P.Telefono, C.Fecha, C.Hora, C.Estado 
                             FROM Citas C 
                             INNER JOIN Pacientes P ON C.IdPaciente = P.IdPaciente
                             WHERE C.Estado = 'Activa'";

            try
            {
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    conexion.Open();
                    SqlCommand comando = new SqlCommand(query, conexion);

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read() && totalCitas < 100)
                        {
                            Cita c = new Cita();
                            c.IdCita = reader.GetInt32(0);

                            c.DatosPaciente = new Paciente();
                            c.DatosPaciente.IdPaciente = reader.GetInt32(1);
                            c.DatosPaciente.Nombre = reader.GetString(2);
                            c.DatosPaciente.Apellido = reader.GetString(3);
                            c.DatosPaciente.Telefono = reader.IsDBNull(4) ? "N/A" : reader.GetString(4);

                            DateTime fecha = reader.GetDateTime(5);
                            TimeSpan hora = reader.GetTimeSpan(6);
                            c.FechaHora = fecha.Add(hora).ToString("yyyy-MM-dd HH:mm");

                            c.Estado = reader.GetString(7);

                            agendaCitas[totalCitas] = c;
                            totalCitas++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[AVISO AL INICIAR]: Error al cargar datos: {ex.Message}");
            }
        }

        static void ImprimirDetalleCita(Cita c)
        {
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"[ID Cita: {c.IdCita}] - Fecha y Hora: {c.FechaHora}");
            Console.WriteLine($"Paciente: {c.DatosPaciente.Nombre} {c.DatosPaciente.Apellido} | Tel: {c.DatosPaciente.Telefono}");
        }
    }
} 