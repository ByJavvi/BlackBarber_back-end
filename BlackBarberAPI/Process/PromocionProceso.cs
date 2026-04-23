using BlackBarberAPI.Data;
using BlackBarberAPI.DTOs;
using BlackBarberAPI.Services.Contratos;

namespace BlackBarberAPI.Process
{
    public class PromocionProceso
    {
        private readonly IPromocionService<BlackBarberContext> _promocionService;

        public PromocionProceso(IPromocionService<BlackBarberContext> promocionService)
        {
            this._promocionService = promocionService;
        }

        public async Task<List<PromocionDTO>> ObtenerTodasPromociones()
        {
            var lista = await _promocionService.ObtenerTodos();
            return lista;
        }

        public async Task<List<PromocionDTO>> ObtenerPromocionesVigentes()
        {
            var lista = await _promocionService.ObtenerTodos();
            var listaVigentes = lista.Where(p => p.FechaInicio <= DateOnly.FromDateTime(DateTime.Now) && p.FechaFin >= DateOnly.FromDateTime(DateTime.Now)).ToList();
            return listaVigentes;
        }

        public async Task<RespuestaDTO> CrearPromocion(PromocionDTO promocion)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var objetoCreado = await _promocionService.CrearYObtener(promocion);
            respuesta.Estatus = objetoCreado.Id > 0;
            respuesta.Descripcion = respuesta.Estatus ? "Promoción creada exitosamente." : "Error al crear la promoción.";
            return respuesta;
        }

        public async Task<RespuestaDTO> EditarPromocion(PromocionDTO promocion)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            respuesta = await _promocionService.Editar(promocion);
            return respuesta;
        }

        public async Task<RespuestaDTO> EliminarPromocion(int id)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            respuesta =  await _promocionService.Eliminar(id);
            return respuesta;
        }
    }
}
