namespace CentroEventos.Aplicacion.Excepciones
{
    public class EntidadNotFoundException : Exception
    {
        public EntidadNotFoundException() : base("La entidad con el ID especificado no fue encontrada. ")
        { }

        public EntidadNotFoundException(string mensaje) : base(mensaje)
        { }

        public EntidadNotFoundException(string mensaje, Exception innerException) : base(mensaje, innerException)
        { }
    }
}