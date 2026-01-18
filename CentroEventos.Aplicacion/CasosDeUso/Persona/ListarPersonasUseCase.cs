using System.Runtime.CompilerServices;
using CentroEventos.Aplicacion.Entidades;
using CentroEventos.Aplicacion.Interfaces;


namespace CentroEventos.Aplicacion.CasosDeUso.Persona
{
    public class ListarPersonasUseCase{
        private readonly IRepositorioPersona _repoPersona;

        public ListarPersonasUseCase(IRepositorioPersona repoPersona)
        {
            _repoPersona = repoPersona;
        }

        public List<Entidades.Persona> Ejecutar()
        {
            return _repoPersona.ListarTodos();
        }
    }
}