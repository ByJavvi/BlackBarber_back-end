namespace BlackBarberAPI.DTOs
{
    public class RestablecerContrasenaDTO
    {
        public string Correo { get; set; } = "";
        public string Token { get; set; } = "";
        public string Contrasena { get; set; } = "";
    }
}
