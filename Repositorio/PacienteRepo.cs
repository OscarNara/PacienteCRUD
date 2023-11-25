using Oscar.Entidades;

namespace Oscar.Repositorio
{
    public class PacientesRepositorio
    {
        private readonly List<Paciente> pacientes;
        public List<Paciente> ObtenerTodosLosPacientes()
        {
            return pacientes;
        }

        public PacientesRepositorio()
        {
            // Inicialización de pacientes (puedes modificar o agregar según tus necesidades)
            var paciente1 = new Paciente
            {
                Id_Paciente = Guid.NewGuid(),
                NombreUsuario = "Paciente1",
                Apellido = "Apellidos1",
                Sexo = "Femenino",
                Email = "paciente1@gmail.com",
                Contraseña = "Contraseña1",
                Telefono = "12345678"
                // Agrega otros atributos según tu modelo de Paciente...
            };

            var paciente2 = new Paciente
            {
                Id_Paciente = Guid.NewGuid(),
                NombreUsuario = "Paciente2",
                Apellido = "Apellidos2",
                Sexo = "Masculino",
                Email = "paciente2@gmail.com",
                Contraseña = "Contraseña2",
                Telefono = "87654321"
                // Agrega otros atributos según tu modelo de Paciente...
            };

            pacientes = new List<Paciente> { paciente1, paciente2 };
        }

        public void CrearPaciente(Paciente paciente)
        {
            pacientes.Add(paciente);
        }

        public void ActualizarPaciente(Paciente paciente)
        {
            Paciente pacienteExistente = ObtenerPacientePorId(paciente.Id_Paciente);

            if (pacienteExistente != null)
            {
                // Actualiza los atributos del pacienteExistente con los valores del paciente que se pasa como parámetro.
                pacienteExistente.NombreUsuario = paciente.NombreUsuario;
                pacienteExistente.Apellido = paciente.Apellido;
                pacienteExistente.Sexo = paciente.Sexo;
                pacienteExistente.Email = paciente.Email;
                pacienteExistente.Contraseña = paciente.Contraseña;
                pacienteExistente.Telefono = paciente.Telefono;
                // Actualiza otros atributos según tu modelo de Paciente...
            }
            else
            {
                Console.WriteLine("Paciente no encontrado.");
            }
        }
        public void EliminarPaciente(Guid id)
        {
            Paciente pacienteExistente = ObtenerPacientePorId(id);

            if (pacienteExistente != null)
            {
                pacientes.Remove(pacienteExistente);
                Console.WriteLine($"Paciente eliminado con éxito - Id: {pacienteExistente.Id_Paciente}");
            }
            else
            {
                Console.WriteLine("Paciente no encontrado.");
            }
        }

        public Paciente ObtenerPacientePorId(Guid id)
        {
            return pacientes.Find(p => p.Id_Paciente == id)!;
        }

        public List<Paciente> LeerPacientes()
        {
            return pacientes;
        }
    }
}
