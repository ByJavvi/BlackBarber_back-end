using BlackBarberAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BlackBarberAPI.Services.Contratos
{
    public interface IDiasHabilesService<T> where T :  DbContext
    {
        public Task<List<DiasHabilDTO>> ObtenerTodos();
        public Task<DiasHabilDTO> ObtenerXId(int id);
        public Task<RespuestaDTO> EditarDiaHabil(DiasHabilDTO objeto);
    }
}
