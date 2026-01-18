using CentroEventos.Aplicacion.Excepciones;
using CentroEventos.Aplicacion.Interfaces;


namespace CentroEventos.Aplicacion.CasosDeUso.Usuarios
{
    public class UsuarioBajaUseCase
    {
        private readonly IRepositorioUsuario _repoUsuario;
        private readonly IServicioAutorizacion _autorizacion;
        private readonly IServicioSesion _sesion;

        public UsuarioBajaUseCase(IRepositorioUsuario repoUsuario, IServicioAutorizacion autorizacion, IServicioSesion sesion)
        {
            _repoUsuario = repoUsuario;
            _autorizacion = autorizacion;
            _sesion = sesion;
        }

        public Entidades.Usuario ObtenerParaBaja(int idUsuarioAEliminar)
        {
            var usuarioActual = _sesion.UsuarioActual ?? throw new EntidadNotFoundException("No hay usuario logueado en la sesión.");
            int idUsuario = usuarioActual.Id;

            if (idUsuario == idUsuarioAEliminar)
                throw new OperacionInvalidaException("No se puede dar de baja a si mismo.");

            if (!_autorizacion.PoseeElPermiso(idUsuario, Enums.Permiso.UsuarioBaja))
                throw new FalloAutorizacionException("No tiene permiso para dar de baja este usuario.");

            // Verifico que exista el Usuario a eliminar
            var existe = _repoUsuario.ObtenerPorId(idUsuarioAEliminar);
            if (existe == null)
                throw new EntidadNotFoundException("El Usuario a Eliminar no existe.");

            // No se detalla en el Pdf pero agrego una validacion antes de dar de baja un "administrador"
            if (existe.EsAdmin())
                throw new FalloAutorizacionException("No se puede eliminar al Administrador.");

            return existe;
        }

        public void Ejecutar(int idUsuarioAEliminar)
        {
            var usuario = ObtenerParaBaja(idUsuarioAEliminar);

            _repoUsuario.Eliminar(idUsuarioAEliminar);

        }
    }
}