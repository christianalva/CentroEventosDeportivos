using CentroEventos.Aplicacion.Entidades;
using CentroEventos.Aplicacion.Interfaces;
using CentroEventos.Aplicacion.Excepciones;
using Microsoft.EntityFrameworkCore;


namespace CentroEventos.Repositorios
{
    public class UsuarioRepositorio : IRepositorioUsuario
    {
        // Almaceno la instancia del contexto de la base de datos
        private readonly CentroEventosContext _context;
        private readonly IHashServicio          _hashServicio;

        // Constructor que recibe el contexto a traves de inyeccion de dependencias
        public UsuarioRepositorio(CentroEventosContext context, IHashServicio hashServicio)
        {
            _context = context;
            _hashServicio = hashServicio;
        }

        // ================= Agregar ==================
        public void Agregar(Usuario usuario)
        {
            _context.Usuarios.Add(usuario); // Añado el objeto Usuario al Dbset de usuarios del contexto
            _context.SaveChanges(); //Se guardan los cambios en la base de datos
        }

        // ================== Modificar =================
        public void Modificar(Usuario usuarioAModificar)
        {
            // Busco al usuario a modificar
            var usuario = ObtenerPorId(usuarioAModificar.Id) ?? throw new EntidadNotFoundException("No se encuentra el usuario a modificar.");

            // Asigno los nuevo datos al Usuario que obtuve por parametro 
            usuario.Email      = usuarioAModificar.Email;
            usuario.Nombre     = usuarioAModificar.Nombre;
            usuario.Apellido   = usuarioAModificar.Apellido;
            usuario.Password   = usuarioAModificar.Password;

            // Agrego los permisos (esto me sirve para el modificarPermisos)
            usuario.Permisos = usuarioAModificar.Permisos.ToList();

            // Guardo los cambios en la base de datos
            _context.SaveChanges();
        }

        // ================== Eliminar ====================
        public void Eliminar(int id)
        {
            var usuario = _context.Usuarios.Find(id); // Busco al usuario por su ID 
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario); // Elimino al Usuario del DbSet
                _context.SaveChanges();
            }

        }

        // ================== Obtener por ID =================
        public Usuario? ObtenerPorId(int id)
            => _context.Usuarios.Find(id);

        // ================ Obtener Por Email ================
        public Usuario? ObtenerPorEmail(string email)
            => _context.Usuarios.FirstOrDefault(u => u.Email == email);
        // Busca y retorna el primer usuario que el Email coincida con el valor dado, si no retorna null

        // ================ Listar Usuarios =================
        public List<Usuario> Lista()
            => _context.Usuarios.ToList();
    }
}
