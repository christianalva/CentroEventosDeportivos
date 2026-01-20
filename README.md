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

<h3> CentroEventos.Aplicacion </h3>
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
  <ul>
    <li></li>
    <li></li>
    <li></li>
    <li></li>
  </ul>
</ul>
