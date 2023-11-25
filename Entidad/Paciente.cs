namespace Oscar.Entidades
{
    public class Paciente
    {
        public Guid Id_Paciente { get; set; }
        public Guid Id_Rol { get; set; }  

        public string? NombreUsuario { get; set; }
        public string? Apellido { get; set; }
        public string? Sexo { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Contraseña { get; set; }
    }
}
