// Lo uso para la pantalla de inicio nomas
using CentroEventos.Aplicacion.Interfaces;

namespace CentroEventos.Aplicacion.CasosDeUso.Reserva
{
    public class ReservasRealizadas
    {
        private readonly IRepositorioReserva _repoReserva;

        public ReservasRealizadas(IRepositorioReserva repoReserva)
        {
            _repoReserva = repoReserva;
        }

        public int Ejecutar()
        {
            return _repoReserva.ListarTodos().Count();
        }

    }
}