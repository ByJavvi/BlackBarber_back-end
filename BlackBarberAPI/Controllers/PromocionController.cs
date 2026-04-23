using BlackBarberAPI.DTOs;
using BlackBarberAPI.Process;
using Microsoft.AspNetCore.Mvc;

namespace BlackBarberAPI.Controllers
{
    [Route("api/promocion")]
    [ApiController]
    public class PromocionController : ControllerBase
    {
        private readonly PromocionProceso _promocionProceso;

        public PromocionController(PromocionProceso promocionProceso)
        {
            _promocionProceso = promocionProceso;
        }

        [HttpGet("obtenerPromociones")]
        public async Task<ActionResult<List<PromocionDTO>>> ObtenerTodas()
        {
            var lista = await _promocionProceso.ObtenerTodasPromociones();
            return lista;
        }

        [HttpGet("obtenerPromocionesvigentes")]
        public async Task<ActionResult<List<PromocionDTO>>> ObtenerVigentes()
        {
            var lista = await _promocionProceso.ObtenerPromocionesVigentes();
            return lista;
        }

        [HttpPost("crearPromocion")]
        public async Task<ActionResult<RespuestaDTO>> CrearPromociones([FromBody] PromocionDTO promocion)
        {
            var respuesta = await _promocionProceso.CrearPromocion(promocion);
            return respuesta;
        }

        [HttpPost("editarPromocion")]
        public async Task<ActionResult<RespuestaDTO>> EditarPromocion([FromBody] PromocionDTO promocion)
        {
            var respuesta = await _promocionProceso.EditarPromocion(promocion);
            return respuesta;
        }

        [HttpGet("eliminarPromocion/{id:int}")]
        public async Task<ActionResult<RespuestaDTO>> EliminarPromocion(int id)
        {
            var respuesta = await _promocionProceso.EliminarPromocion(id);
            return respuesta;
        }

    }
}
