namespace BlackBarberAPI.Models
{
    public class DiasHabiles
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public bool Habil {  get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin {  get; set; }
    }
}
