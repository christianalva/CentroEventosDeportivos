using CentroEventos.Aplicacion.Entidades;
using CentroEventos.Aplicacion.Excepciones;
using CentroEventos.Aplicacion.Interfaces;

namespace CentroEventos.Aplicacion.CasosDeUso.EventosDeportivos
{
    public class EventoDeportivoBajaUseCase
    {
        private readonly IRepositorioEventoDeportivo _repoEvento;
        private readonly IServicioAutorizacion _servicioAutorizacion;
        private readonly IRepositorioReserva _repoReserva;
        private readonly IServicioSesion _sesion;
        public EventoDeportivoBajaUseCase(IServicioAutorizacion servicio, IRepositorioEventoDeportivo repoEvento, IRepositorioReserva repoReserva, IServicioSesion sesion)
        {
            _repoEvento = repoEvento;
            _servicioAutorizacion = servicio;
            _repoReserva = repoReserva;
            _sesion = sesion;
        }

        public EventoDeportivo ObtenerParaBaja(int eventoId)
        {
            var usuario = _sesion.UsuarioActual ?? throw new EntidadNotFoundException("No hay usuario en sesión.");

            // Verifico quue el usuario tenga autorizacion para eliminar un evento deportivo
            if (!_servicioAutorizacion.PoseeElPermiso(usuario.Id, Enums.Permiso.EventoBaja))
                throw new FalloAutorizacionException();

            // Verifico que el evento exista
            var evento = _repoEvento.ObtenerPorId(eventoId);
            if (evento == null)
            {
                throw new EntidadNotFoundException($"No existe EventoDeportivo con ID {eventoId}.");
            }

            return evento;
        }

        public void Ejecutar(int eventoId)
        {
            var evento = ObtenerParaBaja(eventoId);

            // Verifico que el evento deportivo no tenga reservas asiciadas ( regla: No puede eliminarse un EventoDeportivo si existen Reservas asociadas al mismo (independientemente del estado de las reservas))
            var reservas = _repoReserva.ObtenerReservasPorEvento(eventoId);
            if (reservas.Any())
            {
                throw new OperacionInvalidaException("No se puede Eliminar el Evento Deportivo, porque tiene reservas asociadas. ");
            }

            // Si cumple con todas las validaciones lo elimino
            _repoEvento.Eliminar(eventoId);
        }
    }
}