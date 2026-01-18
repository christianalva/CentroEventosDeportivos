namespace CentroEventos.Aplicacion.Excepciones
{
    public class ValidacionException : Exception
    {
        public List<string> Errores { get; } = new List<string>();
        public ValidacionException() : base("Error de validación: algún dato obligatorio está ausente o tiene formato incorrecto.")
        { }

        public ValidacionException(List<string> errores) : base("Se encontraron errores de validación.")
        {
            Errores = errores;
        }

        public ValidacionException(string mensaje) : base(mensaje)
        { }


        public ValidacionException(string mensaje, Exception innerException) : base(mensaje, innerException)
        { }
        
    }
}