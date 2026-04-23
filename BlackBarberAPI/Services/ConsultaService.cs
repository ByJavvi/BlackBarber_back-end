using AutoMapper;
using BlackBarberAPI.DTOs;
using BlackBarberAPI.Models;
using BlackBarberAPI.Services.Contratos;
using Microsoft.EntityFrameworkCore;

namespace BlackBarberAPI.Services
{
    public class ConsultaService<T> : IConsultaService<T> where T : DbContext
    {
        private readonly IGenericRepository<Consultas, T> _repositorio;
        private readonly IMapper _Mapper;

        public ConsultaService(IGenericRepository<Consultas, T> repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _Mapper = mapper;
        }

        public async Task<ConsultaDTO> CrearConsulta(ConsultaDTO consulta)
        {
            var modelo = _Mapper.Map<Consultas>(consulta);
            var objetoCreado = await _repositorio.Crear(modelo);
            return _Mapper.Map<ConsultaDTO>(objetoCreado);
        }

        public async Task<RespuestaDTO> EditarConsulta(ConsultaDTO consulta)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var objetoEncontrado = await _repositorio.Obtener(c=>c.Id == consulta.Id);
            if(objetoEncontrado == null)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No se encontró la consulta.";
                return respuesta;
            }
            objetoEncontrado.Estatus = consulta.Estatus;
            respuesta.Estatus = await _repositorio.Editar(objetoEncontrado);
            respuesta.Descripcion = respuesta.Estatus ? "Consulta editada exitosamente." : "Ocurrió un error al intentar editar la consulta.";
            return respuesta;
        }

        public async Task<RespuestaDTO> EliminarConsulta(int id)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var objetoEncontrado = await _repositorio.Obtener(c => c.Id == id);
            if (objetoEncontrado == null)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No se encontró la consulta.";
                return respuesta;
            }
            respuesta.Estatus = await _repositorio.Eliminar(objetoEncontrado);
            respuesta.Descripcion = respuesta.Estatus ? "Consulta eliminada exitosamente." : "Ocurrió un error al intentar eliminar la consulta.";
            return respuesta;
        }

        public async Task<List<ConsultaDTO>> ObtenerTodos()
        {
            var lista = await _repositorio.ObtenerTodos();
            return _Mapper.Map<List<ConsultaDTO>>(lista);
        }

        public async Task<ConsultaDTO> ObtenerXId(int id)
        {
            var resultado = await _repositorio.Obtener(c=>c.Id == id);
            return _Mapper.Map<ConsultaDTO>(resultado);
        }
    }
}
