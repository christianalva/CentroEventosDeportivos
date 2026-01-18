using System.Data;
using CentroEventos.Aplicacion.Entidades;
using CentroEventos.Aplicacion.Excepciones;
using CentroEventos.Aplicacion.Interfaces;
using CentroEventos.Aplicacion.Validadores;

namespace CentroEventos.Aplicacion.CasosDeUso.Reserva
{
    public class ModificarReservaUseCase
    {
        private readonly IRepositorioReserva _repoReserva;
        private readonly IRepositorioEventoDeportivo _repoEvento;
        private readonly IServicioAutorizacion _servicioAutorizacion;
        private readonly IServicioSesion _sesion;
        private readonly ValidadorReserva _validador;
        public ModificarReservaUseCase(IServicioAutorizacion servicio, IRepositorioReserva repoReserva, IRepositorioEventoDeportivo repoEvento, IServicioSesion sesion, ValidadorReserva validador)
        {
            _repoReserva = repoReserva;
            _servicioAutorizacion = servicio;
            _repoEvento = repoEvento;
            _sesion = sesion;
            _validador = validador;
        }

        public Entidades.Reserva ObtenerParaModificar(int id)
        {
            var usuario = _sesion.UsuarioActual ?? throw new EntidadNotFoundException("No hay usuario en sesión.");

            if (!_servicioAutorizacion.PoseeElPermiso(usuario.Id, Enums.Permiso.ReservaModificacion))
                throw new FalloAutorizacionException("No posee el permiso para realizar esta Operacion.");

            // Verifico que la reserva exista
            var reservaExistente = _repoReserva.ObtenerPorId(id);
            if (reservaExistente == null)
                throw new EntidadNotFoundException("La reserva no existe.");
            return reservaExistente;
        }
        public void Ejecutar(Entidades.Reserva datosModificados)
        {

            var copia = new Entidades.Reserva
            {
                Id = datosModificados.Id,
                PersonaId = datosModificados.PersonaId,
                EventoDeportivoId = datosModificados.EventoDeportivoId,
                FechaAltaReserva = datosModificados.FechaAltaReserva,
                EstadoAsistencia = datosModificados.EstadoAsistencia
            };

            

            var errores = _validador.Validar(copia.PersonaId, copia.EventoDeportivoId);
            if (errores.Any()) throw new ValidacionException(string.Join(" • ", errores));

            var original = ObtenerParaModificar(datosModificados.Id);
            // Verifico que el evento asiciado a la reserva todavia no haya finalizado
            var evento = _repoEvento.ObtenerPorId(original.EventoDeportivoId);
            if (evento == null)
                throw new EntidadNotFoundException("El evento deportivo asociado a la reserva no existe.");
            if (evento.FechaHoraInicio < DateTime.Now)
                throw new OperacionInvalidaException("No se puede modificar la reserva de un evento que ya inició o expiró.");

            original.PersonaId = datosModificados.PersonaId;
            original.EventoDeportivoId = datosModificados.EventoDeportivoId;
            original.FechaAltaReserva = datosModificados.FechaAltaReserva;
            original.EstadoAsistencia = datosModificados.EstadoAsistencia;


            _repoReserva.Modificar(original);
        }
    }
}