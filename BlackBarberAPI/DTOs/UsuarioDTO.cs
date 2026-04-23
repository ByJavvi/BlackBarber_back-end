namespace BlackBarberAPI.DTOs
{
    public class UsuarioDTO
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string Correo { get; set; } = null!;

        public string? PasswordHash { get; set; }

        public byte[] HoraCreacion { get; set; } = null!;

        public int? Estatus { get; set; }

        public int? IdRol { get; set; }
    }

    public class UsuarioCreacionDTO
    {
        public string Username { get; set; } = "";
        public string Correo { get; set; } = "";
        public string Contrasena { get; set; } = "";
    }

    public class UsuarioEdicionDTO : UsuarioCreacionDTO
    {
        public int Id { get; set; }
        public int Estatus { get; set; }

    }
}
