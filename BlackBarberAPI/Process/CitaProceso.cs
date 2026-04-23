using BlackBarberAPI.Data;
using BlackBarberAPI.DTOs;
using BlackBarberAPI.Services.Contratos;

namespace BlackBarberAPI.Process
{
    public class CitaProceso
    {
        private readonly ICitaService<BlackBarberContext> _citaService;
        private readonly IServicioCitaService<BlackBarberContext> _servicioCita_service;
        private readonly IDetalleCitaService<BlackBarberContext> _detalleCitaService;

        public CitaProceso(ICitaService<BlackBarberContext> citaService
            , IServicioCitaService<BlackBarberContext> servicioCita_service
            , IDetalleCitaService<BlackBarberContext> detalleCitaService
            )
        {
            _citaService = citaService;
            _servicioCita_service = servicioCita_service;
            _detalleCitaService = detalleCitaService;
        }

        public async Task<RespuestaDTO> CrearCita(CitaCreacionDTO citaCreacionDTO)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            return respuesta;
        }

        public async Task<RespuestaDTO> ObtenerDisponibilidad(DateTime horaInicio, DateTime horaFin, int IdBarbero)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            return respuesta;
        }

        public async Task<List<CitaDTO>> ObtenerCitasXBarbero(int IdBarbero)
        {
            var citas = await _citaService.ObtenerCitasVigentes();
            List<CitaDTO> lista = new List<CitaDTO>();
            foreach (var cita in citas)
            {
                var detallesCita = await _servicioCita_service.ObtenerXPerteneciente(cita.Id);
                var detallesBarberp = detallesCita.Where(d => d.IdBarbero == IdBarbero);
                if (detallesBarberp.Any())
                {
                    lista.Add(cita);
                }
            }
            return lista;
        }
    }
}
