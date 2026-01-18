using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using CentroEventos.Aplicacion.Entidades;

namespace CentroEventos.Aplicacion.Validadores
{
    public class ValidadorPersona {

        // valido los datos de una persona.
        // devuelvo una lista con los errores encontrados. si la lista esta vacia, los datos son validos.
        public List<string> Validar(Persona persona)
        {
            List<string> errores = new List<string>();

            // verifico si DNI, nombre o apellido no son nulos o estan vacios
            // IsNullOrWhiteSpace me devuelve true en caso de que lo sean y si asi es lanza la excepción
            if (string.IsNullOrWhiteSpace(persona.Dni))
            {
                errores.Add("El Dni es obligatorio.");
            }
            if (string.IsNullOrWhiteSpace(persona.Nombre))
            {
                errores.Add("El Nombre es Obligatorio. ");
            }
            if (string.IsNullOrWhiteSpace(persona.Apellido))
            {
                errores.Add("El apellido es Obligatorio.");
            }
            if (string.IsNullOrWhiteSpace(persona.Email))
            {
                errores.Add("El Email es Obligatorio.");
            }
            // Verifico el formato del Email a ver si es correcto.
            // El metodo Contais me devuelve true si @ se encuentra en la cadena, caso contrario devuelve false.
            if (!persona.Email.Contains("@"))
            {
                errores.Add("El Email no Contiene el formato deseado.");
            }

            return errores;
        }
    }
}