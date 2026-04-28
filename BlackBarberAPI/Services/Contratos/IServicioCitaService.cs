using BlackBarberAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BlackBarberAPI.Services.Contratos
{
    public interface IServicioCitaService<T> where T :  DbContext
    {
        Task<List<ServicioCitaDTO>> ObtenerXPerteneciente(int idPerteneciente);
        Task<List<ServicioCitaDTO>> ObtenerXIdServicio(int idServicio);
        Task<ServicioCitaDTO> ObtenerXId(int id);
        Task<ServicioCitaDTO> CrearYObtener(ServicioCitaDTO objeto);
        Task<RespuestaDTO> Editar(ServicioCitaDTO objeto);
        Task<RespuestaDTO> Eliminar(int id);
    }
}
