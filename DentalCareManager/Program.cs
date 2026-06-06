using System;
using System.IO;

namespace DentalCareManager
{
    // 1. Definición de Estructuras
    struct Paciente
    {
        public int IdPaciente;          // Identificador único del paciente
        public string NombreCompleto;   // Nombre y apellido del paciente
        public string Telefono;         // Número de contacto principal
        public int Edad;                // Edad del paciente 
        public string Correo;           // Correo electrónico 
    }

    struct Cita
    {
        public int IdCita;              // Identificador único de la cita
        public Paciente DatosPaciente;  // Llamada a la estructura del paciente
        public string FechaHora;        // Fecha y hora programada
        public string MotivoConsulta;   // Razón de la visita
        public bool Activa;             // Estado de la cita 
    }

    class Program
    {
        // 2. Variables Globales y Arreglos
        static Cita[] agendaCitas = new Cita[100];
        static int totalCitas = 0; // Rastrea la cantidad de registros actuales
        const string archivoDB = "citas.txt";

        static void Main(string[] args)
        {
            // Cargar datos al arrancar el programa
            CargarArchivo();

            bool salir = false;

            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("======================================================");
                Console.WriteLine("   DENTAL CARE MANAGER - Consultorio Dr.    ");
                Console.WriteLine("======================================================");
                Console.WriteLine("1. Registrar nueva cita");
                Console.WriteLine("2. Consultar agenda completa");
                Console.WriteLine("3. Buscar cita por paciente");
                Console.WriteLine("4. Modificar datos de una cita");
                Console.WriteLine("5. Cancelar (Eliminar) cita");
                Console.WriteLine("6. Guardar y actualizar archivo");
                Console.WriteLine("7. Salir del sistema");
                Console.WriteLine("======================================================");
                Console.Write("Seleccione una opción: ");

                string opcion = Console.ReadLine() ?? "";

                switch (opcion)
                {
                    case "1":
                        RegistrarCita();
                        break;
                    case "2":
                        MostrarAgenda();
                        break;
                    case "3":
                        BuscarCita();
                        break;
                    case "4":
                        ModificarCita();
                        break;
                    case "5":
                        CancelarCita();
                        break;
                    case "6":
                        GuardarArchivo();
                        Console.WriteLine("\nArchivo guardado con éxito. Presione ENTER para continuar.");
                        Console.ReadLine();
                        break;
                    case "7":
                        GuardarArchivo(); // Autoguardado al salir
                        salir = true;
                        Console.WriteLine("\nSaliendo del sistema... ¡Hasta pronto!");
                        break;
                    default:
                        Console.WriteLine("\nOpción no válida. Presione ENTER para intentar de nuevo.");
                        Console.ReadLine();
                        break;
                }
            }
        }

        // --- MÉTODOS DE LA APLICACIÓN ---

        static void RegistrarCita()
        {
            Console.Clear();
            Console.WriteLine("--- REGISTRAR NUEVA CITA ---");

            if (totalCitas >= 100)
            {
                Console.WriteLine("La agenda está llena (Límite de 100 citas alcanzado).");
                Console.ReadLine();
                return;
            }

            Cita nuevaCita = new Cita();
            nuevaCita.IdCita = totalCitas + 1;

            // Datos del Paciente
            nuevaCita.DatosPaciente = new Paciente();
            nuevaCita.DatosPaciente.IdPaciente = totalCitas + 1; // Para simplificar, usamos el mismo ID

            Console.Write("Nombre completo del paciente: ");
            nuevaCita.DatosPaciente.NombreCompleto = Console.ReadLine() ?? "Desconocido";

            Console.Write("Teléfono: ");
            nuevaCita.DatosPaciente.Telefono = Console.ReadLine() ?? "N/A";

            // Validación de número entero con try-catch
            try
            {
                Console.Write("Edad: ");
                nuevaCita.DatosPaciente.Edad = int.Parse(Console.ReadLine() ?? "0");
            }
            catch (FormatException)
            {
                Console.WriteLine("Formato de edad incorrecto. Se asignará 0 por defecto.");
                nuevaCita.DatosPaciente.Edad = 0;
            }

            Console.Write("Correo electrónico: ");
            nuevaCita.DatosPaciente.Correo = Console.ReadLine() ?? "N/A";

            // Datos de la Cita con validación de empalme
            bool horarioValido = false;
            while (!horarioValido)
            {
                Console.Write("Fecha y Hora (Ej. 15/10/2026 14:00): ");
                string fechaHora = Console.ReadLine() ?? "";

                if (ValidarHorario(fechaHora))
                {
                    nuevaCita.FechaHora = fechaHora;
                    horarioValido = true;
                }
                else
                {
                    Console.WriteLine("ERROR: Ya existe una cita activa en ese horario. Intente con otra hora.");
                }
            }

            Console.Write("Motivo de la consulta (Ej. Limpieza, Extracción): ");
            nuevaCita.MotivoConsulta = Console.ReadLine() ?? "Consulta General";

            nuevaCita.Activa = true;

            // Guardar en el arreglo
            agendaCitas[totalCitas] = nuevaCita;
            totalCitas++;

            Console.WriteLine("\n¡Cita registrada con éxito!");
            Console.ReadLine();
        }

        static bool ValidarHorario(string fechaHora)
        {
            // Recorre el arreglo para evitar duplicidad de horarios activos
            for (int i = 0; i < totalCitas; i++)
            {
                if (agendaCitas[i].Activa && agendaCitas[i].FechaHora == fechaHora)
                {
                    return false; // El horario ya está ocupado
                }
            }
            return true; // El horario está libre
        }

        static void MostrarAgenda()
        {
            Console.Clear();
            Console.WriteLine("--- AGENDA COMPLETA DE CITAS ---");

            int citasActivas = 0;

            for (int i = 0; i < totalCitas; i++)
            {
                if (agendaCitas[i].Activa)
                {
                    ImprimirDetalleCita(agendaCitas[i]);
                    citasActivas++;
                }
            }

            if (citasActivas == 0)
            {
                Console.WriteLine("No hay citas activas registradas.");
            }
            else
            {
                Console.WriteLine($"\nTotal de citas activas: {citasActivas}");
            }

            Console.WriteLine("\nPresione ENTER para regresar al menú.");
            Console.ReadLine();
        }

        static void BuscarCita()
        {
            Console.Clear();
            Console.WriteLine("--- BUSCAR CITA POR PACIENTE ---");
            Console.Write("Ingrese el nombre del paciente a buscar: ");
            string busqueda = (Console.ReadLine() ?? "").ToLower();

            bool encontrada = false;

            for (int i = 0; i < totalCitas; i++)
            {
                if (agendaCitas[i].Activa && agendaCitas[i].DatosPaciente.NombreCompleto.ToLower().Contains(busqueda))
                {
                    ImprimirDetalleCita(agendaCitas[i]);
                    encontrada = true;
                }
            }

            if (!encontrada)
            {
                Console.WriteLine("\nNo se encontraron citas activas para ese paciente.");
            }

            Console.ReadLine();
        }

        static void ModificarCita()
        {
            Console.Clear();
            Console.WriteLine("--- MODIFICAR DATOS DE CITA ---");
            Console.Write("Ingrese el ID de la cita a modificar: ");

            if (int.TryParse(Console.ReadLine(), out int idBuscado))
            {
                bool encontrada = false;
                for (int i = 0; i < totalCitas; i++)
                {
                    if (agendaCitas[i].IdCita == idBuscado && agendaCitas[i].Activa)
                    {
                        encontrada = true;
                        Console.WriteLine($"Modificando cita de: {agendaCitas[i].DatosPaciente.NombreCompleto}");
                        Console.WriteLine("1. Modificar Fecha/Hora");
                        Console.WriteLine("2. Modificar Motivo de consulta");
                        Console.Write("Seleccione una opción: ");
                        string opc = Console.ReadLine() ?? "";

                        if (opc == "1")
                        {
                            Console.Write("Nueva Fecha y Hora (Ej. 15/10/2026 14:00): ");
                            string nuevaFecha = Console.ReadLine() ?? "";

                            if (ValidarHorario(nuevaFecha))
                            {
                                agendaCitas[i].FechaHora = nuevaFecha;
                                Console.WriteLine("Fecha modificada con éxito.");
                            }
                            else
                            {
                                Console.WriteLine("El horario ingresado ya está ocupado.");
                            }
                        }
                        else if (opc == "2")
                        {
                            Console.Write("Nuevo motivo de consulta: ");
                            agendaCitas[i].MotivoConsulta = Console.ReadLine() ?? agendaCitas[i].MotivoConsulta;
                            Console.WriteLine("Motivo modificado con éxito.");
                        }
                        break;
                    }
                }

                if (!encontrada) Console.WriteLine("Cita no encontrada o ya está cancelada.");
            }
            else
            {
                Console.WriteLine("ID inválido.");
            }

            Console.ReadLine();
        }

        static void CancelarCita()
        {
            Console.Clear();
            Console.WriteLine("--- CANCELAR CITA ---");
            Console.Write("Ingrese el ID de la cita a cancelar: ");

            if (int.TryParse(Console.ReadLine(), out int idBuscado))
            {
                bool encontrada = false;
                for (int i = 0; i < totalCitas; i++)
                {
                    if (agendaCitas[i].IdCita == idBuscado && agendaCitas[i].Activa)
                    {
                        agendaCitas[i].Activa = false; // Borrado lógico
                        Console.WriteLine($"La cita de {agendaCitas[i].DatosPaciente.NombreCompleto} ha sido cancelada y el espacio liberado.");
                        encontrada = true;
                        break;
                    }
                }
                if (!encontrada) Console.WriteLine("Cita no encontrada o ya estaba cancelada.");
            }
            else
            {
                Console.WriteLine("ID inválido.");
            }

            Console.ReadLine();
        }

        // --- PERSISTENCIA DE DATOS ---

        static void GuardarArchivo()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(archivoDB))
                {
                    for (int i = 0; i < totalCitas; i++)
                    {
                        Cita c = agendaCitas[i];
                        // Separamos los datos con el carácter '|'
                        string linea = $"{c.IdCita}|{c.DatosPaciente.IdPaciente}|{c.DatosPaciente.NombreCompleto}|{c.DatosPaciente.Telefono}|{c.DatosPaciente.Edad}|{c.DatosPaciente.Correo}|{c.FechaHora}|{c.MotivoConsulta}|{c.Activa}";
                        sw.WriteLine(linea);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar el archivo: {ex.Message}");
            }
        }

        static void CargarArchivo()
        {
            if (File.Exists(archivoDB))
            {
                try
                {
                    string[] lineas = File.ReadAllLines(archivoDB);
                    foreach (string linea in lineas)
                    {
                        if (!string.IsNullOrWhiteSpace(linea))
                        {
                            string[] datos = linea.Split('|');
                            if (datos.Length == 9)
                            {
                                Cita c = new Cita();
                                c.IdCita = int.Parse(datos[0]);

                                c.DatosPaciente = new Paciente();
                                c.DatosPaciente.IdPaciente = int.Parse(datos[1]);
                                c.DatosPaciente.NombreCompleto = datos[2];
                                c.DatosPaciente.Telefono = datos[3];
                                c.DatosPaciente.Edad = int.Parse(datos[4]);
                                c.DatosPaciente.Correo = datos[5];

                                c.FechaHora = datos[6];
                                c.MotivoConsulta = datos[7];
                                c.Activa = bool.Parse(datos[8]);

                                agendaCitas[totalCitas] = c;
                                totalCitas++;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Advertencia: Hubo un problema al cargar algunas citas anteriores.");
                }
            }
        }

        // --- MÉTODO AUXILIAR ---

        static void ImprimirDetalleCita(Cita c)
        {
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"[ID Cita: {c.IdCita}] - Fecha y Hora: {c.FechaHora}");
            Console.WriteLine($"Paciente: {c.DatosPaciente.NombreCompleto} | Edad: {c.DatosPaciente.Edad} | Tel: {c.DatosPaciente.Telefono}");
            Console.WriteLine($"Motivo: {c.MotivoConsulta}");
        }
    }
}