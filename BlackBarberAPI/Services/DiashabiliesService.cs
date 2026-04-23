using AutoMapper;
using BlackBarberAPI.DTOs;
using BlackBarberAPI.Models;
using BlackBarberAPI.Services.Contratos;
using Microsoft.EntityFrameworkCore;

namespace BlackBarberAPI.Services
{
    public class DiashabiliesService<T> : IDiasHabilesService<T> where T : DbContext
    {
        private readonly IGenericRepository<DiasHabiles, T> _repositorio;
        private readonly IMapper _Mapper;

        public DiashabiliesService(IGenericRepository<DiasHabiles, T> repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _Mapper = mapper;
        }

        public async Task<RespuestaDTO> EditarDiaHabil(DiasHabilDTO objeto)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var objetoEncontrado = await _repositorio.Obtener(d=>d.Id == objeto.Id);
            if(objetoEncontrado == null)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No se encontró el registro";
                return respuesta;
            }
            objetoEncontrado.Habil = objeto.Habil;
            objetoEncontrado.HoraInicio = objeto.HoraInicio;
            objetoEncontrado.HoraFin = objeto.HoraFin;
            respuesta.Estatus = await _repositorio.Editar(objetoEncontrado);
            respuesta.Descripcion = respuesta.Estatus ? "Día editado exitosamente" : "Ocurrió un problema al intentar editar el día";
            return respuesta;
        }

        public async Task<List<DiasHabilDTO>> ObtenerTodos()
        {
            var lista = await _repositorio.ObtenerTodos();
            return _Mapper.Map<List<DiasHabilDTO>>(lista);
        }

        public async Task<DiasHabilDTO> ObtenerXId(int id)
        {
            var resultado = await _repositorio.Obtener(d=>d.Id  == id);
            return _Mapper.Map<DiasHabilDTO>(resultado);
        }
    }
}
