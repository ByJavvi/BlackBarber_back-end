using BlackBarberAPI.Data;
using BlackBarberAPI.DTOs;
using BlackBarberAPI.Services.Contratos;
using Microsoft.AspNetCore.Mvc;

namespace BlackBarberAPI.Controllers
{
    [Route("api/consultas")]
    [ApiController]
    public class ConsultaController
    {
        private readonly IConsultaService<BlackBarberContext> _service;

        public ConsultaController(IConsultaService<BlackBarberContext> service)
        {
            _service = service;
        }

        [HttpGet("obtenerConsultas")]
        public async Task<ActionResult<List<ConsultaDTO>>> ObtenerTodas()
        {
            var lista = await _service.ObtenerTodos();
            return lista;
        }

        [HttpPost("crearConsulta")]
        public async Task<ActionResult<RespuestaDTO>> CrearConsulta(ConsultaDTO consultaDTO)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var resultado = await _service.CrearConsulta(consultaDTO);
            respuesta.Estatus = resultado.Id > 0;
            respuesta.Descripcion = respuesta.Estatus ? "Consulta enviada exitosamente" : "Ocurrió un error al intentar subir la consulta";
            return respuesta;
        }

        [HttpPost("editarConsulta")]
        public async Task<ActionResult<RespuestaDTO>> EditarConsulta(ConsultaDTO consultaDTO)
        {
            var resultado = await _service.EditarConsulta(consultaDTO);
            return resultado;
        }
    }
}
