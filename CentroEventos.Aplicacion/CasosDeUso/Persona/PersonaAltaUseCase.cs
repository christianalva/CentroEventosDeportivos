using CentroEventos.Aplicacion.Entidades;
using CentroEventos.Aplicacion.Excepciones;
using CentroEventos.Aplicacion.Interfaces;
using CentroEventos.Aplicacion.Validadores;


namespace CentroEventos.Aplicacion.CasosDeUso.Persona
{
    public class PersonaAltaUseCase 
    {
        private readonly IRepositorioPersona _repoPersona;
        private readonly ValidadorPersona _validadorPersona;
        private readonly IServicioAutorizacion _servicioAutorizacion;
        private readonly IServicioSesion _sesion;

        public PersonaAltaUseCase(IServicioAutorizacion servicio, IRepositorioPersona repoPersona, ValidadorPersona validadorPersona, IServicioSesion sesion)
        {
            _repoPersona = repoPersona;
            _validadorPersona = validadorPersona;
            _servicioAutorizacion = servicio;
            _sesion = sesion;
        }
 
        public void Ejecutar(Entidades.Persona persona)
        {
            var usuarioActual = _sesion.UsuarioActual ?? throw new EntidadNotFoundException("No hay usuario logueado en la sesión.");
            int idUsuario = usuarioActual.Id;

            if (!_servicioAutorizacion.PoseeElPermiso(idUsuario, Enums.Permiso.UsuarioAlta))
                throw new FalloAutorizacionException();
           
           
            var errores =  _validadorPersona.Validar(persona);
            // Si se encontraron errores de validacion, lanzo una excepcion
            if (errores.Any())
            {
                throw new ValidacionException(string.Join(" • ", errores));
            }
            _repoPersona.Agregar(persona);
        }

    }
}