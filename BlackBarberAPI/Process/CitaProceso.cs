using BlackBarberAPI.Data;
using BlackBarberAPI.DTOs;
using BlackBarberAPI.Services.Contratos;
using Microsoft.EntityFrameworkCore.Storage;

namespace BlackBarberAPI.Process
{
    public class CitaProceso
    {
        private readonly ICitaService<BlackBarberContext> _citaService;
        private readonly IServicioCitaService<BlackBarberContext> _servicioCita_service;
        private readonly IDetalleCitaService<BlackBarberContext> _detalleCitaService;
        private readonly IBarberoService<BlackBarberContext> _barberoService;
        private readonly IDiasHabilesService<BlackBarberContext> _habilesService;
        private readonly IServicioService<BlackBarberContext> _servicioService;
        private readonly IUsuarioService<BlackBarberContext> _usuarioService;
        private readonly BlackBarberContext _dbContext;

        public CitaProceso(ICitaService<BlackBarberContext> citaService
            , IServicioCitaService<BlackBarberContext> servicioCita_service
            , IDetalleCitaService<BlackBarberContext> detalleCitaService
            , IBarberoService<BlackBarberContext> barberoService
            , IDiasHabilesService<BlackBarberContext> habilesService
            , IServicioService<BlackBarberContext> servicioService
            , IUsuarioService<BlackBarberContext> usuarioService
            , BlackBarberContext dbContext
            )
        {
            _citaService = citaService;
            _servicioCita_service = servicioCita_service;
            _detalleCitaService = detalleCitaService;
            _barberoService = barberoService;
            _habilesService = habilesService;
            _servicioService = servicioService;
            _usuarioService = usuarioService;
            _dbContext = dbContext;
        }

        public async Task<RespuestaDTO> CrearCita(CitaCreacionDTO citaCreacionDTO)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            if (citaCreacionDTO.FechaInicio >= citaCreacionDTO.FechaTermino)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "La fecha de inicio debe ser anterior a la fecha de término.";
                return respuesta;
            }
            if (citaCreacionDTO.FechaInicio.Day != citaCreacionDTO.FechaTermino.Day || citaCreacionDTO.FechaInicio.Month != citaCreacionDTO.FechaTermino.Month || citaCreacionDTO.FechaInicio.Year != citaCreacionDTO.FechaTermino.Year)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "La fecha de inicio y término deben ser el mismo día.";
                return respuesta;
            }
            var diasHabiles = await _habilesService.ObtenerTodos();
            var diaCita = (int)citaCreacionDTO.FechaInicio.DayOfWeek;
            var diaHabil = diasHabiles.FirstOrDefault(d => d.Id == diaCita);
            if (diaHabil == null)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "El día seleccionado no es hábil para agendar citas.";
                return respuesta;
            }
            if (!diaHabil.Habil)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "El día seleccionado no es hábil para agendar citas.";
                return respuesta;
            }
            if (diaHabil.HoraFin < citaCreacionDTO.FechaTermino.TimeOfDay || diaHabil.HoraInicio > citaCreacionDTO.FechaInicio.TimeOfDay)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "El horario seleccionado no es válido para agendar citas.";
                return respuesta;
            }
            CitaDTO cita = new CitaDTO
            {
                FechaInicio = citaCreacionDTO.FechaInicio,
                FechaTermino = citaCreacionDTO.FechaTermino,
                IdCliente = citaCreacionDTO.IdCliente,
                Estatus = 1
            };
            await using IDbContextTransaction transaccion = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var citaCreada = await _citaService.CrearYObtener(cita);
                if (citaCreada.Id <= 0) {
                    throw new Exception("No se pudo crear la cita.");
                }
                foreach (var servicio in citaCreacionDTO.Servicios)
                {
                    var servicioObjeto = await _servicioService.ObtenerXId(servicio.Id);
                    var disponibilidad = await ObtenerDisponibilidad(cita.FechaInicio, cita.FechaTermino, (int)servicio.IdBarbero);
                    if (!disponibilidad.Estatus)
                    {
                        var barbero = await _barberoService.ObtenerXId((int)servicio.IdBarbero);
                        throw new Exception($"El barbero {barbero.Nombre} no está disponible en el horario seleccionado.");
                    }
                    ServicioCitaDTO servicioCita = new ServicioCitaDTO
                    {
                        IdCita = citaCreada.Id,
                        IdServicio = servicio.IdServicio,
                        IdBarbero = servicio.IdBarbero,
                        Precio = servicioObjeto.PrecioBase
                    };
                    var servicioAgregado = await _servicioCita_service.CrearYObtener(servicioCita);
                    if (servicioAgregado.Id <= 0)
                    {
                        throw new Exception("No se pudo crear la cita.");
                    }
                }
                await transaccion.CommitAsync();
                respuesta.Estatus = true;
                respuesta.Descripcion = "Cita creada exitosamente.";
                return respuesta;
            }
            catch (Exception ex)
            {
                await transaccion.RollbackAsync();
                respuesta.Estatus = false;
                respuesta.Descripcion = ex.Message;
                return respuesta;
            }
        }

        public async Task<RespuestaDTO> ObtenerDisponibilidad(DateTime horaInicio, DateTime horaFin, int IdBarbero)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var citasBarbero = await ObtenerCitasXBarbero(IdBarbero);
            var EstaOcupado = false;
            foreach (var cita in citasBarbero)
            {
                if ((horaInicio >= cita.FechaInicio && horaInicio < cita.FechaTermino) ||
                    (horaFin > cita.FechaInicio && horaFin <= cita.FechaTermino) ||
                    (horaInicio <= cita.FechaInicio && horaFin >= cita.FechaTermino))
                {
                    EstaOcupado = true;
                    break;
                }
            }
            respuesta.Estatus = !EstaOcupado;
            respuesta.Descripcion = EstaOcupado ? "El barbero no está disponible en ese horario." : "El barbero está disponible en ese horario.";
            return respuesta;
        }

        public async Task<List<CitaDTO>> ObtenerCitasXBarbero(int IdBarbero)
        {
            var citas = await _citaService.ObtenerCitasVigentes();
            List<CitaDTO> lista = new List<CitaDTO>();
            foreach (var cita in citas)
            {
                var detallesCita = await _servicioCita_service.ObtenerXPerteneciente(cita.Id);
                var detallesBarbero = detallesCita.Where(d => d.IdBarbero == IdBarbero);
                if (detallesBarbero.Any())
                {
                    lista.Add(cita);
                }
            }
            return lista;
        }

        public async Task<List<CitaDTO>> ObtenerCitasHoy()
        {
            var citas = await _citaService.ObtenerCitasHoy();
            return citas;
        }
        public async Task<List<CitaDTO>> ObtenerCitasAyer()
        {
            var citas = await _citaService.ObtenerCitasAyer();
            return citas;
        }

        public async Task<List<CitaDTO>> ObtenerCitasXCliente(int idCLiente)
        {
            var citas = await _citaService.ObtenerCitasVigentes();
            citas = citas.Where(c => c.IdCliente == idCLiente).ToList();
            return citas;
        }

        public async Task<List<CitaDetalladaDTO>> ObtenerListadoDetalladoXUsuario(int idUsuario)
        {
            List<CitaDetalladaDTO> listado = new List<CitaDetalladaDTO>();
            var citas = await ObtenerCitasXCliente(idUsuario);
            var servicios = await _servicioService.ObtenerTodos();
            var usuario = await _usuarioService.ObtenerXId(idUsuario);
            foreach (var cita in citas)
            {
                var citaDetallada = new CitaDetalladaDTO
                {
                    Id = cita.Id,
                    FechaInicio = cita.FechaInicio,
                    FechaTermino = cita.FechaTermino,
                    IdCliente = cita.IdCliente,
                    Estatus = cita.Estatus,
                    NombreCliente = usuario.Username,
                    EstatusDescripcion = cita.Estatus == 1 ? "Agendada" : cita.Estatus == 2 ? "En curso" : cita.Estatus == 3 ? "Completada" : "Cancelada",
                };
                var serviciosCita = await _servicioCita_service.ObtenerXPerteneciente(cita.Id);
                foreach (var servicio in serviciosCita)
                {
                    var servicioObjeto = servicios.FirstOrDefault(s => s.Id == servicio.IdServicio);
                    var barberoRelacionado = await _barberoService.ObtenerXId((int)servicio.IdBarbero);
                    ServicioCitaDetalladoDTO servicioDetallado = new ServicioCitaDetalladoDTO
                    {
                        Id = servicio.Id,
                        IdCita = servicio.IdCita,
                        IdBarbero = servicio.IdBarbero,
                        IdServicio = servicio.IdServicio,
                        Precio = servicio.Precio,
                        NombreBarbero = barberoRelacionado.Nombre,
                        NombreServicio = servicioObjeto != null ? servicioObjeto.Nombre : "Servicio no encontrado"
                    };
                    citaDetallada.Servicios.Add(servicioDetallado);
                }
                citaDetallada.Total = citaDetallada.Servicios.Sum(s => s.Precio);
                listado.Add(citaDetallada);
            }
            return listado;
        }
    }
}
