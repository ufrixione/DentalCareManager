using System;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DentalCareManager
{
    // 1. Definición de Estructuras
    struct Paciente
    {
        public int IdPaciente;
        public string NombreCompleto;
        public string Telefono;
        public string FechaNacimiento;
        public string Correo;
    }

    struct Cita
    {
        public int IdCita;
        public Paciente DatosPaciente;
        public string FechaHora;
        public string MotivoConsulta;
        public bool Activa;
    }

    class Program
    {
        static Cita[] agendaCitas = new Cita[100];
        static int totalCitas = 0;
        const string archivoDB = "citas.txt";

        static void Main(string[] args)
        {
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
                        Console.WriteLine("\nArchivo guardado con éxito.");
                        Pausar();
                        break;
                    case "7":
                        GuardarArchivo();
                        salir = true;
                        Console.WriteLine("\nSaliendo del sistema... ¡Hasta pronto!");
                        Pausar();
                        break;
                    default:
                        Console.WriteLine("\nOpción no válida.");
                        Pausar();
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
                Pausar();
                return;
            }

            Paciente nuevoPaciente = new Paciente();
            nuevoPaciente.IdPaciente = totalCitas + 1;

            // 1. VALIDACIÓN DEL NOMBRE
            while (true)
            {
                Console.Write("Nombre completo del paciente: ");
                string entradaNombre = Console.ReadLine() ?? "";
                if (ValidarSoloLetras(entradaNombre))
                {
                    nuevoPaciente.NombreCompleto = entradaNombre;
                    break;
                }
                Console.WriteLine("ERROR: El nombre solo debe contener letras y no puede estar vacío.");
                Pausar();
            }

            // 2. VALIDACIÓN DEL TELÉFONO
            while (true)
            {
                Console.Write("Teléfono (8 dígitos numéricos, ej. 88881234): ");
                string entradaTel = Console.ReadLine() ?? "";
                if (ValidarTelefonoNicaragua(entradaTel))
                {
                    nuevoPaciente.Telefono = entradaTel;
                    break;
                }
                Console.WriteLine("ERROR: Ingrese un número válido de Nicaragua (exactamente 8 números enteros).");
                Pausar();
            }

            // 3. VALIDACIÓN DE FECHA DE NACIMIENTO
            while (true)
            {
                Console.Write("Fecha de nacimiento (Ej. 25/12/1995): ");
                string entradaFechaNac = Console.ReadLine() ?? "";

                if (ValidarFechaNacimiento(entradaFechaNac, out DateTime fechaNac))
                {
                    if (fechaNac > DateTime.Now)
                    {
                        Console.WriteLine("ERROR: La fecha de nacimiento no puede ser una fecha futura.");
                        Pausar();
                        continue;
                    }
                    nuevoPaciente.FechaNacimiento = entradaFechaNac;
                    break;
                }
                Console.WriteLine("ERROR: Formato incorrecto. Use solo números con barra (/) en formato DD/MM/AAAA.");
                Pausar();
            }

            Console.Write("Correo electrónico (opcional): ");
            nuevoPaciente.Correo = Console.ReadLine() ?? "N/A";

            Cita nuevaCita = new Cita();
            nuevaCita.IdCita = totalCitas + 1;
            nuevaCita.DatosPaciente = nuevoPaciente;

            // 4. VALIDACIÓN DE FECHA Y HORA (AHORA MÁS INTUITIVO)
            Console.WriteLine("\n--- DATOS DE LA CITA ---");
            nuevaCita.FechaHora = SolicitarFechaHoraLibre();

            Console.Write("Motivo de la consulta (Ej. Limpieza, Extracción): ");
            nuevaCita.MotivoConsulta = Console.ReadLine() ?? "Consulta General";

            nuevaCita.Activa = true;

            agendaCitas[totalCitas] = nuevaCita;
            totalCitas++;

            Console.WriteLine("\n¡Cita registrada con éxito!");
            Pausar();
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

            Pausar();
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

            Pausar();
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
                        Console.WriteLine($"\nModificando cita de: {agendaCitas[i].DatosPaciente.NombreCompleto}");
                        Console.WriteLine("1. Modificar Fecha y Hora");
                        Console.WriteLine("2. Modificar Motivo de consulta");
                        Console.Write("Seleccione una opción: ");
                        string opc = Console.ReadLine() ?? "";

                        if (opc == "1")
                        {
                            Console.WriteLine("\n--- NUEVA FECHA Y HORA ---");
                            agendaCitas[i].FechaHora = SolicitarFechaHoraLibre();
                            Console.WriteLine("Fecha modificada con éxito.");
                        }
                        else if (opc == "2")
                        {
                            Console.Write("Nuevo motivo de consulta: ");
                            agendaCitas[i].MotivoConsulta = Console.ReadLine() ?? agendaCitas[i].MotivoConsulta;
                            Console.WriteLine("Motivo modificado con éxito.");
                        }
                        else
                        {
                            Console.WriteLine("Opción no válida.");
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

            Pausar();
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
                        agendaCitas[i].Activa = false;
                        Console.WriteLine($"La cita de {agendaCitas[i].DatosPaciente.NombreCompleto} ha sido cancelada.");
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

            Pausar();
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
                        string linea = $"{c.IdCita}|{c.DatosPaciente.IdPaciente}|{c.DatosPaciente.NombreCompleto}|{c.DatosPaciente.Telefono}|{c.DatosPaciente.FechaNacimiento}|{c.DatosPaciente.Correo}|{c.FechaHora}|{c.MotivoConsulta}|{c.Activa}";
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
                                c.DatosPaciente.FechaNacimiento = datos[4];
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
                    Pausar();
                }
            }
        }

        // --- MÉTODOS AUXILIARES Y DE VALIDACIÓN ---

        static void Pausar()
        {
            Console.WriteLine("Presione ENTER para continuar...");
            Console.ReadLine();
        }

        static string SolicitarFechaHoraLibre()
        {
            while (true)
            {
                // 1. Pedir solo la Fecha
                Console.Write("Fecha (Ej. 15/10/2026): ");
                string entradaFecha = Console.ReadLine() ?? "";

                if (!DateTime.TryParseExact(entradaFecha, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fechaCita))
                {
                    Console.WriteLine("ERROR: La fecha debe tener el formato DD/MM/AAAA.");
                    Pausar();
                    continue; // Vuelve a intentar
                }

                // 2. Pedir solo la Hora
                Console.Write("Hora (Formato 24h, Ej. 14:30): ");
                string entradaHora = Console.ReadLine() ?? "";

                if (!DateTime.TryParseExact(entradaHora, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime horaCita))
                {
                    Console.WriteLine("ERROR: La hora debe tener el formato HH:MM (Ej. 09:00, 14:30).");
                    Pausar();
                    continue; // Vuelve a intentar
                }

                // 3. Juntar ambas y verificar que no sea en el pasado
                DateTime fechaHoraCompleta = fechaCita.Date.Add(horaCita.TimeOfDay);
                if (fechaHoraCompleta < DateTime.Now)
                {
                    Console.WriteLine("ERROR: No puede programar una cita en una fecha/hora pasada.");
                    Pausar();
                    continue;
                }

                // 4. Verificar disponibilidad en la agenda
                string fechaHoraResult = fechaHoraCompleta.ToString("dd/MM/yyyy HH:mm");
                if (ValidarHorario(fechaHoraResult))
                {
                    return fechaHoraResult; // Salimos del bucle devolviendo la fecha limpia
                }
                else
                {
                    Console.WriteLine("ERROR: Ya existe una cita activa en ese horario. Intente con otra hora.");
                    Pausar();
                }
            }
        }

        static bool ValidarHorario(string fechaHora)
        {
            for (int i = 0; i < totalCitas; i++)
            {
                if (agendaCitas[i].Activa && agendaCitas[i].FechaHora == fechaHora)
                {
                    return false;
                }
            }
            return true;
        }

        static bool ValidarSoloLetras(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return false;
            return Regex.IsMatch(texto, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$");
        }

        static bool ValidarTelefonoNicaragua(string telefono)
        {
            if (string.IsNullOrWhiteSpace(telefono)) return false;
            return Regex.IsMatch(telefono, @"^\d{8}$");
        }

        static bool ValidarFechaNacimiento(string fechaStr, out DateTime fechaResultado)
        {
            return DateTime.TryParseExact(fechaStr, "dd/MM/yyyy",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaResultado);
        }

        static void ImprimirDetalleCita(Cita c)
        {
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"[ID Cita: {c.IdCita}] - Fecha y Hora: {c.FechaHora}");
            Console.WriteLine($"Paciente: {c.DatosPaciente.NombreCompleto} | F. Nac: {c.DatosPaciente.FechaNacimiento} | Tel: {c.DatosPaciente.Telefono}");
            Console.WriteLine($"Motivo: {c.MotivoConsulta}");
        }
    }
}