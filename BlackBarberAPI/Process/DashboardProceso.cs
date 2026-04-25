using BlackBarberAPI.Data;
using BlackBarberAPI.DTOs;
using BlackBarberAPI.Services.Contratos;

namespace BlackBarberAPI.Process
{
    public class DashboardProceso
    {
        private readonly BarberoProceso _barberoProceso;
        private readonly ServicioProceso _servicioProceso;
        private readonly CitaProceso _citaProceso;
        private readonly IServicioCitaService<BlackBarberContext> _servicioCitaService;

        public DashboardProceso(BarberoProceso barberoProceso, ServicioProceso servicioProceso, CitaProceso citaProceso, IServicioCitaService<BlackBarberContext> servicioCitaService)
        {
            _barberoProceso = barberoProceso;
            _servicioProceso = servicioProceso;
            _citaProceso = citaProceso;
            _servicioCitaService = servicioCitaService;
        }

        public async Task<DashboardAdminDTO> ObtenerDashboardAdmin()
        {
            DashboardAdminDTO dashboard = new DashboardAdminDTO();
            var barberos = await _barberoProceso.ObtenerBarberos();
            dashboard.Barberostotal = barberos.Count();
            dashboard.BarberosActivos = barberos.Where(b => b.Estatus == 1).Count();
            var servicios = await _servicioProceso.ObtenerTodosServicios();
            dashboard.ServiciosActivos = servicios.Count();
            var countExtras = 0;
            foreach(var servicio in servicios)
            {
                var anadidos = await _servicioProceso.ObtenerAnadidosXPerteneciente(servicio.Id);
                countExtras += anadidos.Count();
            }
            dashboard.ExtrasServicios = countExtras;
            var citas = await _citaProceso.ObtenerCitasHoy();
            var citasAyer = await _citaProceso.ObtenerCitasAyer();
            var diferencia = citas.Count() - citasAyer.Count();
            if(diferencia == 0 || citasAyer.Count() == 0)
            {
                dashboard.PorcentajeCitas = 0;
            }
            else
            {
                dashboard.PorcentajeCitas = (diferencia / citasAyer.Count()) * 100;
            }
            dashboard.CitasDeldia = citas.Count();
            decimal sumaIngresos = 0;
            foreach(var cita in citas)
            {
                var detallesCita = await _servicioCitaService.ObtenerXPerteneciente(cita.Id);
                foreach (var detalle in detallesCita)
                {
                    sumaIngresos += detalle.Precio;
                }
            }
            dashboard.IngresosDeldia = sumaIngresos;
            dashboard.PorcentajeXTicket = citas.Count()>0 ? sumaIngresos / citas.Count() : 0;
            //Falta objtener la información de las citas del día, ingresos y porcentajes
            return dashboard;
        }

        public async Task<DashboardBarberoDTO> ObtenerDashboardBarbero(int idBarbero)
        {
            DashboardBarberoDTO dashboard = new DashboardBarberoDTO();
            var citas = await _citaProceso.ObtenerCitasXBarbero(idBarbero);
            var citasHoy = citas.Where(c => DateOnly.FromDateTime(c.FechaInicio.Date) == DateOnly.FromDateTime(DateTime.Now)).ToList();
            dashboard.CitasDeldia = citasHoy.Count();
            dashboard.CitasEnCurso = citasHoy.Where(c => c.Estatus == 2).Count();
            dashboard.CitasPorAtender = citasHoy.Where(c => c.Estatus == 1).Count();
            var proximaCita = citasHoy.Where(c => c.Estatus == 1).OrderBy(c => c.FechaInicio).FirstOrDefault();
            dashboard.ProximaCita = proximaCita != null ? proximaCita.FechaInicio.ToString("g") : "No hay próximas citas";
            dashboard.CitasAtendidas = citasHoy.Where(c => c.Estatus == 3).Count();
            dashboard.CalidadRitmo = dashboard.CitasAtendidas > 5 ? "Excelente" : dashboard.CitasAtendidas > 2 ? "Buen ritmo de trabajo" : "Ya mejorará";
            //Falta la disponiblilidad
            return dashboard;
        }

        public async Task<DashboardClienteDTO> ObtenerDashboardCliente(int IdCliente)
        {
            DashboardClienteDTO dashboard = new DashboardClienteDTO();
            var citas = await _citaProceso.ObtenerCitasXCliente(IdCliente);
            dashboard.Reservas = citas.Count();
            var citasProximaSemana = citas.Where(c => c.FechaInicio.Date >= DateTime.Now.Date && c.FechaInicio.Date <= DateTime.Now.AddDays(7).Date).ToList();
            dashboard.DetalleReservas = $"{citasProximaSemana.Count()} para esta semana";
            var idsServicios = new List<int>();
            var idsBarberos = new List<int>();
            foreach (var cita in citas)
            {
                var detallesCita = await _servicioCitaService.ObtenerXPerteneciente(cita.Id);
                foreach (var detalle in detallesCita)
                {
                    if(detalle.IdServicio!=null && !idsServicios.Contains((int)detalle.IdServicio))
                    idsServicios.Add((int)detalle.IdServicio);
                    if (detalle.IdBarbero != null && !idsBarberos.Contains((int)detalle.IdBarbero))
                    idsBarberos.Add((int) detalle.IdBarbero);
                }
            }
            dashboard.ServiciosFavoritos = idsServicios.Count();
            var servicio = await _servicioProceso.ObtenerXId(idsServicios.FirstOrDefault());
            dashboard.DetalleServiciosFavoritos = servicio!= null? servicio.Nombre : "";
            dashboard.BarberosSugeridos = idsBarberos.Count();
            //Falta  obtener la disponibiliodad
            return dashboard;
        }
    }
}
