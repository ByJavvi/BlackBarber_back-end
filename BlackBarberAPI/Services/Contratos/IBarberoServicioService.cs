using BlackBarberAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BlackBarberAPI.Services.Contratos
{
    public interface IBarberoServicioService<T> where T : DbContext
    {
        Task<List<BarberoServicioDTO>> ObtenerTodos();
        Task<BarberoServicioDTO> ObtenerXId(int id);
        Task<BarberoServicioDTO> CrearYObtener(BarberoServicioDTO objeto);
        Task<RespuestaDTO> Eliminar(int id);
    }
}
