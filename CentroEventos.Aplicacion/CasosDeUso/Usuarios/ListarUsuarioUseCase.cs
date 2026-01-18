using CentroEventos.Aplicacion.Entidades;
using CentroEventos.Aplicacion.Excepciones;
using CentroEventos.Aplicacion.Interfaces;

namespace CentroEventos.Aplicacion.CasosDeUso.Usuarios
{
    public class ListarUsuarioUseCase
    {
        private readonly IRepositorioUsuario _repoUsuario;
        private readonly IServicioAutorizacion _autorizacion;
        private readonly IServicioSesion _sesion;

        public ListarUsuarioUseCase(IRepositorioUsuario repoUsuario, IServicioAutorizacion autorizacion, IServicioSesion sesion)
        {
            _repoUsuario = repoUsuario;
            _autorizacion = autorizacion;
            _sesion = sesion;
        }


        public List<Usuario> Ejecutar()
        {
            var usuarioActual = _sesion.UsuarioActual ?? throw new EntidadNotFoundException("No hay usuario logueado en la sesión.");
            int idUsuario = usuarioActual.Id;

            if (!_autorizacion.PoseeElPermiso(idUsuario, Enums.Permiso.GestionUsuario))
            {
                // Si no posee el permiso de GestionUsuario devuelvo solamente el usuario que ingreso 
                return new List<Usuario> { usuarioActual };

            }

            return _repoUsuario.Lista();
        }

    }
}