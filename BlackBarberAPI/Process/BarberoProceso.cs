using BlackBarberAPI.Data;
using BlackBarberAPI.DTOs;
using BlackBarberAPI.Services.Contratos;

namespace BlackBarberAPI.Process
{
    public class BarberoProceso
    {
        private readonly IBarberoService<BlackBarberContext> _barberoService;
        private readonly IUsuarioService<BlackBarberContext> _usuarioService;
        private readonly CitaProceso _citaProceso;

        public BarberoProceso(IBarberoService<BlackBarberContext> barberoContext, IUsuarioService<BlackBarberContext> usuarioService, CitaProceso citaProceso)
        {
            _barberoService = barberoContext;
            _usuarioService = usuarioService;
            _citaProceso = citaProceso;
        }

        public async Task<List<BarberoListadoDTO>> ObtenerBarberos()
        {
            List<BarberoListadoDTO> listado = new List<BarberoListadoDTO>();
            var lista = await _barberoService.ObtenerTodos();
            var usuarios = await _usuarioService.ObtenerTodos();
            foreach(var barbero in lista)
            {
                var usuario = usuarios.Where(x => x.Id == barbero.IdUsuario).FirstOrDefault();
                listado.Add(new BarberoListadoDTO
                {
                    Id = barbero.Id,
                    Nombre = barbero.Nombre,
                    IdUsuario = barbero.IdUsuario,
                    Estatus = barbero.Estatus,
                    NombreUsuario = usuario!=null?usuario.Username:"Desconocido",
                    DescripcionEstatus = barbero.Estatus == 1 ? "Activo" : "No disponible"
                });
            }
            return listado;
        }

        public async Task<RespuestaDTO> CrearBarbero(BarberoDTO barbero)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var usuario = await _usuarioService.ObtenerXId((int)barbero.IdUsuario);
            if (usuario == null)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "El usuario no existe";
                return respuesta;
            }
            barbero.Estatus = 1;
            var barberoCreado = await _barberoService.CrearYObtener(barbero);
            respuesta.Estatus = barberoCreado.Id > 0;
            respuesta.Descripcion = barberoCreado.Id > 0 ? "Barbero creado correctamente" : "Error al crear el barbero";
            if (respuesta.Estatus)
            {
                usuario.IdRol = 2; // Asignar rol de barbero
                await _usuarioService.Editar(usuario);
            }
            return respuesta;
        }

        public async Task<RespuestaDTO> EditarBarbero(BarberoDTO barbero)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            respuesta = await _barberoService.Editar(barbero);
            return respuesta;
        }

        public async Task<RespuestaDTO> EliminarBarbero(int id)
        {
            var barbero = await _barberoService.ObtenerXId(id);
            if (barbero.Id <= 0)
            {
                return new RespuestaDTO
                {
                    Estatus = false,
                    Descripcion = "El barbero no existe."
                };
            }
            var citas = await _citaProceso.ObtenerCitasXBarbero(id);
            if (citas.Count() > 0)
            {
                return new RespuestaDTO
                {
                    Estatus = false,
                    Descripcion = "No se puede eliminar el barbero si ya se le han asignado citas."
                };
            }
            var eliminado = await _barberoService.Eliminar(id);
            if (eliminado.Estatus)
            {
                var usuario = await _usuarioService.ObtenerXId((int)barbero.IdUsuario);
                if (usuario.Id > 0)
                {
                    usuario.IdRol = 2; // Asignar rol de cliente
                    await _usuarioService.Editar(usuario);
                }
            }
            return eliminado;
        }

        public async Task<RespuestaDTO> AsignarPerfilBarbero(AsignacionBarberoDTO asignacion)
        {
            var barbero = await _barberoService.ObtenerXId(asignacion.IdBarbero);
            var usuario = await _usuarioService.ObtenerXId(asignacion.IdUsuario);
            if(barbero.Id<= 0 || usuario.Id <= 0)
            {
                return new RespuestaDTO
                {
                    Estatus = false,
                    Descripcion = "El barbero o el usuario no existen"
                };
            }
            if (barbero.IdUsuario != null)
            {
                var usuarioAnterior = await _usuarioService.ObtenerXId((int)barbero.IdUsuario);
                if (usuarioAnterior.Id > 0)
                {
                    usuarioAnterior.IdRol = 2; // Asignar rol de cliente
                    await _usuarioService.Editar(usuarioAnterior);
                }
            }
            barbero.IdUsuario = asignacion.IdUsuario;
            var respuesta = await _barberoService.Editar(barbero);
            if (respuesta.Estatus)
            {
                usuario.IdRol = 3; // Asignar rol de barbero
                await _usuarioService.Editar(usuario);
            }
            return respuesta;
        }
    }
}
