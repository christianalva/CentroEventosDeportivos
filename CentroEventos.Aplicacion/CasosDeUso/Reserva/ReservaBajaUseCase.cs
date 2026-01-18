using CentroEventos.Aplicacion.Excepciones;
using CentroEventos.Aplicacion.Interfaces;

namespace CentroEventos.Aplicacion.CasosDeUso.Reserva
{
    public class ReservaBajaUseCase
    {
        private readonly IRepositorioReserva _repoReserva;
        private readonly IServicioAutorizacion _servicioAutorizacion;
        private readonly IServicioSesion _sesion;
        public ReservaBajaUseCase(IServicioAutorizacion servicio, IRepositorioReserva repoReserva, IServicioSesion sesion)
        {
            _repoReserva = repoReserva;
            _servicioAutorizacion = servicio;
            _sesion = sesion;
        }

        public Entidades.Reserva ObtenerParaBaja(int id)
        {
            var usuario = _sesion.UsuarioActual ?? throw new EntidadNotFoundException("No hay usuario en sesión.");

            if (!_servicioAutorizacion.PoseeElPermiso(usuario.Id, Enums.Permiso.ReservaBaja))
                throw new FalloAutorizacionException("No posee los permisos para realizar esta operacion.");

            // Verifico que la reserva exista
            var reserva = _repoReserva.ObtenerPorId(id);
            if (reserva == null)
            {
                throw new EntidadNotFoundException("La Reserva que se quiere eliminar no existe. ");
            }

            return reserva;
        }

        public void Ejecutar(int reservaId)
        {
            ObtenerParaBaja(reservaId);

            _repoReserva.Eliminar(reservaId);
        }
    }
}