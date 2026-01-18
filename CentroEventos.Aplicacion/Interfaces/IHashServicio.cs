namespace CentroEventos.Aplicacion.Interfaces

{
    public interface IHashServicio
    {
        public string CalcularHash(string password);
        public bool VerificarHash(string texto, string hashAlmacenado);
    }
}