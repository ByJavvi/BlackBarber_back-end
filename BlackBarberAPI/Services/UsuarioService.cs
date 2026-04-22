using AutoMapper;
using BlackBarberAPI.Data;
using BlackBarberAPI.DTOs;
using BlackBarberAPI.Models;
using BlackBarberAPI.Services.Contratos;
using Microsoft.EntityFrameworkCore;

namespace BlackBarberAPI.Services
{
    public class UsuarioService<T> : IUsuarioService<T> where T : DbContext
    {
        private readonly IGenericRepository<Usuario, BlackBarberContext> _repository;
        private readonly IMapper _mapper;

        public UsuarioService(IGenericRepository<Usuario, BlackBarberContext> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<UsuarioDTO>> ObtenerTodos()
        {
            var lista = await _repository.ObtenerTodos();
            return _mapper.Map<List<UsuarioDTO>>(lista);
        }

        public async Task<UsuarioDTO> ObtenerXId(int id)
        {
            var modelo = await _repository.Obtener(u => u.Id == id);
            return (modelo.Id != 0) ? _mapper.Map<UsuarioDTO>(modelo) : new UsuarioDTO();
        }

        public async Task<UsuarioDTO> Login(string correo, string password)
        {
            // Buscamos el usuario por correo y contraseña (idealmente usarás un Hash después)
            var usuario = await _repository.Obtener(u => u.Correo == correo && u.PasswordHash == password && u.Estatus == 1);

            if (usuario.Id == 0) return null; // Si no existe o credenciales incorrectas

            return _mapper.Map<UsuarioDTO>(usuario);
        }

        public async Task<UsuarioDTO> CrearYObtener(UsuarioDTO objeto)
        {
            // Validamos si el correo ya existe antes de crear
            var existe = await _repository.Obtener(u => u.Correo == objeto.Correo);
            if (existe.Id != 0) return null;

            var modelo = _mapper.Map<Usuario>(objeto);
            modelo.IdRol = modelo.IdRol ?? 2; // Por defecto es 'Cliente' si no se especifica

            var creado = await _repository.Crear(modelo);
            return _mapper.Map<UsuarioDTO>(creado);
        }

        public async Task<RespuestaDTO> Editar(UsuarioDTO objeto)
        {
            var objetoEncontrado = await _repository.Obtener(u => u.Id == objeto.Id);

            if (objetoEncontrado.Id <= 0)
                return new RespuestaDTO { Estatus = false, Descripcion = "Usuario no encontrado" };
            objetoEncontrado.Estatus = objeto.Estatus;
            objetoEncontrado.IdRol = objeto.IdRol;
            objetoEncontrado.PasswordHash = objeto.PasswordHash;
            var modelo = _mapper.Map<Usuario>(objetoEncontrado);

            bool actualizado = await _repository.Editar(objetoEncontrado);

            return new RespuestaDTO
            {
                Estatus = actualizado,
                Descripcion = actualizado ? "Usuario actualizado" : "Error al actualizar"
            };
        }

        public async Task<RespuestaDTO> Eliminar(int id)
        {
            var objetoEncontrado = await _repository.Obtener(u => u.Id == id);

            if (objetoEncontrado.Id <= 0)
                return new RespuestaDTO { Estatus = false, Descripcion = "Usuario no encontrado" };

            bool eliminado = await _repository.Eliminar(objetoEncontrado);

            return new RespuestaDTO
            {
                Estatus = eliminado,
                Descripcion = eliminado ? "Usuario eliminado" : "Error al eliminar"
            };
        }

        public async Task<UsuarioDTO> ObtenerXCorreo(string correo)
        {
            var modelo = await _repository.Obtener(u => u.Correo == correo);
            return (modelo.Id != 0) ? _mapper.Map<UsuarioDTO>(modelo) : new UsuarioDTO();
        }
    }
}
