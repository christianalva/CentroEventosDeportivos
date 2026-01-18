using Microsoft.EntityFrameworkCore;
using CentroEventos.Aplicacion.Entidades;

namespace CentroEventos.Repositorios

{
    public class CentroEventosContext : DbContext
    {
        // Constructor que recibe las opciones de configuracion (cadena de conexion, proveedor, etc..) y las pasa a la base DbContext
        public CentroEventosContext(DbContextOptions<CentroEventosContext> options) : base(options) { }


        // La propiedad Usuarios representa la coleccion de todas las instancias de la entidad Usuario
        // Esto permite realizar operaciones CRUD sobre los usuarios (agregar, actualizar, eliminar)
        // Cada DbSet<T> representa una tabla en la base de datos
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Persona> Personas { get; set; } 
        public DbSet<EventoDeportivo> Eventos { get; set; } 
        public DbSet<Reserva> Reservas { get; set; } 



        // OnModelCreating se ejecuta cuando EF construye el modelo a partir de las clases
        // Configuro indices y restricciones especificas (no es tan util)
        protected override void OnModelCreating(ModelBuilder modelBuilder) //  OnModelCreating para especificar distintos aspectos sobre cómo se realiza el mapeo entre el modelo y la base de datos (sacado de teoria 10)
        {
            // Configuracion para la entidad Usuario: indice único en Email
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Configuracion para la entidad Persona: indices unicos en Dni y Email
            modelBuilder.Entity<Persona>()
                .HasIndex(p => p.Dni)
                .IsUnique();
            modelBuilder.Entity<Persona>()
                .HasIndex(p => p.Email)
                .IsUnique();

            // Configuracion para la entidad Reserva, indice unico para evitar reservas duplicadas
            modelBuilder.Entity<Reserva>()
                .HasIndex(r => new { r.PersonaId, r.EventoDeportivoId })
                .IsUnique();
            
            // Configuraciones adicionales si fuera necesario.
            base.OnModelCreating(modelBuilder);

        }

        // Metodo se usa para configurar el proveedor de dato
        // Aunque yo estoy creando la base de datos desde program.cs 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=centroeventos.db");
            }
        }

    }
}