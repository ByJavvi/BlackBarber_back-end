namespace BlackBarberAPI.DTOs
{
    public class ServicioCitaDTO
    {
        public int Id { get; set; }

        public int? IdCita { get; set; }

        public int? IdBarbero { get; set; }

        public int? IdServicio { get; set; }

        public decimal Precio { get; set; }
    }
}
