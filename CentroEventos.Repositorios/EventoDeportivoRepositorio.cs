using CentroEventos.Aplicacion.Entidades;
using CentroEventos.Aplicacion.Excepciones;
using CentroEventos.Aplicacion.Interfaces;
namespace CentroEventos.Repositorios
{
    public class EventoDeportivoRepositorio : IRepositorioEventoDeportivo
    {
        private readonly CentroEventosContext _context;

        public EventoDeportivoRepositorio(CentroEventosContext context)
        {
            _context = context;
        }

        //====================== Agregar =====================
        public void Agregar(EventoDeportivo evento)
        {
            _context.Eventos.Add(evento);
            _context.SaveChanges();
        }

        //===================== Modificar ====================
        public void Modificar(EventoDeportivo eventoAModificar)
        {

            var evento = ObtenerPorId(eventoAModificar.Id) ?? throw new EntidadNotFoundException("No se encuentra el Evento a modificar.");

            evento.Nombre = eventoAModificar.Nombre;
            evento.Descripcion = eventoAModificar.Descripcion;
            evento.FechaHoraInicio = eventoAModificar.FechaHoraInicio;
            evento.DuracionHoras = eventoAModificar.DuracionHoras;
            evento.CupoMaximo = eventoAModificar.CupoMaximo;
            evento.ResponsableId = eventoAModificar.ResponsableId;

            _context.SaveChanges();
        }

        //===================== Eliminar ====================
        public void Eliminar(int id)
        {
            var evento = _context.Eventos.Find(id);
            if (evento == null) throw new EntidadNotFoundException();
            _context.Eventos.Remove(evento);
            _context.SaveChanges();
        }

        //================== Obtener por Id ===================
        public EventoDeportivo? ObtenerPorId(int id)
            => _context.Eventos.Find(id);

        //================ Listar Todos =======================
        public List<EventoDeportivo> ListarTodos()
            => _context.Eventos.ToList();

        //================ Existe por Nombre =================
        public bool ExistePorNombre(string nombre)
            => _context.Eventos.Any(n => n.Nombre == nombre);

    }
}