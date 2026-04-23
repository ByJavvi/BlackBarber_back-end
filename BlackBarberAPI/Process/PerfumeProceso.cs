using BlackBarberAPI.Data;
using BlackBarberAPI.DTOs;
using BlackBarberAPI.Models;
using BlackBarberAPI.Services.Contratos;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BlackBarberAPI.Process
{
    public class PerfumeProceso
    {
        private readonly IPerfumeService<BlackBarberContext> _perfumeService;

        public PerfumeProceso(IPerfumeService<BlackBarberContext> perfumeService)
        {
            _perfumeService = perfumeService;
        }

        public async Task<List<PerfumeDTO>> ObtenerPerfumes()
        {
            var lista = await _perfumeService.ObtenerTodos();
            return lista;
        }

        public async Task<RespuestaDTO> CrearPerfume(PerfumeConArchivoDTO objeto)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            PerfumeDTO perfume = new PerfumeDTO
            {
                Nombre = objeto.Nombre,
                Descripcion = objeto.Descripcion,
                Disponible = objeto.Disponible
            };
            if (objeto.Archivo != null)
            {
                var base64 = await GenerarBase64(objeto.Archivo);
                perfume.Base64 = base64;
            }

            var objetoCreado = await _perfumeService.CrearYObtener(perfume);
            respuesta.Estatus = objetoCreado.Id > 0;
            respuesta.Descripcion = objetoCreado.Id > 0 ? "Perfume creado exitosamente." : "Error al crear el perfume.";
            return respuesta;
        }

        public async Task<RespuestaDTO> EditarPerfume(PerfumeConArchivoDTO objeto)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            PerfumeDTO perfume = new PerfumeDTO
            {
                Id = objeto.Id,
                Nombre = objeto.Nombre,
                Descripcion = objeto.Descripcion,
                Disponible = objeto.Disponible
            };
            if (objeto.Archivo != null)
            {
                var base64 = await GenerarBase64(objeto.Archivo);
                perfume.Base64 = base64;
            }
            respuesta = await _perfumeService.Editar(perfume);
            return respuesta;
        }

        public async Task<RespuestaDTO> EliminarPerfume(int id)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            respuesta = await _perfumeService.Eliminar(id);
            return respuesta;
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
    }
}
