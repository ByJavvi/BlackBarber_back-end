using BlackBarberAPI.Data;
using BlackBarberAPI.DTOs;
using BlackBarberAPI.Process;
using BlackBarberAPI.Services.Contratos;
using Microsoft.AspNetCore.Mvc;

namespace BlackBarberAPI.Controllers
{
    [Route("api/perfume")]
    [ApiController]
    public class PerfumeController : ControllerBase
    {
        private readonly PerfumeProceso _perfumeProceso;

        public PerfumeController(PerfumeProceso perfumeService)
        {
            _perfumeProceso = perfumeService;
        }

        [HttpGet("obtenerPerfumes")]
        public async Task<ActionResult<List<PerfumeDTO>>> ObtenerTodos()
        {
            var lista = await _perfumeProceso.ObtenerPerfumes();
            return lista;
        }

        [HttpPost("crearPerfume")]
        public async Task<ActionResult<RespuestaDTO>> CrearPerfume([FromBody] PerfumeConArchivoDTO objeto)
        {
            var respuesta = await _perfumeProceso.CrearPerfume(objeto);
            return respuesta;
        }

        [HttpPost("editarPerfume")]
        public async Task<ActionResult<RespuestaDTO>> EditarPerfume([FromBody] PerfumeConArchivoDTO objeto)
        {
            var respuesta = await _perfumeProceso.EditarPerfume(objeto);
            return respuesta;
        }

        [HttpGet("eliminarPerfume/{id:int}")]
        public async Task<ActionResult<RespuestaDTO>> EliminarPerfume(int id)
        {
            var respuesta = await _perfumeProceso.EliminarPerfume(id);
            return respuesta;
        }
    }
}
