using BlackBarberAPI.DTOs;
using BlackBarberAPI.Process;
using Microsoft.AspNetCore.Mvc;

namespace BlackBarberAPI.Controllers
{
    [Route("api/servicio")]
    [ApiController]
    public class ServicioController : ControllerBase
    {
        private readonly ServicioProceso _servicioProceso;

        public ServicioController(ServicioProceso servicioProceso)
        {
            _servicioProceso = servicioProceso;
        }

        [HttpGet("obtenerServicios")]
        public async Task<ActionResult<List<ServicioDTO>>> ObtenerServicios()
        {
            return await _servicioProceso.ObtenerTodosServicios();
        }

        [HttpGet("obtenerAnadidosServicios/{id:int}")]
        public async Task<ActionResult<List<AnadidoServicioDTO>>> ObtenerAnadidosSeriviocs(int id)
        {
            return await _servicioProceso.ObtenerAnadidosXPerteneciente(id);
        }

        [HttpPost("crearServicio")]
        public async Task<ActionResult<RespuestaDTO>> CrearServicio([FromBody] ServicioConArchivoDTO objeto)
        {
            return await _servicioProceso.CrearServicio(objeto);
        }

        [HttpPost("editarServicio")]
        public async Task<ActionResult<RespuestaDTO>> EditarServicio([FromBody] ServicioConArchivoDTO objeto)
        {
            return await _servicioProceso.EditarServicio(objeto);
        }

        [HttpGet("eliminarServicio/{id:int}")]
        public async Task<ActionResult<RespuestaDTO>> EliminarServicio(int id)
        {
            return await _servicioProceso.EliminarServicio(id);
        }

        [HttpPost("crearAnadidoServicio")]
        public async Task<ActionResult<RespuestaDTO>> CrearAnadidoServicio([FromBody] AnadidoServicioDTO objeto)
        {
            return await _servicioProceso.CrearAnadidoServicio(objeto);
        }

        [HttpPost("editarAnadidoServicio")]
        public async Task<ActionResult<RespuestaDTO>> EditarAnadidoServicio([FromBody] AnadidoServicioDTO objeto)
        {
            return await _servicioProceso.EditarAnadidoServicio(objeto);
        }

        [HttpGet("eliminarAnadidoServicio/{id:int}")]
        public async Task<ActionResult<RespuestaDTO>> EliminarAnadidoServicio(int id)
        {
            return await _servicioProceso.EliminarAnadidoServicio(id);
        }
    }
}
