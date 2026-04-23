using BlackBarberAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BlackBarberAPI.Services.Contratos
{
    public interface IConsultaService<T> where T : DbContext
    {
        public Task<List<ConsultaDTO>> ObtenerTodos();
        public Task<ConsultaDTO> ObtenerXId(int id);
        public Task<ConsultaDTO> CrearConsulta(ConsultaDTO consulta);
        public Task<RespuestaDTO> EditarConsulta(ConsultaDTO consulta);
        public Task<RespuestaDTO> EliminarConsulta(int id);
    }
}
