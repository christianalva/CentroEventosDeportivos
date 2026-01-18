namespace CentroEventos.Aplicacion.Entidades 
{
    
    public class Persona
    {
        // Propiedades
        public int Id { get; set; } // el repositorio asignara el id
        public string Dni { get; set; } = "";
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; }     = "";    
        public string Email { get; set; } = "";
        public string Telefono { get; set; } = "";
        public Persona(){ }


        public override string ToString()
        {
            return $"ID: {Id}, DNI: {Dni}, Nombre: {Nombre}, Apellido: {Apellido}, Email: {Email}, Telefono: {Telefono}" ;
        }

    }
}
