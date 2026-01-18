namespace CentroEventos.Aplicacion.Excepciones
{
    public class OperacionInvalidaException : Exception
    {
        public OperacionInvalidaException() : base("Operacion no permitida por las reglas del negocio.")
        { }

        public OperacionInvalidaException(string mensaje) : base(mensaje)
        { }

        public OperacionInvalidaException(string mensaje, Exception innerException) : base(mensaje, innerException)
        { }
    }
}