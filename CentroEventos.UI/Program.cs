using CentroEventos.UI.Components;
using CentroEventos.Aplicacion.CasosDeUso.Usuarios;
using System.Data.Common;
using CentroEventos.Aplicacion.CasosDeUso.Persona;
using CentroEventos.Aplicacion.CasosDeUso.EventosDeportivos;
using CentroEventos.Aplicacion.CasosDeUso.Reserva;
using CentroEventos.Aplicacion.Interfaces;
using CentroEventos.Repositorios.Servicios;
using CentroEventos.Repositorios;  
using Microsoft.EntityFrameworkCore;
using CentroEventos.Aplicacion.Servicios;
using CentroEventos.Aplicacion.CasosDeUso;
using CentroEventos.Aplicacion.Validadores;

var builder = WebApplication.CreateBuilder(args);

// Registrar Servicios y componetes de UI
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Registro el DbContext en el contenedor de dependencias
// Se registra como scoped por defecto 
// Configurándolo para usar SQLite con el archivo "centroeventos.db".
// Por defecto al Usar AddDbContex, el servicio se registra con ciclo de vida scoped: se crea una instancia del DbContext por cada request HTTP(por conexion de usuario)
// Esto permite inyectar CentroEventosContext en servicios y componentes                                
builder.Services.AddDbContext<CentroEventosContext>(opts =>
    opts.UseSqlite("Data Source=centroeventos.db"));


// Registro en el contenedor DI todos los Repositorios y ServicioSesion como Scoped para que: 
// - Cada Usuario reciba su propia instancia (Evito compartir datos de un usuario con otro)
// - Cuando cierre la pestaña o termine la peticion, el framework la descarte y libere recursos
builder.Services.AddScoped<IServicioSesion, ServicioSesion>();
builder.Services.AddScoped<IRepositorioUsuario, UsuarioRepositorio>();
builder.Services.AddScoped<IRepositorioPersona, PersonaRepositorio>();
builder.Services.AddScoped<IRepositorioEventoDeportivo, EventoDeportivoRepositorio>();
builder.Services.AddScoped<IRepositorioReserva, ReservaRepositorio>();
builder.Services.AddScoped<IHashServicio, Hasheador>();
builder.Services.AddScoped<IServicioAutorizacion, ServicioAutorizacion>();

// contenedor DI Transient de Usuarios
builder.Services.AddTransient<UsuarioAltaUseCase>();
builder.Services.AddTransient<UsuarioBajaUseCase>();
builder.Services.AddTransient<LoginUsuarioUseCase>();
builder.Services.AddTransient<ModificarUsuarioUseCase>();
builder.Services.AddTransient<ModificarPermisosUseCase>();
builder.Services.AddTransient<ListarUsuarioUseCase>();
builder.Services.AddTransient<ValidadorUsuario>();


// Personas
builder.Services.AddTransient<PersonaAltaUseCase>();
builder.Services.AddTransient<PersonaBajaUseCase>();
builder.Services.AddTransient<ModificarPersonaUseCase>();
builder.Services.AddTransient<ListarPersonasUseCase>();
builder.Services.AddTransient<ValidadorPersona>();
builder.Services.AddTransient<ContarPersonas>();

// Eventos Deportivos
builder.Services.AddTransient<EventoDeportivoAltaUseCase>();
builder.Services.AddTransient<EventoDeportivoBajaUseCase>();
builder.Services.AddTransient<ListarEventosUseCase>();
builder.Services.AddTransient<ListarEventosConCupoDisponibleUseCase>();
builder.Services.AddTransient<ListarAsistenciaAEventoUseCase>();
builder.Services.AddTransient<ModificarEventosUseCase>();
builder.Services.AddTransient<ValidadorEventoDeportivo>();
builder.Services.AddTransient<EventosDeportivosActivos>();

// Reservas
builder.Services.AddTransient<ReservaAltaUseCase>();
builder.Services.AddTransient<ReservaBajaUseCase>();
builder.Services.AddTransient<ModificarReservaUseCase>();
builder.Services.AddTransient<ListarReservasUseCase>();
builder.Services.AddTransient<ValidadorReserva>();
builder.Services.AddTransient<ReservasRealizadas>();

// Construyo la app
var app = builder.Build();


// Inicializo la base de datos
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CentroEventosContext>();

    // Creo el DB si no existe
    context.Database.EnsureCreated(); 

    var connection = context.Database.GetDbConnection();
    connection.Open();
    using (var command = connection.CreateCommand())
    {
        //Especifico el registro de transacciones que se utilizara
        // Al utilizar DELETE en SQlite los cambios realizados se reflejan inmediatamente en la base de datos principal
        command.CommandText = "PRAGMA journal_mode=DELETE;";
        command.ExecuteNonQuery();
    }
} 



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}
app.UseRouting();
app.UseAntiforgery();
app.UseStaticFiles();



app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
