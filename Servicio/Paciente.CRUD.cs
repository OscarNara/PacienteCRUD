using System.Text.RegularExpressions;
using Oscar.Entidades;
using Oscar.Repositorio;

namespace Oscar.Servicios
{
    public class PacienteServicioo
    {
        private readonly PacientesRepositorio pacientesRepositorio = new PacientesRepositorio();
        private List<Paciente> pacientes;

        public PacienteServicioo()
        {
            // Obtén los pacientes iniciales del repositorio
            pacientes = pacientesRepositorio.LeerPacientes();
        }

        public void MostrarMenu()
        {
            while (true)
            {
                Console.Clear();
                MostrarEncabezado("Bienvenido a la aplicación de gestión de pacientes");

                Console.WriteLine("1. Crear Paciente");
                Console.WriteLine("2. Actualizar Paciente");
                Console.WriteLine("3. Eliminar Paciente");
                Console.WriteLine("4. Leer Pacientes");
                Console.WriteLine("5. Salir");
                Console.Write("Seleccione una opción: ");

                string opcion = Console.ReadLine() ?? "5";
                Operaciones operacion = Enum.Parse<Operaciones>(opcion);

                Console.Clear(); // Limpiamos la consola para cada opción

                switch (operacion)
                {
                    case Operaciones.Crear:
                        CrearPaciente();
                        break;
                    case Operaciones.Actualizar:
                        ActualizarPaciente();
                        break;
                    case Operaciones.Eliminar:
                        EliminarPaciente();
                        break;
                    case Operaciones.Leer:
                        LeerPacientes();
                        break;
                    case Operaciones.Salir:
                        Environment.Exit(5);
                        Console.WriteLine("Saliendo de la aplicación. ¡Hasta luego!");
                        return;
                    default:
                        Console.WriteLine("Opción no válida. Inténtelo de nuevo.");
                        break;
                }

                MostrarMensaje("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
            }
        }
        private void CrearPaciente()
        {
            MostrarEncabezado("Creando nuevo paciente");

            // Lógica para obtener datos del paciente
            Paciente nuevoPaciente = ObtenerDatosPaciente();

            // Utiliza el repositorio para crear el paciente
            pacientesRepositorio.CrearPaciente(nuevoPaciente);

            Console.Clear();
            MostrarEncabezado("Nuevo Paciente creado con éxito");
            MostrarInformacionPaciente(nuevoPaciente);
        }

        private void ActualizarPaciente()
        {
            MostrarEncabezado("Actualizando el paciente");

            Console.Write("Ingrese el ID del paciente a actualizar: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid idPaciente))
            {
                MostrarMensaje("Entrada no válida. Volviendo al menú principal.");
                return;
            }

            Paciente pacienteAActualizar = pacientes.Find(p => p.Id_Paciente == idPaciente)!;

            if (pacienteAActualizar == null)
            {
                MostrarMensaje("Paciente no encontrado. Volviendo al menú principal.");
                return;
            }

            Console.Clear();
            MostrarEncabezado("Paciente actualizado con éxito");
            MostrarInformacionPaciente(pacienteAActualizar);
        }
        private void EliminarPaciente()
        {
            Console.Write("Ingrese el Id del paciente a eliminar: ");
            if (Guid.TryParse(Console.ReadLine(), out Guid idPacienteEliminar))
            {
                Paciente pacienteEliminado = pacientes.Find(p => p.Id_Paciente == idPacienteEliminar)!;

                if (pacienteEliminado != null)
                {
                    Console.WriteLine("Eliminando el paciente...");
                    MostrarAnimacionCargando();

                    // Utiliza el repositorio para eliminar el paciente
                    pacientesRepositorio.EliminarPaciente(idPacienteEliminar);

                    // Elimina el paciente de la lista dinámica en el servicio
                    pacientes.Remove(pacienteEliminado);

                    Console.Clear();
                    MostrarEncabezado($"Paciente eliminado con éxito - Id: {pacienteEliminado.Id_Paciente}");
                    MostrarInformacionPaciente(pacienteEliminado);
                }
                else
                {
                    MostrarMensaje("No se encontró un paciente con el Id especificado.");
                }
            }
            else
            {
                MostrarMensaje("Entrada no válida. Por favor, ingrese un número.");
            }
        }
        private void LeerPacientes()
        {
            Console.WriteLine("Mostrando información de pacientes:");
            Console.WriteLine("===================================");

            // Utiliza la lista de pacientes en el servicio para mostrar la información
            foreach (var paciente in pacientes)
            {
                Console.WriteLine($"Id: {paciente.Id_Paciente}, NombreUsuario: {paciente.NombreUsuario}, Sexo: {paciente.Sexo}, Email: {paciente.Email}");
            }

            MostrarAnimacionCargando();
        }
        private Paciente ObtenerDatosPaciente()
        {
            MostrarEncabezado("Ingrese los datos del paciente:");

            Console.Write("Ingrese el nombre: ");
            string? nombre = Console.ReadLine();

            Console.Write("Ingrese el apellido: ");
            string? apellido = Console.ReadLine();

            Console.Write("Ingrese el sexo (Femenino/Masculino): ");
            string? sexo = Console.ReadLine();
            while (sexo?.ToLower() != "femenino" && sexo?.ToLower() != "masculino")
            {
                MostrarMensaje("Opción no válida. Inténtelo de nuevo.");
                Console.Write("Ingrese el sexo (Femenino/Masculino): ");
                sexo = Console.ReadLine();
            }

            Console.Write("Ingrese el email: ");
            string? email = Console.ReadLine();
            while (!EsCorreoValido(email))
            {
                MostrarMensaje("Correo no válido. Inténtelo de nuevo.");
                Console.Write("Ingrese el email: ");
                email = Console.ReadLine();
            }

            Console.Write("Ingrese la contraseña (mínimo 8 caracteres, al menos un número y un carácter especial): ");
            string? contraseña = Console.ReadLine();
            while (!EsContraseñaValida(contraseña))
            {
                MostrarMensaje("Contraseña no válida. Inténtelo de nuevo.");
                Console.Write("Ingrese la contraseña (mínimo 8 caracteres, al menos un número y un carácter especial): ");
                contraseña = Console.ReadLine();
            }

            Console.Write("Ingrese el número telefónico (8 dígitos): ");
            string? telefono = Console.ReadLine();
            while (!EsTelefonoValido(telefono))
            {
                MostrarMensaje("Número telefónico no válido. Inténtelo de nuevo.");
                Console.Write("Ingrese el número telefónico (8 dígitos): ");
                telefono = Console.ReadLine();
            }

            Guid idRol = Guid.NewGuid();
            Guid idPaciente = Guid.NewGuid(); // Nuevo Guid único

            return new Paciente
            {
                Id_Paciente = idPaciente,
                Id_Rol = idRol,
                NombreUsuario = $"{nombre} {apellido}",
                Sexo = sexo,
                Email = email,
                Contraseña = contraseña,
                Telefono = telefono
            };
        }
        // Métodos adicionales...
        private void MostrarEncabezado(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(mensaje);
            Console.ResetColor();
        }

        private void MostrarMensaje(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(mensaje);
            Console.ResetColor();
        }
        private void MostrarInformacionPaciente(Paciente paciente)
        {
            Console.WriteLine($"ID: {paciente.Id_Paciente}");
            Console.WriteLine($"Nombre de Usuario: {paciente.NombreUsuario}");
            Console.WriteLine($"Sexo: {paciente.Sexo}");
            Console.WriteLine($"Email: {paciente.Email}");
            Console.WriteLine($"Contraseña: {new string('*', paciente.Contraseña?.Length ?? 0)}");
            Console.WriteLine($"Teléfono: {paciente.Telefono}");
        }

        private void MostrarAnimacionCargando()
        {
            Console.Write("Cargando");
            for (int i = 0; i < 3; i++)
            {
                Console.Write(".");
                System.Threading.Thread.Sleep(500);
            }
            Console.WriteLine();
        }
        private bool EsCorreoValido(string? correo)
        {
            if (correo == null)
            {
                return false;
            }

            // Se utiliza una expresión regular simple para verificar si es un correo válido.
            string patronCorreo = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            return Regex.IsMatch(correo, patronCorreo);
        }

        private bool EsContraseñaValida(string? contraseña)
        {
            if (contraseña == null)
            {
                return false;
            }

            // Se utiliza una expresión regular para verificar que la contraseña tenga al menos 8 caracteres,
            // al menos un número y al menos un carácter especial.
            string patronContraseña = @"^(?=.*[a-zA-Z\d])(?=.*[!@#$%^&*()_+{}\[\]:;<>,.?~\\-]).{8,}$";
            return Regex.IsMatch(contraseña, patronContraseña);
        }

        private bool EsTelefonoValido(string? telefono)
        {
            if (telefono == null)
            {
                return false;
            }

            // Se utiliza una expresión regular simple para verificar que el número de teléfono tenga exactamente 8 dígitos.
            string patronTelefono = @"^\d{8}$";
            return Regex.IsMatch(telefono, patronTelefono);
        }

    }
}