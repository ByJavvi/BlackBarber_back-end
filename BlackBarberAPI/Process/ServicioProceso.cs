using BlackBarberAPI.Data;
using BlackBarberAPI.DTOs;
using BlackBarberAPI.Services.Contratos;

namespace BlackBarberAPI.Process
{
    public class ServicioProceso
    {
        private readonly IServicioService<BlackBarberContext> _servicioService;
        private readonly IAnadidoServicioService<BlackBarberContext> _anadidoServicioService;

        public ServicioProceso(IServicioService<BlackBarberContext> servicioService, IAnadidoServicioService<BlackBarberContext> anadidoServicioService)
        {
            _servicioService = servicioService;
            _anadidoServicioService = anadidoServicioService;
        }

        public async Task<List<ServicioDTO>> ObtenerTodosServicios()
        {
            var lista = await _servicioService.ObtenerTodos();
            return lista;
        }

        public async Task<RespuestaDTO> CrearServicio(ServicioConArchivoDTO objeto)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            ServicioDTO servicio = new ServicioDTO
            {
                Nombre = objeto.Nombre,
                Descripcion = objeto.Descripcion,
                IdTipo = objeto.IdTipo,
                PrecioBase = objeto.PrecioBase,
                Estatus = objeto.Estatus
            };
            if(objeto.Archivo != null)
            {
                servicio.Base64 = await GenerarBase64(objeto.Archivo);
            }
            var objetoCreado = await _servicioService.CrearYObtener(servicio);
            respuesta.Estatus = objetoCreado.Id > 0;
            respuesta.Descripcion = respuesta.Estatus ? "Servicio creado exitosamente" : "Error al crear el servicio";
            return respuesta;
        }

        public async Task<RespuestaDTO> EditarServicio(ServicioConArchivoDTO objeto)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            ServicioDTO servicio = new ServicioDTO
            {
                Id = objeto.Id,
                Nombre = objeto.Nombre,
                Descripcion = objeto.Descripcion,
                IdTipo = objeto.IdTipo,
                PrecioBase = objeto.PrecioBase,
                Estatus = objeto.Estatus
            };
            if (objeto.Archivo != null)
            {
                servicio.Base64 = await GenerarBase64(objeto.Archivo);
            }
            respuesta =  await _servicioService.Editar(servicio);
            return respuesta;
        }

        public async Task<RespuestaDTO> EliminarServicio(int id)
        {
            var anadidosServicio = await _anadidoServicioService.ObtenerXPerteneciente(id);
            foreach (var anadido in anadidosServicio)
            {
                await _anadidoServicioService.Eliminar(anadido.Id);
            }
            var respuesta = await _servicioService.Eliminar(id);
            return respuesta;
            //Falta validar que el servicio no esté siendo utilizado en alguna cita, si es así, no se podrá eliminar y se deberá informar al usuario.
        }

        public async Task<List<AnadidoServicioDTO>> ObtenerAnadidosXPerteneciente(int idServicio)
        {
            var lista = await _anadidoServicioService.ObtenerXPerteneciente(idServicio);
            return lista;
        }

        public async Task<RespuestaDTO> CrearAnadidoServicio(AnadidoServicioDTO objeto)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var objetoCreado = await _anadidoServicioService.CrearYObtener(objeto);
            respuesta.Estatus = objetoCreado.Id > 0;
            respuesta.Descripcion = respuesta.Estatus ? "Añadido creado exitosamente" : "Error al crear el añadido";
            return respuesta;
        }

        public async Task<RespuestaDTO> EditarAnadidoServicio(AnadidoServicioDTO objeto)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            respuesta = await _anadidoServicioService.Editar(objeto);
            return respuesta;
        }

        public async Task<RespuestaDTO> EliminarAnadidoServicio(int id)
        {
            return await _anadidoServicioService.Eliminar(id);
        }

        public async Task<string> GenerarBase64(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                byte[] imageBytes = memoryStream.ToArray();
                string base64String = Convert.ToBase64String(imageBytes);
                string fullBase64ForFront = $"data:{file.ContentType};base64,{base64String}";
                return fullBase64ForFront;
            }
        }

        public Task<ServicioDTO> ObtenerXId(int id)
        {
            return _servicioService.ObtenerXId(id);
        }
    }
}
