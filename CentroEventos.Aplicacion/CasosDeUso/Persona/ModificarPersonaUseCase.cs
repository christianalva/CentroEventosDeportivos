using CentroEventos.Aplicacion.Entidades;
using CentroEventos.Aplicacion.Excepciones;
using CentroEventos.Aplicacion.Interfaces;
using CentroEventos.Aplicacion.Validadores;

namespace CentroEventos.Aplicacion.CasosDeUso.Persona
{
    public class ModificarPersonaUseCase
    {
        private readonly IRepositorioPersona _repoPersona;
        private readonly IServicioAutorizacion _servicioAutorizacion;
        private readonly ValidadorPersona _validadorPersona;
        private readonly IServicioSesion _sesion;

        public ModificarPersonaUseCase(IServicioAutorizacion servicio, IRepositorioPersona repoPersona, ValidadorPersona validadorPersona, IServicioSesion sesion)
        {
            _repoPersona = repoPersona;
            _servicioAutorizacion = servicio;
            _validadorPersona = validadorPersona;
            _sesion = sesion;
        }

        /// Valido la existencia de la persona lo hago en un modulo diferente para validar la existencia antes de modificar la persona
        public Entidades.Persona ObtenerParaModificar(int personaId)
        {
            var usuario = _sesion.UsuarioActual ?? throw new EntidadNotFoundException("No hay usuario en sesión.");

            if (!_servicioAutorizacion.PoseeElPermiso(usuario.Id, Enums.Permiso.UsuarioModificacion))
                throw new FalloAutorizacionException("No tiene permiso para modificar personas.");

            var persona = _repoPersona.ObtenerPorId(personaId) ?? throw new EntidadNotFoundException($"No existe persona con Id {personaId}.");

            return persona;
        }

        
        public void Ejecutar(Entidades.Persona personaActualizada)
        {
            var copia = new Entidades.Persona
            {
                Id = personaActualizada.Id,
                Dni = personaActualizada.Dni,
                Nombre = personaActualizada.Nombre,
                Apellido = personaActualizada.Apellido,
                Email = personaActualizada.Email,
                Telefono = personaActualizada.Telefono
            };

            var personaOriginal = ObtenerParaModificar(personaActualizada.Id);

            var errores = _validadorPersona.Validar(copia);
            if (errores.Any())
                throw new ValidacionException(string.Join(" • ", errores));

            personaOriginal.Dni = personaActualizada.Dni;
            personaOriginal.Nombre = personaActualizada.Nombre;
            personaOriginal.Apellido = personaActualizada.Apellido;
            personaOriginal.Email = personaActualizada.Email;
            personaOriginal.Telefono = personaActualizada.Telefono;

            _repoPersona.Modificar(personaOriginal);
        }
    }
}

