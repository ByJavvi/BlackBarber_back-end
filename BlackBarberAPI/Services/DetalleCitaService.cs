using AutoMapper;
using BlackBarberAPI.Data;
using BlackBarberAPI.DTOs;
using BlackBarberAPI.Models;
using BlackBarberAPI.Services.Contratos;
using Microsoft.EntityFrameworkCore;

namespace BlackBarberAPI.Services
{
    public class DetalleCitaService<T> : IDetalleCitaService<T> where T : DbContext
    {
        private readonly IGenericRepository<DetalleCitum, BlackBarberContext> _repository;
        private readonly IMapper _mapper;

        public DetalleCitaService(IGenericRepository<DetalleCitum, BlackBarberContext> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<DetalleCitaDTO>> ObtenerXPerteneciente(int idPerteneciente)
        {
            var lista = await _repository.ObtenerTodos(d => d.IdServicioCita == idPerteneciente);
            return _mapper.Map<List<DetalleCitaDTO>>(lista);
        }

        public async Task<DetalleCitaDTO> ObtenerXId(int id) => _mapper.Map<DetalleCitaDTO>(await _repository.Obtener(d => d.Id == id));

        public async Task<DetalleCitaDTO> CrearYObtener(DetalleCitaDTO objeto) => _mapper.Map<DetalleCitaDTO>(await _repository.Crear(_mapper.Map<DetalleCitum>(objeto)));

        public async Task<RespuestaDTO> Editar(DetalleCitaDTO objeto)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            // Tu lógica de validación con el ID del genérico
            var objetoEncontrado = await _repository.Obtener(d => d.Id == objeto.Id);

            if (objetoEncontrado == null || objetoEncontrado.Id <= 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No se encontró el detalle de la cita";
                return respuesta;
            }

            // AutoMapper sincroniza los cambios del DTO al Modelo encontrado
            _mapper.Map(objeto, objetoEncontrado);

            respuesta.Estatus = await _repository.Editar(objetoEncontrado);
            respuesta.Descripcion = respuesta.Estatus ? "Detalle actualizado" : "Error al actualizar el detalle";

            return respuesta;
        }

        public async Task<RespuestaDTO> Eliminar(int id)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var objetoEncontrado = await _repository.Obtener(d => d.Id == id);

            if (objetoEncontrado == null || objetoEncontrado.Id <= 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No se encontró el detalle";
                return respuesta;
            }

            respuesta.Estatus = await _repository.Eliminar(objetoEncontrado);
            respuesta.Descripcion = respuesta.Estatus ? "Detalle eliminado" : "Error al eliminar";

            return respuesta;
        }
    }
}
