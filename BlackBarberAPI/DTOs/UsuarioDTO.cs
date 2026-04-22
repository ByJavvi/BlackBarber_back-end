namespace BlackBarberAPI.DTOs
{
    public class UsuarioDTO
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string Correo { get; set; } = null!;

        public string? PasswordHash { get; set; }

        public DateTime HoraCreacion { get; set; }

        public int? Estatus { get; set; }

        public int? IdRol { get; set; }
    }
}
