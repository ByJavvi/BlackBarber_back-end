namespace BlackBarberAPI.DTOs
{
    public class DashboardAdminDTO
    {
        public int ServiciosActivos { get; set; }
        public int ExtrasServicios { get; set; }
        public int Barberostotal{ get; set; }
        public int BarberosActivos { get; set; }
        public int CitasDeldia { get; set; }
        public int PorcentajeCitas { get; set; }
        public decimal IngresosDeldia { get; set; }
        public decimal PorcentajeXTicket { get; set; }
    }

    public class  DashboardBarberoDTO
    {
        public int CitasDeldia { get; set; }
        public int CitasEnCurso { get; set; }
        public int CitasPorAtender{ get; set; }
        public string ProximaCita { get; set; } = "";
        public int CitasAtendidas{ get; set; }
        public string CalidadRitmo { get; set; } = "";
        public string Disponibilidad { get; set; } = "";
        public string DescripcionDisponibilidad { get; set; } = "";
    }

    public class DashboardClienteDTO
    {
        public int Reservas { get; set; }
        public string DetalleReservas { get; set; } = "";
        public int ServiciosFavoritos { get; set; }
        public string DetalleServiciosFavoritos { get; set; } = "";
        public int BarberosSugeridos { get; set; }
        public string Disponibilidad { get; set; } = "";
        public string DetalleDisponibilidad { get; set; } = "";
    }
}
