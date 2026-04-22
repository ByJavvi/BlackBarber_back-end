using AutoMapper;
using BlackBarberAPI.Data;
using BlackBarberAPI.DTOs;
using BlackBarberAPI.Models;
using BlackBarberAPI.Services.Contratos;
using Microsoft.EntityFrameworkCore;

namespace BlackBarberAPI.Services
{
    public class BarberoServicioService<T> : IBarberoServicioService<T> where T : DbContext
    {
        private readonly IGenericRepository<BarberoServicio, BlackBarberContext> _repository;
        private readonly IMapper _mapper;

        public BarberoServicioService(IGenericRepository<BarberoServicio, BlackBarberContext> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<BarberoServicioDTO>> ObtenerXPerteneciente(int idPerteneciente)
        {
            // En este caso, idPerteneciente suele ser el IdBarbero
            var lista = await _repository.ObtenerTodos(bs => bs.IdBarbero == idPerteneciente);
            return _mapper.Map<List<BarberoServicioDTO>>(lista);
        }

        public async Task<BarberoServicioDTO> ObtenerXId(int id) => _mapper.Map<BarberoServicioDTO>(await _repository.Obtener(bs => bs.Id == id));

        public async Task<BarberoServicioDTO> CrearYObtener(BarberoServicioDTO objeto)
        {
            var modelo = _mapper.Map<BarberoServicio>(objeto);
            var creado = await _repository.Crear(modelo);
            return _mapper.Map<BarberoServicioDTO>(creado);
        }

        public async Task<RespuestaDTO> Eliminar(int id)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var objetoEncontrado = await _repository.Obtener(bs => bs.Id == id);
            if (objetoEncontrado == null || objetoEncontrado.Id <= 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No se encontró el registro.";
                return respuesta;
            }
            respuesta.Estatus = await _repository.Eliminar(objetoEncontrado);
            respuesta.Descripcion = respuesta.Estatus ? "Asignación actualizada" : "Error";
            return respuesta;
        }

        public async Task<List<BarberoServicioDTO>> ObtenerTodos()
        {
            var lista = await _repository.ObtenerTodos();
            return _mapper.Map<List<BarberoServicioDTO>>(lista);
        }
    }
}
