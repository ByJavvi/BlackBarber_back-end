using AutoMapper;
using BlackBarberAPI.Data;
using BlackBarberAPI.DTOs;
using BlackBarberAPI.Models;
using BlackBarberAPI.Services.Contratos;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlackBarberAPI.Services
{
    public class CitaService<T> : ICitaService<T> where T : DbContext
    {
            private readonly IGenericRepository<Citum, BlackBarberContext> _repository;
            private readonly IMapper _mapper;

            public CitaService(IGenericRepository<Citum, BlackBarberContext> repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<List<CitaDTO>> ObtenerTodas()
            {
                var lista = await _repository.ObtenerTodos();
                return _mapper.Map<List<CitaDTO>>(lista);
            }

            public async Task<CitaDTO> ObtenerXId(int id)
            {
                var modelo = await _repository.Obtener(c => c.Id == id);
                return (modelo.Id != 0) ? _mapper.Map<CitaDTO>(modelo) : null;
            }

            public async Task<CitaDTO> CrearYObtener(CitaDTO objeto)
            {
                var modelo = _mapper.Map<Citum>(objeto);
                var creado = await _repository.Crear(modelo);
                return _mapper.Map<CitaDTO>(creado);
            }

            public async Task<RespuestaDTO> Editar(CitaDTO objeto)
            {
            RespuestaDTO respuesta = new RespuestaDTO();
            var objetoEncontrado = await _repository.Obtener(c=>c.Id == objeto.Id);
            if (objetoEncontrado == null || objetoEncontrado.Id <= 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No se encontró la cita";
                return respuesta;
            }
            objetoEncontrado.Estatus = objeto.Estatus;
            objetoEncontrado.FechaInicio = objeto.FechaInicio;
            objetoEncontrado.FechaTermino = objeto.FechaTermino;
                respuesta.Estatus = await _repository.Editar(objetoEncontrado);
                respuesta.Descripcion = respuesta.Estatus ? "Cita actualizada" : "Error al actualizar";
            return respuesta;
            }

            public async Task<RespuestaDTO> Eliminar(int id)
            {
            RespuestaDTO respuesta = new RespuestaDTO();
            var objetoEncontrado = await _repository.Obtener(c => c.Id == id);
            if (objetoEncontrado == null || objetoEncontrado.Id <= 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No se encontró la cita";
                return respuesta;
            }
            respuesta.Estatus = await _repository.Eliminar(objetoEncontrado);
            respuesta.Descripcion = respuesta.Estatus ? "Cita elimiada" : "Error al eliminar";
            return respuesta;
        }
        }
}
