namespace BlackBarberAPI.DTOs
{
    public class ServicioDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string? Descripcion { get; set; }

        public int? IdTipo { get; set; }

        public decimal PrecioBase { get; set; }

        public string? Base64 { get; set; }

        public int? Estatus { get; set; }
    }

    public class ServicioListadoDTO : ServicioDTO
    {
        public string TipoServicio { get; set; } = "";
    }

    public class ServicioConArchivoDTO : ServicioDTO
    {
        public IFormFile? Archivo { get; set; }
    }
}
