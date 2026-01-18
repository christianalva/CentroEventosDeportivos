using CentroEventos.Aplicacion.Entidades;
using CentroEventos.Aplicacion.Excepciones;
using CentroEventos.Aplicacion.Interfaces;
using CentroEventos.Aplicacion.Validadores;
namespace CentroEventos.Aplicacion.CasosDeUso.EventosDeportivos
{
    public class EventoDeportivoAltaUseCase
    {
        private readonly IRepositorioEventoDeportivo _repoEvento;
        private readonly IRepositorioPersona _repoPersona;
        private readonly IServicioAutorizacion _servicioAutorizacion;
        private readonly ValidadorEventoDeportivo _validadorEvento;
        private readonly Usuario _usuarioActual;
        public EventoDeportivoAltaUseCase(IRepositorioEventoDeportivo repoEvento, IServicioAutorizacion servicio, ValidadorEventoDeportivo validadorEvento, IServicioSesion sesion, IRepositorioPersona repoPersona)
        {
            _repoEvento = repoEvento;
            _servicioAutorizacion = servicio;
            _validadorEvento = validadorEvento;
            _usuarioActual = sesion.UsuarioActual!;
            _repoPersona = repoPersona;
        }


        public void Ejecutar(EventoDeportivo evento)
        {
            if (!_servicioAutorizacion.PoseeElPermiso(_usuarioActual.Id, Enums.Permiso.EventoAlta)) 
                throw new FalloAutorizacionException();
            // Valido los datos del Evento
            var errores = _validadorEvento.Validar(evento);

            // Verifico que la persona exista
            var persona = _repoPersona.ObtenerPorId(evento.ResponsableId);
            if (persona == null) throw new EntidadNotFoundException($"La persona con Id {evento.ResponsableId} no existe.");

            if (errores.Any())
            {
                throw new ValidacionException(string.Join(" • ", errores));
            }
            _repoEvento.Agregar(evento);
        }
    }
}