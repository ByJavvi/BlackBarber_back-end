using BlackBarberAPI.Data;
using BlackBarberAPI.DTOs;
using BlackBarberAPI.Services.Contratos;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlackBarberAPI.Process
{
    public class UsuarioProceso
    {
        private readonly IUsuarioService<BlackBarberContext> _usuarioService;
        private readonly IRolService<BlackBarberContext> _rolService;
        private readonly PasswordEncrtyption _passwordEncryption;
        private readonly IBarberoService<BlackBarberContext> _barberoService;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        public UsuarioProceso(IUsuarioService<BlackBarberContext> usuarioService, 
            PasswordEncrtyption passwordEncrtyption, 
            IRolService<BlackBarberContext> rolService, 
            IBarberoService<BlackBarberContext> barberoService,
            IConfiguration configuration,
            IEmailService emailService
            )
        {
            _usuarioService = usuarioService;
            _passwordEncryption = passwordEncrtyption;
            _rolService = rolService;
            _barberoService = barberoService;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<RespuestaDTO> CrearUsuario(UsuarioCreacionDTO usuario)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var usuarios = await _usuarioService.ObtenerTodos();
            if (string.IsNullOrWhiteSpace(usuario.Contrasena))
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "Los datos no pueden ir vacíos.";
                return respuesta;
            }
            var usuarioExistente = usuarios.Where(u=>u.Username == usuario.Username).ToList();
            if (usuarioExistente.Count > 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "Ya existe un usuario con ese nombre de usuario";
                return respuesta;
            }
            usuarioExistente = new List<UsuarioDTO>();
            usuarioExistente = usuarios.Where(u=>u.Correo == usuario.Correo).ToList();
            if (usuarioExistente.Count > 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "Ya existe un usuario con ese correo electrónico";
                return respuesta;
            }
            UsuarioDTO usuarioCrear = new UsuarioDTO
            {
                Id = 0,
                Username = usuario.Username,
                Correo = usuario.Correo,
                Estatus = 1,
                IdRol = 2,
            };
            //usuario.HoraCreacion = DateTime.Now;
            usuarioCrear.PasswordHash = _passwordEncryption.HashPassword(usuario.Contrasena);
            UsuarioDTO resultado = await _usuarioService.CrearYObtener(usuarioCrear);
            respuesta.Estatus = resultado.Id > 0;
            respuesta.Descripcion = respuesta.Estatus ? "Usuario creado exitosamente." : "Ocurrió un error al intentar editar el usuario.";
            return respuesta;
        }

        public async Task<RespuestaDTO> EditarUsuario(UsuarioEdicionDTO objeto)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            UsuarioDTO usuario = new UsuarioDTO
            {
                Id = objeto.Id,
                Username = objeto.Username,
                Correo = objeto.Correo,
                Estatus = objeto.Estatus,
            };
            var barberoAsociado = await _barberoService.ObtenerXIdUsuario(usuario.Id);
            if (barberoAsociado != null)
            {
                usuario.IdRol = 3;
            }
            var usuarioActual = await _usuarioService.ObtenerXId(usuario.Id);
            if(usuarioActual == null || usuarioActual.Id <= 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No se encontró el usuario";
                return respuesta;
            }
            if (usuarioActual.Username != usuario.Username)
            {
                var usuariosExistentes = await _usuarioService.ObtenerTodos();
                if(usuariosExistentes.Where(u=>u.Id!=usuario.Id && u.Username == usuario.Username).ToList().Count() > 0)
                {
                    respuesta.Estatus = false;
                    respuesta.Descripcion = "Ya existe un usuario con ese nombre de usuario";
                    return respuesta;
                }
            }
            usuario.PasswordHash = usuarioActual.PasswordHash;
            respuesta = await _usuarioService.Editar(usuario);
            return respuesta;
        }

        public async Task<List<UsuarioDTO>> ObtenerListadoUsuarios()
        {
            var lista = await _usuarioService.ObtenerTodos();
            lista = lista.Where(u => u.IdRol != 1).ToList();
            return lista;
        }

        public async Task<RespuestaAutenticacionDTO> IniciarSesion(CredencialesUsuarioDTO credenciales)
        {
            RespuestaAutenticacionDTO respuesta = new RespuestaAutenticacionDTO();
            if(string.IsNullOrWhiteSpace(credenciales.Email)|| string.IsNullOrWhiteSpace(credenciales.Contrasena))
            {
                respuesta.Estatus = false;
                respuesta.Token = "No se pueden dejar campos vacíos.";
                return respuesta;
            }
            var usuarios = await _usuarioService.ObtenerTodos();
            var usuario = usuarios.Where(u => u.Correo == credenciales.Email).FirstOrDefault();
            if (usuario==null)
            {
                respuesta.Estatus = false;
                respuesta.Token = "No hay un usuario registrado con ese correo.";
                return respuesta;
            }
            if(usuario.Estatus == 0)
            {
                respuesta.Estatus = false;
                respuesta.Token = "El usuario está inactivo.";
                return respuesta;
            }
            var contrasenaCorrecta = _passwordEncryption.VerifyPassword(credenciales.Contrasena, usuario.PasswordHash);
            if (!contrasenaCorrecta)
            {
                respuesta.Estatus = false;
                respuesta.Token = "La contraseña es incorrecta.";
                return respuesta;
            }
            respuesta = await ConstruirToken(usuario);
            return respuesta;
        }

        public async Task<RespuestaAutenticacionDTO> ConstruirToken(UsuarioDTO usuario)
        {
            RespuestaAutenticacionDTO respuesta = new RespuestaAutenticacionDTO();
            var zvClaims = new List<Claim>()
            {
                new Claim("username", usuario.Username),
                new Claim("email", usuario.Correo),
                new Claim("id", usuario.Id.ToString()),
                new Claim("activo","1")
            };
            var rol = await _rolService.ObtenerXId((int)usuario.IdRol);
            if(rol == null)
            {
                respuesta.Estatus = false;
                respuesta.Token = "No se encontró el rol del usuario.";
                return respuesta;
            }
            var barbero = await _barberoService.ObtenerXIdUsuario(usuario.Id);
            if (barbero != null && barbero.Id>0)
            {
                zvClaims.Add(new Claim("role", "Barbero"));
            }
            else if (rol.Nombre == "Administrador")
            {
                zvClaims.Add(new Claim("role", "Administrador"));
            }
            else
            {
                zvClaims.Add(new Claim("role", "Cliente"));
            }
            var zvLlave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["llavejwt"]!));
            var zvCreds = new SigningCredentials(zvLlave, SecurityAlgorithms.HmacSha256);
            var zvExpiracion = DateTime.UtcNow.AddHours(5);
            var zvToken = new JwtSecurityToken(issuer: null, audience: null, claims: zvClaims, expires: zvExpiracion, signingCredentials: zvCreds);
            respuesta.Token = new JwtSecurityTokenHandler().WriteToken(zvToken);
            respuesta.Estatus = true;
            return respuesta;
        }

        public async Task<RespuestaAutenticacionDTO> ConstruirTokenRecuperacion(string correo)
        {
            RespuestaAutenticacionDTO respuesta = new RespuestaAutenticacionDTO();
            var zvClaims = new List<Claim>()
            {
                new Claim("email", correo),
            };
            var zvLlave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["llavejwt"]!));
            var zvCreds = new SigningCredentials(zvLlave, SecurityAlgorithms.HmacSha256);
            var zvExpiracion = DateTime.UtcNow.AddHours(2);
            var zvToken = new JwtSecurityToken(issuer: null, audience: null, claims: zvClaims, expires: zvExpiracion, signingCredentials: zvCreds);
            respuesta.Token = new JwtSecurityTokenHandler().WriteToken(zvToken);
            respuesta.Estatus = true;
            return respuesta;
        }

        public async Task<RespuestaDTO> EnviarCorreoRecuperacion(string Correo)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var usuario = await _usuarioService.ObtenerXCorreo(Correo);
            if(usuario == null || usuario.Id <= 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No se encontró un usuario registrado con ese correo.";
                return respuesta;
            }
            var resultadoToken = await ConstruirTokenRecuperacion(Correo);
            var emailDTO = new EmailDTO
            {
                To = Correo,
                Subject = "Recuperación de contraseña - BlackBarber",
                Token = resultadoToken.Token
            };
            await _emailService.EnviarEmail(emailDTO);
            respuesta.Estatus = true;
            respuesta.Descripcion = "Se ha enviado un correo con las instrucciones para restablecer tu contraseña.";
            return respuesta;
        }

        public async Task<RespuestaDTO> RestablecerContrasena(RestablecerContrasenaDTO objeto)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            if (string.IsNullOrWhiteSpace(objeto.Correo) || string.IsNullOrWhiteSpace(objeto.Token))
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "Email y token son requeridos.";
                return respuesta;
            }

            if (string.IsNullOrWhiteSpace(objeto.Contrasena))
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "La contraseña es requerida.";
                return respuesta;
            }

            var usuario = await _usuarioService.ObtenerXCorreo(objeto.Correo);
            if (usuario == null || usuario.Id<=0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "Usuario no encontrado";
                return respuesta;
            }

            string decodedToken;
            try
            {
                var tokenBytes = WebEncoders.Base64UrlDecode(objeto.Token);
                decodedToken = Encoding.UTF8.GetString(tokenBytes);
            }
            catch
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "Token inválido o expirado.";
                return respuesta;
            }
            var handler = new JwtSecurityTokenHandler();
            if(!handler.CanReadToken(objeto.Token))
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "Token inválido o expirado.";
                return respuesta;
            }
            var jwtToken = handler.ReadJwtToken(objeto.Token);

            // El email suele venir en el claim 'email' o 'sub' (Subject)
            var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            if(emailClaim == null)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "Token inválido: no contiene el correo electrónico.";
                return respuesta;
            }

            if(emailClaim != usuario.Correo)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "Token inválido: el correo electrónico no coincide.";
                return respuesta;
            }

            usuario.PasswordHash = _passwordEncryption.HashPassword(objeto.Contrasena);
            respuesta = await _usuarioService.Editar(usuario);

            return respuesta;
        }
    }
}
