using BlackBarberAPI.DTOs;

namespace BlackBarberAPI.Process
{
    public class DashboardProceso
    {
        private readonly BarberoProceso _barberoProceso;
        private readonly ServicioProceso _servicioProceso;
        private readonly CitaProceso _citaProceso;

        public DashboardProceso(BarberoProceso barberoProceso, ServicioProceso servicioProceso, CitaProceso citaProceso)
        {
            _barberoProceso = barberoProceso;
            _servicioProceso = servicioProceso;
            _citaProceso = citaProceso;
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
            //Falta objtener la información de las citas del día, ingresos y porcentajes
            return dashboard;
        }


    }
}
