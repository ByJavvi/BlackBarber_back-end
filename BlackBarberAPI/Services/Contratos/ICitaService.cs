using BlackBarberAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BlackBarberAPI.Services.Contratos
{
    public interface ICitaService<T> where T :  DbContext
    {
        Task<List<CitaDTO>> ObtenerTodas();
        Task<List<CitaDTO>> ObtenerCitasVigentes();
        Task<CitaDTO> ObtenerXId(int id);
        Task<CitaDTO> CrearYObtener(CitaDTO objeto);
        Task<RespuestaDTO> Editar(CitaDTO objeto);
        Task<RespuestaDTO> Eliminar(int id);
    }
}
