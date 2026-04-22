using BlackBarberAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BlackBarberAPI.Services.Contratos
{
    public interface IBarberoService<T> where T : DbContext
    {
        Task<List<BarberoDTO>> ObtenerTodos();
        Task<BarberoDTO> ObtenerXId(int id);
        Task<BarberoDTO> ObtenerXIdUsuario(int idUsuario);
        Task<BarberoDTO> CrearYObtener(BarberoDTO objeto);
        Task<RespuestaDTO> Editar(BarberoDTO objeto);
        Task<RespuestaDTO> Eliminar(int id);

    }
}
