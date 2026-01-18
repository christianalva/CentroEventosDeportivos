using CentroEventos.Aplicacion.Entidades;

namespace CentroEventos.Aplicacion.Interfaces
{
    public interface IServicioSesion
    {
        Usuario? UsuarioActual { get; }
        bool EstaLogueado { get; }
        void IniciarSesion(Usuario usuario);
        void CerrarSesion();
    }
}