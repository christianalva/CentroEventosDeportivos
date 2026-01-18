using CentroEventos.Aplicacion.Enums;
using CentroEventos.Aplicacion.Interfaces;

namespace CentroEventos.Aplicacion.Servicios
{
    public class ServicioAutorizacion : IServicioAutorizacion
    {
        private readonly IRepositorioUsuario _repoUsuario;

        public ServicioAutorizacion(IRepositorioUsuario repoUsuario)
        {
            _repoUsuario = repoUsuario;
        }

        public bool PoseeElPermiso(int idUsuario, Permiso permiso)
        {
            var usuario = _repoUsuario.ObtenerPorId(idUsuario);
            if (usuario == null) return false;
            return usuario.Permisos.Contains(permiso);

        }

    }
}