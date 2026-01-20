# Descripción General
CentroEventosDeportivos es un sistema de gestión desarrollado en .NET 8, orientado a la administración de un Centro Deportivo Universitario. 
<br>
El sistema permite administrar personas, eventos deportivos, reservas y usuarios con un sistema de permisos. Fue desarrollado como trabajo final para el curso de Seminario de Lenguajes .NET.

# Funcionalidades Principales
<ul>
  <li>Gestión de Usuarios: Sistema de autenticación con contraseñas hasheadas (SHA-256)</li>
  <li>Control de Permisos: Sistema granular de autorización basado en permisos</li>
  <li>Gestión de Personas: CRUD completo de personas del centro deportivo</li>
  <li>Gestión de Eventos: Creación y administración de eventos deportivos con cupos</li>
  <li>Sistema de Reservas: Inscripción a eventos con control de cupos y asistencia</li>
  <li>Interfaz Web Moderna: Desarrollada con Blazor Server</li>
  <li>Persistencia en SQLite: Base de datos relacional con Entity Framework Core</li>
</ul>

# Arquitectura del Proyecto
El proyecto sigue los principios de Arquitectura Limpia (Clean Architecture), separando las responsabilidades en capas bien definidas y utilizando Inyección de Dependencias para el desacoplamiento.

<h3>📦 CentroEventos.Aplicacion </h3>
<p>Esta es la capa de dominio/aplicación y el corazón del sistema. NO tiene dependencias de otros proyectos de la solución.</p>
<br>

<ul>
  <li>🎯 <b>Casos de Uso:</b> Los casos de uso implementan la lógica de negocio.</li>
  <ul>
    <li>Recibe las dependencias necesarias por inyección de dependencias (constructor)</li>
    <li>Valida las entradas utilizando validadores</li>
    <li>Verifica permisos usando <b>IServicioAutorizacion</b></li>
    <li>Ejecuta la operación y persiste usando los repositorios</li>
    <li>Lanza excepciones personalizadas en caso de error</li>
    </ul>
</ul>
<ul>
  <li>🏛️ <b>Entidades:</b> Las entidades representan el modelo de dominio</li>
  <ul>
    <li>Usuario: Usuario del sistema con email, contraseña hasheada y lista de permisos</li>
    <li>Persona: Individuo del centro deportivo (participante o responsable)</li>
    <li>EventoDeportivo: Evento con fecha, hora, duración, cupo y responsable</li>
    <li>Reserva: Inscripción de una persona a un evento con estado de asistencia</li>
  </ul>
</ul>

<ul>
  <li>🔌 <b>Interfaces:</b> Define los contratos que deben implementar las capas externas</li>
  <ul>
    <li>IRepositorio: Contratos para persistencia (CRUD)</li>
    <li>IServicioAutorizacion: Contrato para verificar permisos</li>
    <li>IServicioSesion: Contrato para mantener la sesión del usuario</li>
    <li>IHashServicio: Contrato para hashear contraseñas</li>
  </ul>
</ul>

<ul>
  <li>⚙️ <b>Servicios:</b> Implementaciones de lógica compartida:</li>
  <b>ServicioAutorizacion</b>
  <ul>
    <li>IVerifica si un usuario tiene un permiso específico</li>
    <li>Consulta los permisos del usuario en el repositorio</li>
    <li>Los administradores tienen todos los permisos</li>
  </ul>
  <b>ServicioSesion (Scoped)</b>
  <ul>
    <li>Mantiene el usuario actual durante la sesión</li>
    <li>Se registra como Scoped para que cada usuario tenga su propia instancia</li>
    <li>Permite acceder al usuario logueado desde cualquier componente</li>
  </ul>
  <b>Hasheador</b>
  <ul>
    <li>Implementa el hasheo de contraseñas usando SHA-256</li>
    <li>Irreversible: no se puede obtener la contraseña original</li>
    <li>Al validar, se hashea la entrada y se compara con el hash almacenado</li>
  </ul>
</ul>

<ul>
  <li>✅ <b>Validadores:</b> Cada entidad tiene su validador que verifica:</li>
  <b>ValidadorUsuario</b>
  <ul>
    <li>Nombre, apellido y contraseña requeridos</li>
    <li>Email único en el sistema</li>
  </ul>
  <b>ValidadorPersona</b>
  <ul>
    <li>Todos los campos requeridos</li>
    <li>DNI y Email únicos</li>
  </ul>
  <b>ValidadorEventoDeportivo</b>
  <ul>
    <li>Nombre y descripción no vacíos</li>
    <li>FechaHoraInicio >= fecha actual</li>
    <li>CupoMaximo y DuracionHoras > 0</li>
    <li>ResponsableId existe en Personas</li>
  </ul>
  <b>ValidadorReserva</b>
  <ul>
    <li>PersonaId y EventoDeportivoId existen</li>
    <li>No permitir reservas duplicadas</li>
    <li>Verificar cupo disponible</li>
  </ul>
</ul>


<ul>
  <li>🚨 <b>Excepciones Personalizadas:</b></li>
  <ul>
    <li>ValidacionException: Datos inválidos</li>
    <li>EntidadNotFoundException: ID no encontrado</li>
    <li>FalloAutorizacionException: Usuario sin permiso</li>
    <li>CupoExcedidoException: Sin cupo en evento</li>
    <li>DuplicadoException: Entidad duplicada</li>
    <li>OperacionInvalidaException: Operación no permitida</li>
  </ul>
</ul>

<ul>
  <li>🚨 <b>Enums</b></li>
  <ul>
    <li>Permiso</li>
      <p>  EventoAlta, EventoModificacion, EventoBaja <br>
           ReservaAlta, ReservaModificacion, ReservaBaja <br>
           UsuarioAlta, UsuarioModificacion, UsuarioBaja</p>
    <li>EstadoAsistencia</li>
      <p>Pendiente, Presente, Ausente</p>
  </ul>
</ul>

<h3>💾 CentroEventos.Repositorios </h3>
<p>Esta capa implementa la persistencia de datos usando Entity Framework Core con SQLite.</p>

<ul>
  <li>📊 <b>CentroEventosContext</b> Es el DbContext que representa la base de datos</li>
  Configuraciones importantes:
  <ul>
    <li>Índices únicos en Email (Usuario y Persona)</li>
    <li>Índice único en DNI (Persona)</li>
    <li>Índice único compuesto (PersonaId, EventoDeportivoId) para evitar reservas duplicadas</li>
    <li>Journal mode DELETE para reflejar cambios inmediatamente en SQLite</li>
  </ul>
</ul>

<ul>
  <li>🗄️ <b>Repositorios</b> Cada repositorio implementa las operaciones CRUD</li>
</ul>

<h3>🖥️ CentroEventos.UI</h3>
<p>Interfaz de usuario desarrollada con Blazor Server.</p>

<ul>
  <li>🎨 <b> Blazor Server</b> Blazor Server es un framework para crear aplicaciones web interactivas con C# en lugar de JavaScript:</li>
  Características:
  <ul>
    <li>Renderizado en el servidor</li>
    <li>Comunicación en tiempo real con SignalR</li>
    <li>Interactividad completa del lado del cliente</li>
    <li>Componentes reutilizables (.razor)</li>
  </ul>
</ul>

<ul>
  <li>🔐 <b>Sistema de Sesión</b> El ServicioSesion se registra como Scoped, lo que significa:</li>
  <ul>
    <li>Una instancia por usuario/conexión</li>
    <li>Persiste durante toda la sesión de Blazor</li>
    <li>Se reinicia si el usuario recarga la página</li>
    <li>No persiste entre pestañas</li>
  </ul>
</ul>

# Cómo Ejecutar el Proyecto
<h3>Requisitos</h3>
<ul>
  <li>.NET 8.0 SDK - [Descarga](https://dotnet.microsoft.com/es-es/download/dotnet/8.0).</li>
  <li>SQLite Database Manager - [Descarga](https://sqlite.org/download.html).</li>
  <li>Visual Studio 2022 / VS Code / Rider</li>
</ul>

<h3>Pasos</h3>

1. **Clonar el repositorio**: 

```bash
  git clone https://github.com/christianalva/CentroEventosDeportivos.git
```
2. **Navegue hasta el directorio de la solución**:

```bash
  cd CentroEventosDeportivos
```

3. **Navegue hasta el UI del proyecto**:
```bash
   cd CentroEventos.UI
```

4. **Ejecute la aplicación**:
   
```bash
   dotnet run
```
