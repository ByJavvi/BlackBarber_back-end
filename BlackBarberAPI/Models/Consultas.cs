namespace BlackBarberAPI.Models
{
    public class Consultas
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Correo { get; set; } = "";
        public string Mensaje { get; set; } = "";
        public int Estatus { get; set; }
    }
}
