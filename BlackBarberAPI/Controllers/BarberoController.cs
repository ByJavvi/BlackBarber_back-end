using BlackBarberAPI.DTOs;
using BlackBarberAPI.Process;
using Microsoft.AspNetCore.Mvc;

namespace BlackBarberAPI.Controllers
{
    [Route("api/barbero")]
    [ApiController]
    public class BarberoController : ControllerBase
    {
        private readonly BarberoProceso _proceso;

        public BarberoController(BarberoProceso proceso)
        {
            _proceso = proceso;
        }

        [HttpGet("obtenerBarberos")]
        public async Task<ActionResult<List<BarberoListadoDTO>>> ObtenerBarberos()
        {
            var lista = await _proceso.ObtenerBarberos();
            return lista;
        }

        [HttpPost("crearBarbero")]
        public async Task<ActionResult<RespuestaDTO>> CrearBarbero([FromBody] BarberoDTO barbero)
        {
            var respuesta = await _proceso.CrearBarbero(barbero);
            return respuesta;
        }

        [HttpPost("editarBarbero")]
        public async Task<ActionResult<RespuestaDTO>> EditarBarbero([FromBody] BarberoDTO barbero)
        {
            var respuesta = await _proceso.EditarBarbero(barbero);
            return respuesta;
        }

        [HttpGet("eliminarBarbero/{id:int}")]
        public async Task<ActionResult<RespuestaDTO>> EliminarBarbero(int id)
        {
            var respuesta = await _proceso.EliminarBarbero(id);
            return respuesta;
        }

        [HttpPost("asignarPerfilBarbero")]
        public async Task<ActionResult<RespuestaDTO>> AsignarPerfilBarbero([FromBody] AsignacionBarberoDTO asignacion)
        {
            var respuesta = await _proceso.AsignarPerfilBarbero(asignacion);
            return respuesta;
        }
}
