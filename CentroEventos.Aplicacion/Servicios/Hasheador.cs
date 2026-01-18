using System.Security.Cryptography; // trae las clases de cifrado, incluida SHA256.
using System.Text; //para codificar el string de la contraseña como bytes (Encoding.UTF8).
using CentroEventos.Aplicacion.Interfaces;
namespace CentroEventos.Repositorios.Servicios
{
    public class Hasheador : IHashServicio
    {
        public string CalcularHash(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));

            return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
        }

        public bool VerificarHash(string texto, string hashAlmacenado)
        {

            string hashaIngresado = CalcularHash(texto);
            return hashaIngresado.Equals(hashAlmacenado, StringComparison.OrdinalIgnoreCase);
        }
    }
}

