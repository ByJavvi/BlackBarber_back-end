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
        public List<ServicioCitaDTO> Servicios { get; set; } = new List<ServicioCitaDTO>();
    }

    public class CitaDetalladaDTO : CitaDTO
    {
        public string NombreCliente { get; set; } = "";
        public string EstatusDescripcion { get; set; } = "";
        public decimal Total { get; set; }
        public List<ServicioCitaDetalladoDTO> Servicios { get; set; } = new List<ServicioCitaDetalladoDTO>();
    }
}
