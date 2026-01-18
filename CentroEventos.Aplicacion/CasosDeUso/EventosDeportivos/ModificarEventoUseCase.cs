using CentroEventos.Aplicacion.Entidades;
using CentroEventos.Aplicacion.Excepciones;
using CentroEventos.Aplicacion.Interfaces;
using CentroEventos.Aplicacion.Validadores;


namespace CentroEventos.Aplicacion.CasosDeUso.EventosDeportivos
{
    public class ModificarEventosUseCase
    {
        private readonly IRepositorioEventoDeportivo _repoEvento;
        private readonly IServicioAutorizacion _servicioAutorizacion;
        private readonly ValidadorEventoDeportivo _validadorEvento;
        private readonly IServicioSesion _sesion;
        public ModificarEventosUseCase(IServicioAutorizacion servicio, IRepositorioEventoDeportivo repoEvento, ValidadorEventoDeportivo validadorEvento, IServicioSesion sesion)
        {
            _repoEvento = repoEvento;
            _servicioAutorizacion = servicio;
            _validadorEvento = validadorEvento;
            _sesion = sesion;
        }

        public EventoDeportivo ObtenerParaModificar(int eventoId)
        {
           var usuario = _sesion.UsuarioActual ?? throw new EntidadNotFoundException("No hay usuario en sesión.");

            if (!_servicioAutorizacion.PoseeElPermiso(usuario.Id, Enums.Permiso.EventoModificacion))
                throw new FalloAutorizacionException("No tiene permiso para modificar Eventos Deportivos.");

            var evento = _repoEvento.ObtenerPorId(eventoId) ?? throw new EntidadNotFoundException($"No existe el Evento Deportivo con Id {eventoId}.");

            return evento;
        }
        public void Ejecutar(EventoDeportivo datosModificados)
        {
            

            var copia = new EventoDeportivo
            {
                Id = datosModificados.Id,
                Nombre = datosModificados.Nombre,
                Descripcion = datosModificados.Descripcion,
                FechaHoraInicio = datosModificados.FechaHoraInicio,
                DuracionHoras = datosModificados.DuracionHoras,
                CupoMaximo = datosModificados.CupoMaximo,
                ResponsableId = datosModificados.ResponsableId
            };

            // Verifico que la fecha del evento deportivo sea mayor a la actual, regla: No puede modificarse un EventoDeportivo cuya FechaHoraInicio haya expirado (es decir, no puede modificarse un evento pasado). 
            if (copia.FechaHoraInicio < DateTime.Now) // DEBERIA HACER ESTA VALIDACION EN EL EVENTO ORIGINAL NO EN LA COPIA
            {
                throw new OperacionInvalidaException("El Evento Deportivo ya expiro.");
            }

            // Valido los datos del Evento.
            var errores = _validadorEvento.Validar(copia);
            // Si se encontraron errores de validacion, lanzo una excepcion
            if (errores.Any())
            {
                throw new ValidacionException(string.Join(" • ", errores));
            }

            var eventoOriginal = ObtenerParaModificar(datosModificados.Id);

            eventoOriginal.Id = datosModificados.Id;
            eventoOriginal.Nombre = datosModificados.Nombre;
            eventoOriginal.Descripcion = datosModificados.Descripcion;
            eventoOriginal.FechaHoraInicio = datosModificados.FechaHoraInicio;
            eventoOriginal.DuracionHoras = datosModificados.DuracionHoras;
            eventoOriginal.ResponsableId = datosModificados.ResponsableId;

            // Si cumple con todas las validaciones modfico el evento deportivo.
            _repoEvento.Modificar(eventoOriginal);
        }
        
    }
}