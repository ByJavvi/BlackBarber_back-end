using AutoMapper;
using BlackBarberAPI.Data;
using BlackBarberAPI.DTOs;
using BlackBarberAPI.Models;
using BlackBarberAPI.Services.Contratos;
using Microsoft.EntityFrameworkCore;

namespace BlackBarberAPI.Services
{
    public class RolService<T> : IRolService<T> where T : DbContext
    {
        private readonly IGenericRepository<Rol, BlackBarberContext> _repository;
        private readonly IMapper _mapper;

        public RolService(IGenericRepository<Rol, BlackBarberContext> repository, IMapper mapper)
        {
            _repository = repository; _mapper = mapper;
        }

        public async Task<List<RolDTO>> ObtenerTodos() => _mapper.Map<List<RolDTO>>(await _repository.ObtenerTodos());
        public async Task<RolDTO> ObtenerXId(int id) => _mapper.Map<RolDTO>(await _repository.Obtener(r => r.Id == id));
        public async Task<RolDTO> CrearYObtener(RolDTO objeto) => _mapper.Map<RolDTO>(await _repository.Crear(_mapper.Map<Rol>(objeto)));
        public async Task<RespuestaDTO> Editar(RolDTO objeto)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var objetoEncontrado = await _repository.Obtener(r => r.Id == objeto.Id);

            if (objetoEncontrado == null || objetoEncontrado.Id <= 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No se encontró el rol";
                return respuesta;
            }

            _mapper.Map(objeto, objetoEncontrado);

            respuesta.Estatus = await _repository.Editar(objetoEncontrado);
            respuesta.Descripcion = respuesta.Estatus ? "Rol actualizado" : "Error al actualizar";

            return respuesta;
        }

        public async Task<RespuestaDTO> Eliminar(int id)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var objetoEncontrado = await _repository.Obtener(r => r.Id == id);

            if (objetoEncontrado == null || objetoEncontrado.Id <= 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No se encontró el rol";
                return respuesta;
            }

            respuesta.Estatus = await _repository.Eliminar(objetoEncontrado);
            respuesta.Descripcion = respuesta.Estatus ? "Rol eliminado" : "Error al eliminar";

            return respuesta;
        }
    }
}
