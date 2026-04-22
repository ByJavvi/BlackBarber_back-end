using AutoMapper;
using BlackBarberAPI.Data;
using BlackBarberAPI.DTOs;
using BlackBarberAPI.Models;
using BlackBarberAPI.Services.Contratos;
using Microsoft.EntityFrameworkCore;

namespace BlackBarberAPI.Services
{
    public class PerfumeService<T> : IPerfumeService<T> where T : DbContext
    {
        private readonly IGenericRepository<Perfume, BlackBarberContext> _repository;
        private readonly IMapper _mapper;

        public PerfumeService(IGenericRepository<Perfume, BlackBarberContext> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PerfumeDTO>> ObtenerTodos()
        {
            var lista = await _repository.ObtenerTodos(p => p.Disponible == true);
            return _mapper.Map<List<PerfumeDTO>>(lista);
        }

        public async Task<PerfumeDTO> ObtenerXId(int id)
        {
            var modelo = await _repository.Obtener(p => p.Id == id);
            return (modelo.Id != 0) ? _mapper.Map<PerfumeDTO>(modelo) : null;
        }

        public async Task<PerfumeDTO> CrearYObtener(PerfumeDTO objeto)
        {
            var modelo = _mapper.Map<Perfume>(objeto);
            var creado = await _repository.Crear(modelo);
            return _mapper.Map<PerfumeDTO>(creado);
        }

        public async Task<RespuestaDTO> Editar(PerfumeDTO objeto)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var objetoEncontrado = await _repository.Obtener(p => p.Id == objeto.Id);

            if (objetoEncontrado == null || objetoEncontrado.Id <= 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "Producto no encontrado";
                return respuesta;
            }

            // Actualiza automáticamente Nombre, Precio, Stock, etc.
            _mapper.Map(objeto, objetoEncontrado);

            respuesta.Estatus = await _repository.Editar(objetoEncontrado);
            respuesta.Descripcion = respuesta.Estatus ? "Perfume actualizado" : "Error al guardar cambios";

            return respuesta;
        }

        public async Task<RespuestaDTO> Eliminar(int id)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var objetoEncontrado = await _repository.Obtener(p => p.Id == id);

            if (objetoEncontrado == null || objetoEncontrado.Id <= 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "El perfume no existe";
                return respuesta;
            }

            respuesta.Estatus = await _repository.Eliminar(objetoEncontrado);
            respuesta.Descripcion = respuesta.Estatus ? "Perfume removido del catálogo" : "Error al eliminar";

            return respuesta;
        }
    }
}
