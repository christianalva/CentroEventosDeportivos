using CentroEventos.Aplicacion.Entidades;

namespace CentroEventos.Aplicacion.Validadores
{
    public class ValidadorEventoDeportivo
    {
        // valido los datos de un evento deportivo
        // retorno una lista con los errores encontrados, si la lista esta vacia, no hay errores
        public List<string> Validar(EventoDeportivo evento)
        {
            List<string> errores = new List<string>();
            // valido que el numbre y la descripcion no sean nulos o vacios
            if (string.IsNullOrWhiteSpace(evento.Nombre))
            {
                errores.Add("El nombre es Obligatorio.");
            }
            if (string.IsNullOrWhiteSpace(evento.Descripcion))
            {
                errores.Add("La descripcion es Obligatoria. ");
            }

            //verifico que la fecha y hora de inicio sean Actuales o posteriores a la fecha actual.
            if (evento.FechaHoraInicio < DateTime.Now)
            {
                errores.Add("La Fecha y Hora del Evento deben ser actuales o futuras.");
            }

            // compruebo que la duracion en horas y cupo maximo sean mayores a 0
            if (evento.DuracionHoras <= 0)
            {
                errores.Add("La duracion de Horas del Evento debe ser mayor a 0 (CERO). ");
            }
            if (evento.CupoMaximo <= 0)
            {
                errores.Add("El cupo Maximo del evento debe ser mayor que 0 (CERO). ");
            }

            return errores;

        }
    }
}