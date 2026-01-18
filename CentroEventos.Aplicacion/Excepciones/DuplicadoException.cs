namespace CentroEventos.Aplicacion.Excepciones
{
    public class DuplicadoException : Exception
    {
        public DuplicadoException() : base("Registro Duplicado")
        { }

        public DuplicadoException(string mensaje) : base(mensaje)
        { }

        public DuplicadoException(string mensaje, Exception innerException) : base(mensaje, innerException)
        { }
        
    }
}