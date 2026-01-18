using CentroEventos.Aplicacion.Interfaces;
using CentroEventos.Aplicacion.Validadores;
using CentroEventos.Aplicacion.Entidades;
using CentroEventos.Aplicacion.Excepciones;
using CentroEventos.Aplicacion.Enums;
namespace CentroEventos.Aplicacion.CasosDeUso.Usuarios
{
    public class UsuarioAltaUseCase
    {
        private readonly IRepositorioUsuario _repoUsuario;
        private readonly ValidadorUsuario _validador;
        private readonly IHashServicio _hash;

        public UsuarioAltaUseCase(IRepositorioUsuario repoUsuario, ValidadorUsuario validador, IHashServicio hash)
        {
            _repoUsuario = repoUsuario;
            _validador = validador;
            _hash = hash;
        }

        public void Ejecutar(Usuario usuario)
        {
            // No necesito validar ningun permiso ya que cualquiera puede registrarse y darse de alta a si mismo mientras el email no se repita

            // Valido que los datos pasados cumplan con lo pedido.
            var errores =  _validador.Validar(usuario);
            if (errores.Any())
            {
                throw new ValidacionException(string.Join(" • ", errores));
            }

            // Valido que el Email del usuario no este ya registrado
            if (_repoUsuario.ObtenerPorEmail(usuario.Email) != null)
                throw new DuplicadoException($"El Email {usuario.Email} ya esta registrado. ");
            
            // Hashear la Contraseña pasada
            usuario.Password = _hash.CalcularHash(usuario.Password);

            var todos = _repoUsuario.Lista();
            if (!todos.Any())
                usuario.Permisos = Enum.GetValues<Permiso>().ToList(); // Al primer Admin le doy todos los permisos
            else
                usuario.Permisos = new List<Permiso>{Permiso.UsuarioLectura}; // A los siguiente usuarios solo le doy permiso de lectura.

            _repoUsuario.Agregar(usuario);
        }
    }
}