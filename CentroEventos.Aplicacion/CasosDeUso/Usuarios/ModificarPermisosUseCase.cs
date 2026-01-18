using CentroEventos.Aplicacion.Interfaces;
using CentroEventos.Aplicacion.Validadores;
using CentroEventos.Aplicacion.Entidades;
using CentroEventos.Aplicacion.Excepciones;
using CentroEventos.Aplicacion.Enums;

namespace CentroEventos.Aplicacion.CasosDeUso.Usuarios
{
    public class ModificarPermisosUseCase
    {
        private readonly IRepositorioUsuario _repoUsuario;
        private readonly IServicioAutorizacion _autorizacion;
        private readonly IServicioSesion _sesion;


        public ModificarPermisosUseCase(IRepositorioUsuario repoUsuario, IServicioAutorizacion autorizacion, IServicioSesion sesion)
        {
            _repoUsuario = repoUsuario;
            _autorizacion = autorizacion;
            _sesion = sesion;
        }

        public Usuario ObtenerUsuario(int usuarioId)
        {

            var usuarioActual = _sesion.UsuarioActual ?? throw new EntidadNotFoundException("No hay usuario logueado en la sesión.");
            int idUsuarioEnSesion = usuarioActual.Id;

            var existe = _repoUsuario.ObtenerPorId(usuarioId) ?? throw new EntidadNotFoundException($"Usuario con Id {usuarioId} no existe.");

            // Verifico que tenga el permiso de Gestion de usuarios o sea admin 
            if (!_autorizacion.PoseeElPermiso(idUsuarioEnSesion, Permiso.GestionUsuario) && !UsuarioAdmin.EsAdmin(usuarioActual))
                throw new FalloAutorizacionException("No tiene permiso para dar Permisos a Usuarios.");

            // No dejo que un usuario se modifique sus propios permisos ()
            if (idUsuarioEnSesion == usuarioId)
                throw new OperacionInvalidaException("No puedes modificar tus propios permisos.");

            return existe;
        }


        public void Ejecutar(int usuarioId, List<Enums.Permiso> permisos)
        {
    
            Usuario usuario = ObtenerUsuario(usuarioId);          

            usuario.Permisos = permisos;
            _repoUsuario.Modificar(usuario); // Reuso el Modificar solo que ahora le paso los mismos datos, con distintos permisos 

        }


    }
}