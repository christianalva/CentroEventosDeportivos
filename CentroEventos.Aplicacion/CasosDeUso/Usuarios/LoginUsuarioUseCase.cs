using CentroEventos.Aplicacion.Interfaces;
using CentroEventos.Aplicacion.Entidades;
using CentroEventos.Aplicacion.Excepciones;

namespace CentroEventos.Aplicacion.CasosDeUso.Usuarios
{
    public class LoginUsuarioUseCase
    {
        private readonly IRepositorioUsuario _repoUsuario;
        private readonly IHashServicio _hash;
        private readonly IServicioAutorizacion _autorizacion;

        public class ResultadoLogin
        {
            // si el login es exitoso aca va el usuario encontrado
            public Usuario? Usuario { get; set; }
            // Lista de  mensajes de error, recopilados durante la validacion 
            public List<string> Errores { get; set; } = new();
        }


        // Constructor que resive las dependencias Inyeccion de dependencias
        public LoginUsuarioUseCase(IRepositorioUsuario repoUsuario, IHashServicio hash, IServicioAutorizacion autorizacion)
        {
            _repoUsuario = repoUsuario;
            _hash = hash;
            _autorizacion = autorizacion;
        }

        public ResultadoLogin Ejecutar(string email, string password)
        {
            //Creo un contenedor para el resultado
            var resultado = new ResultadoLogin();

            if (string.IsNullOrWhiteSpace(email))
                resultado.Errores.Add("El email no debe estar vacío.");
            if (string.IsNullOrWhiteSpace(password))
                resultado.Errores.Add("La contraseña no debe estar vacía.");

            if (resultado.Errores.Any())
                return resultado;

            var usuario = _repoUsuario.ObtenerPorEmail(email);
            if (usuario == null)
            {
                resultado.Errores.Add("Usuario no encontrado.");
                return resultado;
            }


            // Verifico que la contraseña pasada coinsida con la contraseña guardada 
            if (!_hash.VerificarHash(password, usuario.Password))
            {
                resultado.Errores.Add("Contraseña incorrecta.");
                return resultado;
            }

            resultado.Usuario = usuario;
            return resultado;
        }

    }
}