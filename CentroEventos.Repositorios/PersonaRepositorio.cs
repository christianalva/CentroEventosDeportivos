using CentroEventos.Aplicacion.Entidades;
using CentroEventos.Aplicacion.Excepciones;
using CentroEventos.Aplicacion.Interfaces;

namespace CentroEventos.Repositorios
{
    public class PersonaRepositorio : IRepositorioPersona // implemento la interfas 
    {
        private readonly CentroEventosContext _context;

        public PersonaRepositorio(CentroEventosContext context)
        {
            _context = context;
        }

        //===================== Agregar ======================
        public void Agregar(Persona persona)
        {
            _context.Personas.Add(persona);
            _context.SaveChanges();
        }

        //================== Modificar ======================
        public void Modificar(Persona personaAModificar)
        {

            // Busco al usuario a modificar
            var persona = ObtenerPorId(personaAModificar.Id) ?? throw new EntidadNotFoundException("No se encuentra la Persona a modificar.");

            // Asigno los nuevo datos al Usuario que obtuve por parametro 
            persona.Dni = persona.Dni;
            persona.Nombre = personaAModificar.Nombre;
            persona.Apellido = personaAModificar.Apellido;
            persona.Email = personaAModificar.Email;
            persona.Telefono = personaAModificar.Telefono;

            // Guardo los cambios en la base de datos
            _context.SaveChanges();
        }

        //================== Eliminar ======================
        public void Eliminar(int id)
        {
            var persona = _context.Personas.Find(id);
            if (persona == null) throw new EntidadNotFoundException();
            _context.Personas.Remove(persona);
            _context.SaveChanges();
        }

        //============== Obtener por Id ===================
        public Persona? ObtenerPorId(int id)
            => _context.Personas.Find(id);

        //============= Listar Todos ======================
        public List<Persona> ListarTodos()
            => _context.Personas.ToList();

        //============ Existe Por Dni =====================
        public bool ExistePorDni(string dni)
            => _context.Personas.Any(p => p.Dni == dni);

        //============= Existe Por Email ===================
        public bool ExistePorEmail(string email)
            => _context.Personas.Any(p => p.Email == email);
    }
}