using CentroEventos.Aplicacion.Entidades;

namespace CentroEventos.Aplicacion.Interfaces
{
    public interface IRepositorioEventoDeportivo
    {
        void Agregar(EventoDeportivo evento);

        void Eliminar(int id);

        void Modificar(EventoDeportivo evento);

        EventoDeportivo? ObtenerPorId(int id);

        List<EventoDeportivo> ListarTodos();

        bool ExistePorNombre(string nombre);
        


    }
}