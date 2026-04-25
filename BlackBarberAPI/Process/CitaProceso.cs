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
        private readonly BlackBarberContext _dbContext;

        public CitaProceso(ICitaService<BlackBarberContext> citaService
            , IServicioCitaService<BlackBarberContext> servicioCita_service
            , IDetalleCitaService<BlackBarberContext> detalleCitaService
            , IBarberoService<BlackBarberContext> barberoService
            , IDiasHabilesService<BlackBarberContext> habilesService
            , BlackBarberContext dbContext
            )
        {
            _citaService = citaService;
            _servicioCita_service = servicioCita_service;
            _detalleCitaService = detalleCitaService;
            _barberoService = barberoService;
            _habilesService = habilesService;
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
            if(!diaHabil.Habil)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "El día seleccionado no es hábil para agendar citas.";
                return respuesta;
            }
            if(diaHabil.HoraFin<citaCreacionDTO.FechaTermino.TimeOfDay || diaHabil.HoraInicio > citaCreacionDTO.FechaInicio.TimeOfDay)
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
                if (citaCreada.Id <= 0){
                    throw new Exception("No se pudo crear la cita.");
                }
                foreach (var servicio in citaCreacionDTO.Servicios)
                {
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
                        IdBarbero = servicio.IdBarbero
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
            catch(Exception ex)
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
    }
}
