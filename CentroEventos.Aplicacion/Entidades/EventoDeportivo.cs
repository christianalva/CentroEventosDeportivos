
namespace CentroEventos.Aplicacion.Entidades
{
    

    public class EventoDeportivo
    {   
        // Propiedades
        public int Id { get; set; } // Asignado por el repositorio
        public string Nombre { get; set; } = "";
        public string Descripcion{ get; set; } = "";
        public DateTime FechaHoraInicio { get; set; }
        public double DuracionHoras { get; set; }
        public int CupoMaximo { get;  set; }
        public int ResponsableId { get; set; }

        public EventoDeportivo(){}


        public override string ToString()
        {
            return $"Nombre Evento: {Nombre}, Descripcion: {Descripcion}, Fecha y Hora: {FechaHoraInicio:yyyy-MM-dd HH:mm}, Duracion: {DuracionHoras}, Cupo Maximo: {CupoMaximo}, ID de la Persona a cargo:{ResponsableId}";
        }

    }
}

