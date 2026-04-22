using AutoMapper;
using BlackBarberAPI.Data;
using BlackBarberAPI.DTOs;
using BlackBarberAPI.Models;
using BlackBarberAPI.Services.Contratos;
using Microsoft.EntityFrameworkCore;

namespace BlackBarberAPI.Services
{
    public class PreferenciasClienteService<T> : IPreferenciasClienteService<T> where T : DbContext
    {
        private readonly IGenericRepository<PreferenciasCliente, BlackBarberContext> _repository;
        private readonly IMapper _mapper;

        public PreferenciasClienteService(IGenericRepository<PreferenciasCliente, BlackBarberContext> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PreferenciasClienteDTO> ObtenerXId(int id) => _mapper.Map<PreferenciasClienteDTO>(await _repository.Obtener(p => p.Id == id));

        public async Task<List<PreferenciasClienteDTO>> ObtenerXCliente(int idCliente)
        {
            var lista = await _repository.ObtenerTodos(p => p.IdCliente == idCliente);
            return _mapper.Map<List<PreferenciasClienteDTO>>(lista);
        }

        public async Task<PreferenciasClienteDTO> CrearYEditar(PreferenciasClienteDTO objeto)
        {
            var modelo = _mapper.Map<PreferenciasCliente>(objeto);
            if (modelo.Id != 0) await _repository.Editar(modelo);
            else await _repository.Crear(modelo);
            return _mapper.Map<PreferenciasClienteDTO>(modelo);
        }

        public async Task<RespuestaDTO> Editar(PreferenciasClienteDTO objeto)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var objetoEncontrado = await _repository.Obtener(p => p.Id == objeto.Id);

            if (objetoEncontrado == null || objetoEncontrado.Id <= 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No se encontraron preferencias para este cliente";
                return respuesta;
            }

            // Mapea preferencias como: tipo de corte favorito, bebida, barbero preferido, etc.
            _mapper.Map(objeto, objetoEncontrado);

            respuesta.Estatus = await _repository.Editar(objetoEncontrado);
            respuesta.Descripcion = respuesta.Estatus ? "Preferencias guardadas" : "Error al actualizar preferencias";

            return respuesta;
        }

        public async Task<RespuestaDTO> Eliminar(int id)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var objetoEncontrado = await _repository.Obtener(p => p.Id == id);

            if (objetoEncontrado == null || objetoEncontrado.Id <= 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "Preferencia no encontrada";
                return respuesta;
            }

            respuesta.Estatus = await _repository.Eliminar(objetoEncontrado);
            respuesta.Descripcion = respuesta.Estatus ? "Preferencia eliminada" : "Error al intentar eliminar";

            return respuesta;
        }
    }
}
