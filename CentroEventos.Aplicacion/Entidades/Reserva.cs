namespace CentroEventos.Aplicacion.Entidades
{
    public class Reserva
    {
        public int Id { get; set; }
        public int PersonaId { get; set; }
        public int EventoDeportivoId { get; set; } 
        public DateTime FechaAltaReserva { get; set; }
        public EstadoAsistencia EstadoAsistencia { get; set; }

        public Reserva(){}


        public override string ToString() {
            return $"ID de la reserva: {Id}, Id de la Persona: {PersonaId}, ID del Evento deportivo reservado: {EventoDeportivoId}, Fecha y Hora: {FechaAltaReserva:yyyy-MM-dd HH:mm}, Estado: {EstadoAsistencia}";
        }


    }

    
}

