namespace CentroEventos.Aplicacion.Excepciones
{
    public class FalloAutorizacionException : Exception
    {
        public FalloAutorizacionException() : base("El usuario no tiene permiso para realizar esta operacion.")
        { } 

        public FalloAutorizacionException(string mensaje) : base(mensaje)
        { }

        public FalloAutorizacionException(string mensaje, Exception innerException) : base(mensaje, innerException)
        { }
    }
}