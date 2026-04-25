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

    public class ServicioCitaDetalladoDTO : ServicioCitaDTO
    {
        public string NombreBarbero { get; set; }
        public string NombreServicio { get; set; }
    }
}
