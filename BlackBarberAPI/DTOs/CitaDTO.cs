namespace BlackBarberAPI.DTOs
{
    public class CitaDTO
    {
        public int Id { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaTermino { get; set; }

        public int? IdCliente { get; set; }

        public int? Estatus { get; set; }
    }

    public class CitaCreacionDTO : CitaDTO
    {
        public List<ServicioCitaDTO> servicios { get; set; } = new List<ServicioCitaDTO>();
    }
}
