using System.Diagnostics.Contracts;
using CentroEventos.Aplicacion.Entidades;

namespace CentroEventos.Aplicacion.Interfaces
{
    public interface IRepositorioReserva
    {
        void Agregar(Reserva reserva);
        void Eliminar(int id);
        void Modificar(Reserva reserva);
        Reserva? ObtenerPorId(int id);
        List<Reserva> ListarTodos();
        List<Reserva> ObtenerReservasPorEvento(int eventoDeportivoId);
        List<Reserva> ObtenerReservasPorPersona(int personaId);

    }

}