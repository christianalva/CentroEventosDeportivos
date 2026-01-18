using CentroEventos.Aplicacion.Entidades;
using CentroEventos.Aplicacion.Interfaces;


namespace CentroEventos.Aplicacion.CasosDeUso.Reserva
{
    public class ListarReservasUseCase
    {
        private readonly IRepositorioReserva _repoReserva;

        public ListarReservasUseCase(IRepositorioReserva repoReserva)
        {
            _repoReserva = repoReserva;
        }
        public List<Entidades.Reserva> Ejecutar()
        {
            return _repoReserva.ListarTodos();
        }
    }
}