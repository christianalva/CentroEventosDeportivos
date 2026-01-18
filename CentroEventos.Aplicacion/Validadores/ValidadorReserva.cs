using CentroEventos.Aplicacion.Entidades;
using CentroEventos.Aplicacion.Interfaces;

namespace CentroEventos.Aplicacion.Validadores
{
    public class ValidadorReserva
    {
        // Campos inyectados para acceder a los datos necesarios.
        private readonly IRepositorioPersona _repoPersona;
        private readonly IRepositorioEventoDeportivo _repoEvento;
        private readonly IRepositorioReserva _repoReserva;
        
        // Constructor que recibe las dependencias necesarias (repositorios)
        public ValidadorReserva(IRepositorioPersona repoPersona, IRepositorioEventoDeportivo repoEvento, IRepositorioReserva repoReserva)
        {
            _repoPersona = repoPersona;
            _repoEvento = repoEvento;
            _repoReserva = repoReserva;
        }
        public List<string> Validar(int personaId, int eventoDeportivoId)
        {
            List<string> errores = new List<string>();

            // VALIDO LA EXISTENCIA DE LA PERSONA
            if (_repoPersona.ObtenerPorId(personaId) == null)
            {
                errores.Add("La persona no existe.");
            }

            //VALIDO LA EXISTENCIA DEL EVENTO DEPORTIVO
            var evento = _repoEvento.ObtenerPorId(eventoDeportivoId); // lo guardo en una variable para despues reutilizarlo a la hora de validar la existencia de cupo disponible
            if (evento == null)
            {
                errores.Add("El Evento Deportivo no existe.");
            }

            // Si la persona y el evento deportivo existen, sigo con las demas validaciones.
            if (errores.Count() == 0 && evento!=null) 
            {
                //VALIDO QUE LA PERSONA NO HAYA RESERVADO YA ESTE EVENTO 
                List<Reserva> listaReservasPorPersona = _repoReserva.ObtenerReservasPorPersona(personaId);
                foreach (var reserva in listaReservasPorPersona)
                {
                    if (reserva.EventoDeportivoId == eventoDeportivoId)
                    {
                        errores.Add("La Persona ya ah reservado este Evento Deportivo.");
                    }
                }

                //VALIDAR QUE EXISTA CUPO DISPONIBLE PARA EL EVENTO DISPONIBLE
                List<Reserva> reservasEvento = _repoReserva.ObtenerReservasPorEvento(eventoDeportivoId);
                if (reservasEvento.Count >= evento.CupoMaximo)
                {
                    errores.Add("No hay cupo disponible para el Evento Deportivo. ");
                }
            }

            


            return errores;

        }
    }
}