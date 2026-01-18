namespace CentroEventos.Aplicacion.Enums
{
    // Defino los permisos Disponibles en el sistema
    public enum Permiso
    {
        // Permisos sobre Eventos Deportivos
        EventoAlta,          // Puede crear nuevos eventos deportivos en el centro
        EventoModificacion,  // Puede modificar los detalles de los eventos deportivos
        EventoBaja,          // Puede eliminar eventos deportivos del centro

        // Permisos sobre Reservas
        ReservaAlta,         
        ReservaModificacion, 
        ReservaBaja,         

        // Permisos sobre Personas
        UsuarioAlta,         
        UsuarioModificacion, 
        UsuarioBaja,    

        // Permisos sobre Usuarios      
        UsuarioLectura,
        GestionUsuario


    }
}