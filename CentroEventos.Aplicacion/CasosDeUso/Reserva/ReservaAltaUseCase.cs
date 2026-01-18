using System.Security.Cryptography.X509Certificates;
using CentroEventos.Aplicacion.Entidades;
using CentroEventos.Aplicacion.Excepciones;
using CentroEventos.Aplicacion.Interfaces;
using CentroEventos.Aplicacion.Validadores;

namespace CentroEventos.Aplicacion.CasosDeUso.Reserva
{
    public class ReservaAltaUseCase
    {
        // CAMPOS PRIVADOS PARA ALMACENAR LAS DEPENDENCIAS (REPOSITORIOS Y SERVICIOS DE AUTORIZACION)
        private readonly IRepositorioReserva _repoReserva;
        private readonly IServicioAutorizacion _servicioAutorizacion;
        private readonly ValidadorReserva _validadorReserva;
        private readonly IServicioSesion _sesion;

        //CONSTRUCTOR PARA INYECTAR LAS DEPENDENCIAS NECESARIAS
        public ReservaAltaUseCase(IRepositorioReserva repoReserva, IServicioAutorizacion servicioAutorizacion, ValidadorReserva validadorReserva, IServicioSesion sesion)
        {
            //Asigno cada uno de los repositorios
            _repoReserva = repoReserva;
            _servicioAutorizacion = servicioAutorizacion;
            _validadorReserva = validadorReserva;
            _sesion = sesion;
        }

        public void Ejecutar(Entidades.Reserva datosReserva)
        {
            var usuario = _sesion.UsuarioActual ?? throw new EntidadNotFoundException("No hay usuario en sesión.");

            //1 - Verifico si el usuario esta autorizado a realizar una operacion
            if (!_servicioAutorizacion.PoseeElPermiso(usuario.Id, Enums.Permiso.ReservaAlta))
            {
                throw new FalloAutorizacionException();
            }

            //Llamo al validador de Reserva para hacer las validaciones pedidas.
            var errores = _validadorReserva.Validar(datosReserva.PersonaId, datosReserva.EventoDeportivoId);
            if (errores.Any())
            {
                throw new ValidacionException(string.Join(" • ", errores));
            }
            
            // SI TODO ESTA OK COMPLETO LOS DATOSRESERVA.
            datosReserva.FechaAltaReserva = DateTime.Now; // asigno la fecha actual como la fecha de alta de la reserva 
            datosReserva.EstadoAsistencia = EstadoAsistencia.Pendiente;
            _repoReserva.Agregar(datosReserva);// agrego la reserva al sistema utilizando el repositorio reservas
        }
    }
}