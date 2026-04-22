using BlackBarberAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BlackBarberAPI.Services.Contratos
{
    public interface IUsuarioService<T> where T :  DbContext
    {
        Task<List<UsuarioDTO>> ObtenerTodos();
        Task<UsuarioDTO> ObtenerXId(int id);
        Task<UsuarioDTO> ObtenerXCorreo(string correo);
        Task<UsuarioDTO> CrearYObtener(UsuarioDTO objeto);
        Task<RespuestaDTO> Editar(UsuarioDTO objeto);
    }
}
