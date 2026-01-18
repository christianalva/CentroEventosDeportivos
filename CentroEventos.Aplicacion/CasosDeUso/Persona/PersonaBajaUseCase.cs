using CentroEventos.Aplicacion.Entidades;
using CentroEventos.Aplicacion.Excepciones;
using CentroEventos.Aplicacion.Interfaces;


namespace CentroEventos.Aplicacion.CasosDeUso.Persona
{
    public class PersonaBajaUseCase
    {
        private readonly IRepositorioPersona _repoPersona;
        private readonly IRepositorioEventoDeportivo _repoEventoDeportivo;
        private readonly IRepositorioReserva _repoReserva;
        private readonly IServicioAutorizacion _servicioAutorizacion;
        private readonly IServicioSesion _sesion;
        public PersonaBajaUseCase(IServicioAutorizacion servicio, IRepositorioPersona repoPersona, IRepositorioEventoDeportivo repoEvento, IRepositorioReserva repoReserva, IServicioSesion sesion)
        {
            _repoPersona = repoPersona;
            _servicioAutorizacion = servicio;
            _repoEventoDeportivo = repoEvento;
            _repoReserva = repoReserva;
            _sesion = sesion;
        }

        // Valido la existencia de la persona, lo hago aparte para a la hora de dar de baja una poersona pueda ver antes de todo su existencia  y si existe preguntar si se la quiere eliminar y sino mandar un mensaje de error.
        // Y para no tener que llamar al repositorio desde CentroEventos.UI
        public Entidades.Persona ObtenerParaBaja(int personaId)
        {
            var usuario = _sesion.UsuarioActual ?? throw new EntidadNotFoundException("No hay usuario en sesión.");

            if (!_servicioAutorizacion.PoseeElPermiso(usuario.Id, Enums.Permiso.UsuarioBaja))
                throw new FalloAutorizacionException("No tiene permiso para eliminar personas.");

            var persona = _repoPersona.ObtenerPorId(personaId)?? throw new EntidadNotFoundException($"No existe persona con Id {personaId}.");

            return persona;
        }


        public void Ejecutar(int personaId)
        {
            var persona = ObtenerParaBaja(personaId);

            // Verifico que la persona no sea responsable de algun evento deportivo
            var eventos = _repoEventoDeportivo.ListarTodos();
            foreach (var e in eventos)
            {
                if (e.ResponsableId == personaId)
                {
                    throw new OperacionInvalidaException("No se puede eliminar la persona ya que es responsable de un evento deportivo.");
                }
            }

            // Verifico que no existen reservas asociadas a esta persona
            var reservasAsociadas = _repoReserva.ObtenerReservasPorPersona(personaId);
            if (reservasAsociadas.Any())
            {
                throw new OperacionInvalidaException("No se puede eliminar la persona porque tiene reservas asociadas. ");
            }


            _repoPersona.Eliminar(personaId);
        }
    }
}