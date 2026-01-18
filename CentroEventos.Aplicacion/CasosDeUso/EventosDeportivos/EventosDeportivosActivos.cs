// Lo uso para la pantalla de inicio nomas
using CentroEventos.Aplicacion.Entidades;
using CentroEventos.Aplicacion.Interfaces;

namespace CentroEventos.Aplicacion.CasosDeUso.EventosDeportivos
{
    public class EventosDeportivosActivos
    {
        private readonly IRepositorioEventoDeportivo _repoEvento;

        public EventosDeportivosActivos(IRepositorioEventoDeportivo repoEvento)
        {
            _repoEvento = repoEvento;
        }

        public int Ejecutar()
        {
            List<EventoDeportivo> eventos = _repoEvento.ListarTodos();
            List<EventoDeportivo> eventosCumplen = new List<EventoDeportivo>();
            foreach (var e in eventos)
            {
                if (e.FechaHoraInicio >= DateTime.Now)
                {
                    eventosCumplen.Add(e);
                }
            }

            return eventosCumplen.Count();

            // Podria hacerlo mas limpio asi': 
            //return _repoEvento
            //    .ListarTodos()
            //    .Count(e => e.FechaHoraInicio >= DateTime.Now);
        }
    }
}