using BlackBarberAPI.DTOs;
using BlackBarberAPI.Process;
using Microsoft.AspNetCore.Mvc;

namespace BlackBarberAPI.Controllers
{
    [Route("api/usuario")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioProceso _proceso;

        public UsuarioController(UsuarioProceso proceso)
        {
            _proceso = proceso;
        }

        [HttpPost("login")]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> Login([FromBody] CredencialesUsuarioDTO credenciales)
        {
            var resultado = await _proceso.IniciarSesion(credenciales);
            return resultado;
        }

        [HttpPost("crearUsuario")]
        public async Task<ActionResult<RespuestaDTO>> CrearUsuario([FromBody] UsuarioCreacionDTO usuario)
        {
            var resultado = await _proceso.CrearUsuario(usuario);
            return resultado;
        }

        [HttpPost("editarUsuario")]
        public async Task<ActionResult<RespuestaDTO>> EditarUsuario([FromBody] UsuarioEdicionDTO usuario)
        {
            var resultado = await _proceso.EditarUsuario(usuario);
            return resultado;
        }

        [HttpGet("obtenerListadoUsuarios")]
        public async Task<ActionResult<List<UsuarioDTO>>> ObtenerListado()
        {
            var lista = await _proceso.ObtenerListadoUsuarios();
            return lista;
        }

        public record CorreoDTO(string email);
        [HttpPost("enviarEmailRecuperacion")]
        public async Task<ActionResult<RespuestaDTO>> EnviarEmailRecuperacion([FromBody] CorreoDTO correo)
        {
            var resultado = await _proceso.EnviarCorreoRecuperacion(correo.email);
            return resultado;
        }

        [HttpPost("restablecerPassword")]
        public async Task<ActionResult<RespuestaDTO>> RestablecerPassword([FromBody] RestablecerContrasenaDTO credenciales)
        {
            var resultado = await _proceso.RestablecerContrasena(credenciales);
            return resultado;
        }

        [HttpPost("obtenerDisponibilidadXUsername")]
        public async Task<ActionResult<RespuestaDTO>> ObtenerDisponibilidadUsername(string username)
        {
            var resultado = await _proceso.ObtenerDisponibilidadNombreUsuario(username);
            return resultado;
        }

        [HttpPost("obtenerDisponibilidadXCorreo")]
        public async Task<ActionResult<RespuestaDTO>> ObtenerDisponibilidadCorreo(string correo)
        {
            var resultado = await _proceso.ObtenerDisponibilidadCorreo(correo);
            return resultado;
        }
    }
}
