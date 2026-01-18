using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using CentroEventos.Aplicacion.Entidades;
using CentroEventos.Aplicacion.Excepciones;
using CentroEventos.Aplicacion.Interfaces;

namespace CentroEventos.Repositorios
{
    public class ReservaRepositorio : IRepositorioReserva
    {
        private readonly CentroEventosContext _context;

        public ReservaRepositorio(CentroEventosContext context)
        {
            _context = context;
        }

        //========================= Agregar =====================
        public void Agregar(Reserva reserva)
        {
            _context.Reservas.Add(reserva);
            _context.SaveChanges();
        }

        // ====================== Modificar =====================
        public void Modificar(Reserva reservaAModificar)
        {
            var reserva = ObtenerPorId(reservaAModificar.Id) ?? throw new EntidadNotFoundException("No se encuentra la Reserva a modificar.");
                     
            reserva.PersonaId      = reservaAModificar.PersonaId;
            reserva.EventoDeportivoId     = reservaAModificar.EventoDeportivoId;
            reserva.FechaAltaReserva   = reservaAModificar.FechaAltaReserva;
            reserva.EstadoAsistencia   = reservaAModificar.EstadoAsistencia;

            _context.SaveChanges();
        }

        // ======================= Eliminar =====================
        public void Eliminar(int id)
        {
            var reserva = _context.Reservas.Find(id);
            if (reserva == null) throw new EntidadNotFoundException();
            _context.Reservas.Remove(reserva);
            _context.SaveChanges();
        }

        //=================== Obtener por Id =====================
        public Reserva? ObtenerPorId(int id)
            => _context.Reservas.Find(id);

        // ================= Listar Todos =========================
        public List<Reserva> ListarTodos()
            => _context.Reservas.ToList();

        // ============= Obtener Reservas Por Evento ==============
        public List<Reserva> ObtenerReservasPorEvento(int eventoId)
            => _context.Reservas.Where(r => r.EventoDeportivoId == eventoId).ToList();

        //=============== Obtener Reservas por Personas ===========
        public List<Reserva> ObtenerReservasPorPersona(int personaId)
            => _context.Reservas.Where(r => r.PersonaId == personaId).ToList();
        
    }
}