namespace BlackBarberAPI.DTOs
{
    public class DiasHabilDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public bool Habil { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
    }
}
