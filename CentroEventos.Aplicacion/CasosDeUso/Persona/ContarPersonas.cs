// Lo uso para la pantalla de inicio nomas
using CentroEventos.Aplicacion.Interfaces;
using CentroEventos.Aplicacion.Entidades;
namespace CentroEventos.Aplicacion.CasosDeUso.Persona
{
    public class ContarPersonas
    {
        private readonly IRepositorioPersona _repoPersona;

        public ContarPersonas(IRepositorioPersona repoPersonas)
        {
            _repoPersona = repoPersonas;
        }

        public int Ejecutar()
        {
            return _repoPersona.ListarTodos().Count();
        }
    }
}