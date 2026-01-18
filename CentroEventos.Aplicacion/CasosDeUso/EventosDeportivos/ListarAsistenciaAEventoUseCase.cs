using CentroEventos.Aplicacion.Entidades;
using CentroEventos.Aplicacion.Excepciones;
using CentroEventos.Aplicacion.Interfaces;

namespace CentroEventos.Aplicacion.CasosDeUso.EventosDeportivos
{
    public class ListarAsistenciaAEventoUseCase
    {
        private readonly IRepositorioReserva _repoReserva;
        private readonly IRepositorioEventoDeportivo _repoEvento;
        private readonly IRepositorioPersona _repoPersona;

        // Inyecto las dependencias
        public ListarAsistenciaAEventoUseCase(IRepositorioReserva repoReserva, IRepositorioEventoDeportivo repoEvento, IRepositorioPersona repoPersona)
        {
            _repoReserva = repoReserva;
            _repoEvento = repoEvento;
            _repoPersona = repoPersona;

        }

    
        public List<Entidades.Persona> Ejecutar(int eventoId)
        {
            var evento = _repoEvento.ObtenerPorId(eventoId);
            if (evento == null)
            {
                throw new EntidadNotFoundException();
            }
            if (evento.FechaHoraInicio.AddHours(evento.DuracionHoras) > DateTime.Now) // verifico que el evento ya haya finalizado 
            {
                throw new OperacionInvalidaException("El Evento Deportivo todavia no ah finalizado");
            }
            // obtengo todas las reservas del evento
            var reservas = _repoReserva.ObtenerReservasPorEvento(eventoId);

            List<Entidades.Reserva> reservasAsistidas = new List<Entidades.Reserva>();
            // recorro las reservas buscando las que estuvieron presentes
            foreach (var r in reservas)
            {
                // verifico si el estado de la reserva es presente (si es asi quiere decir que asistio)
                if (r.EstadoAsistencia == EstadoAsistencia.Presente)
                {
                    reservasAsistidas.Add(r);
                }
            }

            List<Entidades.Persona> personasQueAsistieron = new List<Entidades.Persona>();
            // recorro la lista de reservas buscando a las personas mediante su id
            foreach (var r in reservasAsistidas)
            {
                // obtengo a la persona asociada a la reserva mediante el id guardado anteriormente en reservasAsistidas
                var persona = _repoPersona.ObtenerPorId(r.PersonaId);
                // si la persona existe la agrego a la lista de personas que asistieron
                if (persona != null) personasQueAsistieron.Add(persona);
            }

            return personasQueAsistieron;

        }

        
    }
}