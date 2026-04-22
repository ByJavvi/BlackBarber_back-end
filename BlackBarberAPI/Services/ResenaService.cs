using AutoMapper;
using BlackBarberAPI.Data;
using BlackBarberAPI.DTOs;
using BlackBarberAPI.Models;
using BlackBarberAPI.Services.Contratos;
using Microsoft.EntityFrameworkCore;

namespace BlackBarberAPI.Services
{
    public class ResenaService<T> : IResenaService<T> where T : DbContext
    {
        private readonly IGenericRepository<Resena, BlackBarberContext> _repository;
        private readonly IMapper _mapper;

        public ResenaService(IGenericRepository<Resena, BlackBarberContext> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ResenaDTO>> ObtenerTodos() => _mapper.Map<List<ResenaDTO>>(await _repository.ObtenerTodos());

        public async Task<List<ResenaDTO>> ObtenerXPerteneciente(int idPerteneciente)
        {
            var lista = await _repository.ObtenerTodos(r => r.IdCita == idPerteneciente);
            return _mapper.Map<List<ResenaDTO>>(lista);
        }

        public async Task<ResenaDTO> ObtenerXId(int id) => _mapper.Map<ResenaDTO>(await _repository.Obtener(r => r.Id == id));

        public async Task<ResenaDTO> CrearYObtener(ResenaDTO objeto) => _mapper.Map<ResenaDTO>(await _repository.Crear(_mapper.Map<Resena>(objeto)));

        public async Task<RespuestaDTO> Editar(ResenaDTO objeto)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var objetoEncontrado = await _repository.Obtener(r => r.Id == objeto.Id);

            if (objetoEncontrado == null || objetoEncontrado.Id <= 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No se encontró la reseña";
                return respuesta;
            }

            _mapper.Map(objeto, objetoEncontrado);

            respuesta.Estatus = await _repository.Editar(objetoEncontrado);
            respuesta.Descripcion = respuesta.Estatus ? "Reseña actualizada" : "Error al actualizar";

            return respuesta;
        }

        public async Task<RespuestaDTO> Eliminar(int id)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var objetoEncontrado = await _repository.Obtener(r => r.Id == id);

            if (objetoEncontrado == null || objetoEncontrado.Id <= 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No se encontró la reseña";
                return respuesta;
            }

            respuesta.Estatus = await _repository.Eliminar(objetoEncontrado);
            respuesta.Descripcion = respuesta.Estatus ? "Reseña eliminada" : "Error al eliminar";

            return respuesta;
        }
    }
}
