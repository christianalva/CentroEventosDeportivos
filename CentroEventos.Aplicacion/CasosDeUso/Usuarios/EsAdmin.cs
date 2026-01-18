using CentroEventos.Aplicacion.Entidades;
using CentroEventos.Aplicacion.Enums;

namespace CentroEventos.Aplicacion.CasosDeUso.Usuarios
{
  public static class UsuarioAdmin
  {
    public static bool EsAdmin(this Usuario u)
    {
        // Todos es un array con todos los valores definidos en el enum Permiso
        var todos = Enum.GetValues<Permiso>().Cast<Permiso>();
        
        //Recorro el array todos, si el usurario posee todos los permisos de dicho array quiere decir que es admin, sino no 
      return todos.All(p => u.Permisos.Contains(p));
    }
  }
}
    