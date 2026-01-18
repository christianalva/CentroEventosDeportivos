namespace CentroEventos.Aplicacion.Interfaces
{
    using CentroEventos.Aplicacion.Entidades;


    public interface IRepositorioPersona
    {
        void Agregar(Persona persona); 
        void Eliminar(int Id); 

        void Modificar(Persona persona); 
        Persona? ObtenerPorId(int id);
        List<Persona> ListarTodos(); 

        // Metodos adicionales 
        bool ExistePorDni(string dni); 
        bool ExistePorEmail(string email); 
    }


}
