using AutoMapper;
using BlackBarberAPI.Data;
using BlackBarberAPI.DTOs;
using BlackBarberAPI.Models;
using BlackBarberAPI.Services.Contratos;
using Microsoft.EntityFrameworkCore;

namespace BlackBarberAPI.Services
{
    public class TipoServicioService<T> : ITipoServicioService<T> where T : DbContext
    {
        private readonly IGenericRepository<TipoServicio, BlackBarberContext> _repository;
        private readonly IMapper _mapper;

        public TipoServicioService(IGenericRepository<TipoServicio, BlackBarberContext> repository, IMapper mapper)
        {
            _repository = repository; _mapper = mapper;
        }

        public async Task<List<TipoServicioDTO>> ObtenerTodos() => _mapper.Map<List<TipoServicioDTO>>(await _repository.ObtenerTodos());
        public async Task<TipoServicioDTO> ObtenerXId(int id) => _mapper.Map<TipoServicioDTO>(await _repository.Obtener(t => t.Id == id));
        public async Task<TipoServicioDTO> CrearYObtener(TipoServicioDTO objeto) => _mapper.Map<TipoServicioDTO>(await _repository.Crear(_mapper.Map<TipoServicio>(objeto)));
        public async Task<RespuestaDTO> Editar(TipoServicioDTO objeto)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var objetoEncontrado = await _repository.Obtener(ts => ts.Id == objeto.Id);

            if (objetoEncontrado == null || objetoEncontrado.Id <= 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No se encontró el tipo de servicio";
                return respuesta;
            }

            _mapper.Map(objeto, objetoEncontrado);

            respuesta.Estatus = await _repository.Editar(objetoEncontrado);
            respuesta.Descripcion = respuesta.Estatus ? "Tipo de servicio actualizado" : "Error al actualizar";

            return respuesta;
        }

        public async Task<RespuestaDTO> Eliminar(int id)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var objetoEncontrado = await _repository.Obtener(ts => ts.Id == id);

            if (objetoEncontrado == null || objetoEncontrado.Id <= 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No se encontró el tipo de servicio";
                return respuesta;
            }

            respuesta.Estatus = await _repository.Eliminar(objetoEncontrado);
            respuesta.Descripcion = respuesta.Estatus ? "Tipo de servicio eliminado" : "Error al eliminar";

            return respuesta;
        }
    }
}
