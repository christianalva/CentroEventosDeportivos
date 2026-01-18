using CentroEventos.Aplicacion.Excepciones;
using CentroEventos.Aplicacion.Entidades;
using CentroEventos.Aplicacion.Interfaces;

namespace CentroEventos.Aplicacion.CasosDeUso.EventosDeportivos
{
    public class ListarEventosConCupoDisponibleUseCase
    {
        // CAMPOS PRIVADOS PARA LOS REPOSITORIOS
        private readonly IRepositorioEventoDeportivo _repoEvento;
        private readonly IRepositorioReserva _repoReserva;

        // CONSTRUCTOR QUE INYECCTA DEPENDENCIAS
        public ListarEventosConCupoDisponibleUseCase(IRepositorioEventoDeportivo repoEvento, IRepositorioReserva repoReserva)
        {
            _repoEvento = repoEvento;
            _repoReserva = repoReserva;
        }

        // METODO QUE ME VA A DEVOLVER UNA LISTA CON LOS EVENTOS CON CUPO DISPONIBLE
        public List<EventoDeportivo> Ejecutar()
        {
            List<EventoDeportivo> eventosConCupo = new List<EventoDeportivo>();

            // Traigo todos los eventos
            var todos = _repoEvento.ListarTodos() ?? throw new EntidadNotFoundException("No hay eventos cargados en el sistema.");

            // Filtro sólo los que aún no terminaron y tienen cupo
            foreach (var e in todos)
            {
                // si la fecha de inicio es mayor a la actual
                if (e.FechaHoraInicio.AddHours(e.DuracionHoras) > DateTime.Now)
                {
                    var reservas = _repoReserva.ObtenerReservasPorEvento(e.Id);
                    // veo si la cantidad de reservas es menor al cupo maximo, si es asi lo guardo el la lista
                    if (reservas.Count < e.CupoMaximo)
                    {
                        eventosConCupo.Add(e);
                    }
                }
            }
            
            // Si la lista resultante esta vacia, lanzo excepcion 
            if (!eventosConCupo.Any())
                throw new OperacionInvalidaException("No hay eventos con cupo disponible.");


            return eventosConCupo;
        }

    }
}