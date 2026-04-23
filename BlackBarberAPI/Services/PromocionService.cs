using AutoMapper;
using BlackBarberAPI.Data;
using BlackBarberAPI.DTOs;
using BlackBarberAPI.Models;
using BlackBarberAPI.Services.Contratos;
using Microsoft.EntityFrameworkCore;

namespace BlackBarberAPI.Services
{
    public class PromocionService<T> : IPromocionService<T> where T : DbContext
    {
        private readonly IGenericRepository<Promocion, BlackBarberContext> _repository;
        private readonly IMapper _mapper;

        public PromocionService(IGenericRepository<Promocion, BlackBarberContext> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PromocionDTO>> ObtenerTodos()
        {
            // Solo promociones vigentes
            var hoy = DateTime.Now.Date;
            var lista = await _repository.ObtenerTodos();
            return _mapper.Map<List<PromocionDTO>>(lista);
        }

        public async Task<PromocionDTO> ObtenerXId(int id) => _mapper.Map<PromocionDTO>(await _repository.Obtener(p => p.Id == id));

        public async Task<PromocionDTO> CrearYObtener(PromocionDTO objeto) => _mapper.Map<PromocionDTO>(await _repository.Crear(_mapper.Map<Promocion>(objeto)));

        public async Task<RespuestaDTO> Editar(PromocionDTO objeto)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var objetoEncontrado = await _repository.Obtener(p => p.Id == objeto.Id);

            if (objetoEncontrado == null || objetoEncontrado.Id <= 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No se encontró la promoción";
                return respuesta;
            }

            objetoEncontrado.Descripcion = objeto.Descripcion;
            objetoEncontrado.FechaInicio = objeto.FechaInicio;
            objetoEncontrado.FechaFin = objeto.FechaFin;

            respuesta.Estatus = await _repository.Editar(objetoEncontrado);
            respuesta.Descripcion = respuesta.Estatus ? "Promoción actualizada" : "Error al actualizar la promoción";

            return respuesta;
        }

        public async Task<RespuestaDTO> Eliminar(int id)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var objetoEncontrado = await _repository.Obtener(p => p.Id == id);

            if (objetoEncontrado == null || objetoEncontrado.Id <= 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No se encontró la promoción";
                return respuesta;
            }

            respuesta.Estatus = await _repository.Eliminar(objetoEncontrado);
            respuesta.Descripcion = respuesta.Estatus ? "Promoción eliminada" : "Error al eliminar";

            return respuesta;
        }
    }
}
