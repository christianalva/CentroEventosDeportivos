using CentroEventos.Aplicacion.Entidades;

namespace CentroEventos.Aplicacion.Validadores
{
    public class ValidadorUsuario
    {

        // Uso el parametro esModificacion para a la hora de modificar el Usuario permitir que este se pueda quedar con el password que ya tenia si lo desea
        public List<string> Validar(Usuario usuario, bool esModificacion = false)
        {
            List<string> errores = new List<string>();

            if (string.IsNullOrWhiteSpace(usuario.Nombre))
                errores.Add("El Nombre es Obligatorio.");

            if (string.IsNullOrWhiteSpace(usuario.Apellido))
                errores.Add("El Apellido es Obligatorio.");

            if (string.IsNullOrWhiteSpace(usuario.Email))
                errores.Add("El Email es Obligatorio.");
            else if (!usuario.Email.Contains("@"))
                errores.Add("El Email no es valido.");

            if (!esModificacion)
            {
                if (string.IsNullOrWhiteSpace(usuario.Password))
                    errores.Add("La Contraseña es Obligatoria.");
                else if (usuario.Password.Length < 6)
                    errores.Add("La Contraseña debe ser de al menos 6 caracteres.");
            }
            else if (!string.IsNullOrWhiteSpace(usuario.Password) && usuario.Password.Length < 6)
            {
                errores.Add("La nueva Contraseña debe tener al menos 6 caracteres.");
            }


            return errores;
        }
    }
}