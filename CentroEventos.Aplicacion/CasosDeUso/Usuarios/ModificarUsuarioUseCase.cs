using CentroEventos.Aplicacion.Interfaces;
using CentroEventos.Aplicacion.Validadores;
using CentroEventos.Aplicacion.Entidades;
using CentroEventos.Aplicacion.Excepciones;
using CentroEventos.Aplicacion.Enums;

namespace CentroEventos.Aplicacion.CasosDeUso.Usuarios
{
    public class ModificarUsuarioUseCase
    {
        private readonly IRepositorioUsuario _repoUsuario;
        private readonly ValidadorUsuario _validador;
        private readonly IHashServicio _hashServicio;
        private readonly IServicioAutorizacion _autorizacion;
        private readonly IServicioSesion _sesion;

        public ModificarUsuarioUseCase(IRepositorioUsuario repoUsuario,ValidadorUsuario validador,IHashServicio hashServicio,IServicioAutorizacion autorizacion, IServicioSesion  sesion)
        {
            _repoUsuario   = repoUsuario;
            _validador     = validador;
            _hashServicio  = hashServicio;
            _autorizacion  = autorizacion;
            _sesion        = sesion;
        }

        public Usuario ObtenerParaModificar(int usuarioId)
        {
            var actual = _sesion.UsuarioActual ?? throw new EntidadNotFoundException("Debe iniciar sesión.");
            if (!_autorizacion.PoseeElPermiso(actual.Id, Permiso.UsuarioModificacion) && (actual.Id != usuarioId))
            {
                throw new FalloAutorizacionException("No tiene permiso para modificar este usuario.");
            }

            return _repoUsuario.ObtenerPorId(usuarioId) ?? throw new EntidadNotFoundException($"Usuario con Id {usuarioId} no existe.");
        }

        public void Ejecutar(Usuario datosModificados)
        {
        
            // Creo una copia de la entidad recibida por parametro para no modificarla antes de realizar las validaciones
            var copia = new Usuario
            {
                Id = datosModificados.Id,
                Nombre = datosModificados.Nombre,
                Apellido = datosModificados.Apellido,
                Email = datosModificados.Email,
                Password = datosModificados.Password
            };

            // Valido al usuario modificado, pasandole el parametro esModificacion como true para poder dejar la contreseña en blanco y que me deje la misma que tenia 
            var errores = _validador.Validar(copia, esModificacion: true);
            if (errores.Any()) throw new ValidacionException(string.Join(" • ", errores));

            // Verifico que el Email no este duplicado 
            var original = ObtenerParaModificar(datosModificados.Id);
            if (!string.Equals(original.Email, datosModificados.Email, StringComparison.OrdinalIgnoreCase))
            {
                if (_repoUsuario.ObtenerPorEmail(datosModificados.Email) != null) throw new DuplicadoException($"El email {datosModificados.Email} ya esta en uso.");
            }

            // Aplico los cambios solo si paso la validacion 
            original.Nombre   = datosModificados.Nombre;
            original.Apellido = datosModificados.Apellido;
            original.Email    = datosModificados.Email;

            // Solo vuelvo a hashear la contraseña si se ingresa una nueva sino dejo la misma 
            if (!string.IsNullOrWhiteSpace(datosModificados.Password))
            {
                original.Password = _hashServicio.CalcularHash(datosModificados.Password);
            }
        
            _repoUsuario.Modificar(original);
        }
    }
}