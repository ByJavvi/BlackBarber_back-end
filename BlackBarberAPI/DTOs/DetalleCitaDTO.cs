namespace BlackBarberAPI.DTOs
{
    public class DetalleCitaDTO
    {
        public int Id { get; set; }

        public int? IdServicioCita { get; set; }

        public int? IdAnadidoServicion {  get; set; }

        public decimal Precio { get; set; }
    }
}
