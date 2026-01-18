using CentroEventos.Aplicacion.Entidades;
using CentroEventos.Aplicacion.Interfaces;


namespace CentroEventos.Aplicacion.CasosDeUso.EventosDeportivos
{
    public class ListarEventosUseCase
    {
        private readonly IRepositorioEventoDeportivo _repoEventos;

        public ListarEventosUseCase(IRepositorioEventoDeportivo repoEventos)
        {
            _repoEventos = repoEventos;
        }
        public List<EventoDeportivo> Ejecutar()
        {
            return _repoEventos.ListarTodos();
        }
    }
}