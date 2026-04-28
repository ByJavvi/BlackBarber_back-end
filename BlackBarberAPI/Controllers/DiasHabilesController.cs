using BlackBarberAPI.DTOs;
using BlackBarberAPI.Process;
using Microsoft.AspNetCore.Mvc;

namespace BlackBarberAPI.Controllers
{
    [Route("api/diasHabiles")]
    [ApiController]
    public class DiasHabilesController : ControllerBase
    {
        private readonly DiaSHabilesProceso _proceso;

        public DiasHabilesController(DiaSHabilesProceso proceso)
        {
            _proceso = proceso;
        }

        [HttpGet("todos")]
        public async Task<ActionResult<List<DiasHabilDTO>>> ObtenerTodos()
        {
            var lista = await _proceso.ObtenerDiasHabiles();
            return lista;
        }

        [HttpPost("editar")]
        public async Task<ActionResult<RespuestaDTO>> Editar(DiasHabilDTO objeto)
        {
            var resultado = await _proceso.EditarDiaHabil(objeto);
            return resultado;
        }
    }
}
