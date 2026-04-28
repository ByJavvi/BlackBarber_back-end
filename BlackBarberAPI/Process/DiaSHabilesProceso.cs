using BlackBarberAPI.Data;
using BlackBarberAPI.DTOs;
using BlackBarberAPI.Services.Contratos;

namespace BlackBarberAPI.Process
{
    public class DiaSHabilesProceso
    {
        private readonly IDiasHabilesService<BlackBarberContext> _habilesService;

        public DiaSHabilesProceso(IDiasHabilesService<BlackBarberContext> habilesService)
        {
            _habilesService = habilesService;
        }

        public async Task<List<DiasHabilDTO>> ObtenerDiasHabiles()
        {
            var lista = await _habilesService.ObtenerTodos();
            return lista;
        }

        public async Task<RespuestaDTO> EditarDiaHabil(DiasHabilDTO parametros)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            if (parametros.HoraInicio > parametros.HoraFin)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "La hora de inicio no puede ser después de la hora de fin.";
                return respuesta;
            }
            respuesta = await _habilesService.EditarDiaHabil(parametros);
            return respuesta;
        }
    }
}
