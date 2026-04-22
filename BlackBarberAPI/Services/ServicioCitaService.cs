using AutoMapper;
using BlackBarberAPI.Data;
using BlackBarberAPI.DTOs;
using BlackBarberAPI.Models;
using BlackBarberAPI.Services.Contratos;
using Microsoft.EntityFrameworkCore;

namespace BlackBarberAPI.Services
{
    public class ServicioCitaService<T> : IServicioCitaService<T> where T : DbContext
    {
        private readonly IGenericRepository<ServicioCitum, BlackBarberContext> _repository;
        private readonly IMapper _mapper;

        public ServicioCitaService(IGenericRepository<ServicioCitum, BlackBarberContext> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ServicioCitaDTO>> ObtenerXPerteneciente(int idPerteneciente)
        {
            var lista = await _repository.ObtenerTodos(sc => sc.IdCita == idPerteneciente);
            return _mapper.Map<List<ServicioCitaDTO>>(lista);
        }

        public async Task<ServicioCitaDTO> ObtenerXId(int id)
        {
            var modelo = await _repository.Obtener(sc => sc.Id == id);
            return _mapper.Map<ServicioCitaDTO>(modelo);
        }

        public async Task<ServicioCitaDTO> CrearYObtener(ServicioCitaDTO objeto)
        {
            var modelo = _mapper.Map<ServicioCitum>(objeto);
            var creado = await _repository.Crear(modelo);
            return _mapper.Map<ServicioCitaDTO>(creado);
        }

        public async Task<RespuestaDTO> Editar(ServicioCitaDTO objeto)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var objetoEncontrado = await _repository.Obtener(sc => sc.Id == objeto.Id);

            if (objetoEncontrado == null || objetoEncontrado.Id <= 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No se encontró la vinculación del servicio";
                return respuesta;
            }

            _mapper.Map(objeto, objetoEncontrado);

            respuesta.Estatus = await _repository.Editar(objetoEncontrado);
            respuesta.Descripcion = respuesta.Estatus ? "Servicio de la cita actualizado" : "Error al actualizar";

            return respuesta;
        }

        public async Task<RespuestaDTO> Eliminar(int id)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var objetoEncontrado = await _repository.Obtener(sc => sc.Id == id);

            if (objetoEncontrado == null || objetoEncontrado.Id <= 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No se encontró el registro";
                return respuesta;
            }

            respuesta.Estatus = await _repository.Eliminar(objetoEncontrado);
            respuesta.Descripcion = respuesta.Estatus ? "Registro eliminado" : "Error al eliminar";

            return respuesta;
        }
    }
}
