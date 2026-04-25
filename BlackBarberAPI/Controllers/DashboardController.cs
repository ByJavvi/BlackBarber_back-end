using BlackBarberAPI.DTOs;
using BlackBarberAPI.Process;
using Microsoft.AspNetCore.Mvc;

namespace BlackBarberAPI.Controllers
{
    [Route("api/dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardProceso _proceso;

        public DashboardController(DashboardProceso proceso)
        {
            _proceso = proceso;
        }

        [HttpGet("dashboardAdmin")]
        public async Task<ActionResult<DashboardAdminDTO>> ObtenerDashboardAdmin()
        {
            var resultado = await _proceso.ObtenerDashboardAdmin();
            return resultado;
        }

        [HttpGet("dashboardBarbero/{idBarbero:int}")]
        public async Task<ActionResult<DashboardBarberoDTO>> ObtenerDashboardBarbero(int idBarbero)
        {
            var resultado = await _proceso.ObtenerDashboardBarbero(idBarbero);
            return resultado;
        }
    }
}
