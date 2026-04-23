using BlackBarberAPI.Data;
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


    }
}
