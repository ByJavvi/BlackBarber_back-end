using BlackBarberAPI.DTOs;
using BlackBarberAPI.Process;
using Microsoft.AspNetCore.Mvc;

namespace BlackBarberAPI.Controllers
{
    [Route("api/cita")]
    [ApiController]
    public class CitaController
    {
        private readonly CitaProceso _proceso;

        public CitaController(CitaProceso proceso)
        {
            _proceso = proceso;
        }

        [HttpPost("crearCita")]
        public async Task<ActionResult<RespuestaDTO>> CrearCita([FromBody] CitaCreacionDTO citaCreacionDTO)
        {
            var respuesta = await _proceso.CrearCita(citaCreacionDTO);
            return respuesta;
        }

        [HttpGet("obtenerCitasXbarbero/{idBarbero:int}")]
        public async Task<ActionResult<List<CitaDTO>>> ObtenerCitasXBarbero(int idBarbero)
        {
            var lista = await _proceso.ObtenerCitasXBarbero(idBarbero);
            return lista;
        }
    }
}
