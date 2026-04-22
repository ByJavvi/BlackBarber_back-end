using AutoMapper;
using BlackBarberAPI.DTOs;
using BlackBarberAPI.Models;
using BlackBarberAPI.Services.Contratos;
using Microsoft.EntityFrameworkCore;

namespace BlackBarberAPI.Services
{
    public class BarberoService<T> : IBarberoService<T> where T : DbContext
    {
        private readonly IGenericRepository<Barbero, T> _repositorio;
        private readonly IMapper _Mapper;

        public BarberoService(IGenericRepository<Barbero, T> repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _Mapper = mapper;
        }

        public async Task<BarberoDTO> CrearYObtener(BarberoDTO objeto)
        {
            var modelo = _Mapper.Map<Barbero>(objeto);
            var objetoCreado = await _repositorio.Crear(modelo);
            return _Mapper.Map<BarberoDTO>(objetoCreado);
        }

        public async Task<RespuestaDTO> Editar(BarberoDTO objeto)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var objetoEncontrado = await _repositorio.Obtener(b=>b.Id == objeto.Id);
            if(objetoEncontrado == null || objetoEncontrado.Id <= 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No se encontró el barbero.";
                return respuesta;
            }
            objetoEncontrado.Nombre = objeto.Nombre;
            objetoEncontrado.Estatus = objeto.Estatus;
            respuesta.Estatus = await _repositorio.Editar(objetoEncontrado);
            respuesta.Descripcion = respuesta.Estatus ? "Barbero editado exitosamente." : "Ocurrió un problema al intentar editar el barbero.";
            return respuesta;
        }

        public async Task<RespuestaDTO> Eliminar(int id)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var objetoEncontrado = await _repositorio.Obtener(b => b.Id == id);
            if (objetoEncontrado == null || objetoEncontrado.Id <= 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No se encontró el barbero.";
                return respuesta;
            }
            respuesta.Estatus = await _repositorio.Eliminar(objetoEncontrado);
            respuesta.Descripcion = respuesta.Estatus ? "Barbero eliminado exitosamente." : "Ocurrió un problema al intentar eliminar el barbero.";
            return respuesta;
        }

        public async Task<List<BarberoDTO>> ObtenerTodos()
        {
            var lista = await _repositorio.ObtenerTodos();
            return _Mapper.Map<List<BarberoDTO>>(lista);
        }

        public async Task<BarberoDTO> ObtenerXId(int id)
        {
            var respuesta = await _repositorio.Obtener(b=>b.Id == id);
            return _Mapper.Map<BarberoDTO>(respuesta);
        }

        public async Task<BarberoDTO> ObtenerXIdUsuario(int idUsuario)
        {
            var barberoObtenido = await _repositorio.Obtener(b=>b.IdUsuario == idUsuario);
            return _Mapper.Map<BarberoDTO>(barberoObtenido);
        }
    }
}
