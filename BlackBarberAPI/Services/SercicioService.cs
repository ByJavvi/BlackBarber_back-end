using AutoMapper;
using BlackBarberAPI.Data;
using BlackBarberAPI.DTOs;
using BlackBarberAPI.Models;
using BlackBarberAPI.Services.Contratos;
using Microsoft.EntityFrameworkCore;

namespace BlackBarberAPI.Services
{
    public class SercicioService<T> : IServicioService<T> where T : DbContext
    {
        private readonly IGenericRepository<Servicio, BlackBarberContext> _repository;
        private readonly IMapper _mapper;

        public SercicioService(IGenericRepository<Servicio, BlackBarberContext> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ServicioDTO>> ObtenerTodos()
        {
            var lista = await _repository.ObtenerTodos();
            return _mapper.Map<List<ServicioDTO>>(lista);
        }

        public async Task<ServicioDTO> ObtenerXId(int id)
        {
            var modelo = await _repository.Obtener(s => s.Id == id);
            return (modelo.Id != 0) ? _mapper.Map<ServicioDTO>(modelo) : null;
        }

        public async Task<ServicioDTO> CrearYObtener(ServicioDTO objeto)
        {
            var modelo = _mapper.Map<Servicio>(objeto);
            var creado = await _repository.Crear(modelo);
            return _mapper.Map<ServicioDTO>(creado);
        }

        public async Task<RespuestaDTO> Editar(ServicioDTO objeto)
        {
            var objetoEncontrado = await _repository.Obtener(s => s.Id == objeto.Id);

            if (objetoEncontrado.Id <= 0)
                return new RespuestaDTO { Estatus = false, Descripcion = "Servicio no encontrado" };

            objetoEncontrado.Nombre = objeto.Nombre;
            objetoEncontrado.Descripcion = objeto.Descripcion;
            objetoEncontrado.IdTipo = objeto.IdTipo;
            objetoEncontrado.PrecioBase = objeto.PrecioBase;
            objetoEncontrado.Estatus = objeto.Estatus;

            bool actualizado = await _repository.Editar(objetoEncontrado);
            return new RespuestaDTO { Estatus = actualizado, Descripcion = actualizado ? "Servicio actualizado" : "Error" };
        }

        public async Task<RespuestaDTO> Eliminar(int id)
        {
            var objetoEncontrado = await _repository.Obtener(s => s.Id == id);

            if (objetoEncontrado.Id <= 0)
                return new RespuestaDTO { Estatus = false, Descripcion = "Servicio no encontrado" };

            bool eliminado = await _repository.Eliminar(objetoEncontrado);
            return new RespuestaDTO { Estatus = eliminado, Descripcion = eliminado ? "Servicio eliminado" : "Error" };
        }
    }
}
