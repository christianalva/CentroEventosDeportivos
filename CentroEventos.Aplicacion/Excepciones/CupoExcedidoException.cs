namespace CentroEventos.Aplicacion.Excepciones
{
    public class CupoExcedidoException : Exception
    {
        public CupoExcedidoException() : base("No hay cupo disponible en el evento deportivo. ")
        { }

        public CupoExcedidoException(string mensaje) : base(mensaje)
        { }

        public CupoExcedidoException(string mensaje, Exception innerException) : base(mensaje, innerException)
        { }
    }
}