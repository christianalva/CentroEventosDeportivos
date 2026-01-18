using CentroEventos.Aplicacion.Interfaces;
using CentroEventos.Aplicacion.Entidades;
using CentroEventos.Aplicacion.Enums;
using CentroEventos.Aplicacion.CasosDeUso.Usuarios;

namespace CentroEventos.Aplicacion.Servicios
{
    public class ServicioSesion : IServicioSesion
    {
        private Usuario? _usuarioActual; // Almaceno el usuario actual de la sesión

        // Propiedad publica de solo lectura para acceder al usuario actual
        public Usuario? UsuarioActual
        {
            get => _usuarioActual; // Devuelvo al usuario actual
            private set => _usuarioActual = value; // Permito modificarlo (solo dentro de la clase)
        }

        // Propiedad para saber si hay un usuario logueado
        public bool EstaLogueado => UsuarioActual != null;

        public void IniciarSesion(Usuario? usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            UsuarioActual = usuario;
        }

        // Metodo para cerrar sesion
        public void CerrarSesion()
        {
            UsuarioActual = null; 
        }
    }
}
