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


    }
}
