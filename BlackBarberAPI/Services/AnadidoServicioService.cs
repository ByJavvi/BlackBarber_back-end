using AutoMapper;
using BlackBarberAPI.DTOs;
using BlackBarberAPI.Models;
using BlackBarberAPI.Services.Contratos;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlackBarberAPI.Services
{
    public class AnadidoServicioService<T> : IAnadidoServicioService<T> where T : DbContext
    {
        private readonly IGenericRepository<AnadidoServicio, T> _repositorio;
        private readonly IMapper _Mapper;

        public AnadidoServicioService(IGenericRepository<AnadidoServicio, T> repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _Mapper = mapper;
        }

        public async Task<AnadidoServicioDTO> CrearYObtener(AnadidoServicioDTO objeto)
        {
            var modelo = _Mapper.Map<AnadidoServicio>(objeto);
            var objetoCreado = await _repositorio.Crear(modelo);
            return _Mapper.Map<AnadidoServicioDTO>(objetoCreado);
        }

        public async Task<RespuestaDTO> Editar(AnadidoServicioDTO objeto)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var objetoEncontrado = await _repositorio.Obtener(a=>a.Id== objeto.Id);
            if(objetoEncontrado.Id<=0 || objetoEncontrado == null)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No se encontro el añadido.";
                return respuesta;
            }
            objetoEncontrado.Descripcion= objeto.Descripcion;
            objetoEncontrado.Nombre = objeto.Nombre;
            objetoEncontrado.Precio = objeto.Precio;
            respuesta.Estatus = await _repositorio.Editar(objetoEncontrado);
            respuesta.Descripcion = respuesta.Estatus ? "Añadido editado exitosamente." : "Ocurrió un error al intentar editar el añadido.";
            return respuesta;
        }

        public async Task<RespuestaDTO> Eliminar(int id)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var objetoEncontrado = await _repositorio.Obtener(a => a.Id == id);
            if (objetoEncontrado.Id <= 0 || objetoEncontrado == null)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No se encontro el añadido.";
                return respuesta;
            }
            respuesta.Estatus = await _repositorio.Eliminar(objetoEncontrado);
            respuesta.Descripcion = respuesta.Estatus ? "Añadido eliminado exitosamente." : "Ocurrió un error al intentar eliminar el añadido.";
            return respuesta;
        }

        public async Task<AnadidoServicioDTO> ObtenerXId(int id)
        {
            var respuesta = await _repositorio.Obtener(a => a.Id == id);
            if(respuesta == null)
            {
                return new AnadidoServicioDTO();
            }
            return _Mapper.Map<AnadidoServicioDTO>(respuesta);
        }

        public async Task<List<AnadidoServicioDTO>> ObtenerXPerteneciente(int idPerteneciente)
        {
            var lista = await _repositorio.ObtenerTodos(a=>a.IdPerteneciente == idPerteneciente);
            return _Mapper.Map<List<AnadidoServicioDTO>>(lista);
        }
    }
}
