# Arquitectura en capas paraProductos

## Objetivo

Definir la estructura base para una API REST de gestión de productos con separación de responsabilidades por capas y uso de DTOs, repository, servicios, controladores, inyección de dependencias y middleware global de errores.

## Capa de dominio

La capa de dominio contiene las entidades de negocio y las reglas de dominio que no dependen de infraestructura ni de framework.

### Entidad Producto

```csharp
public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int Stock { get; set; }
}
```

## DTOs

Se separan los modelos de lectura y escritura para evitar exponer internamente la entidad del dominio.

```csharp
public class ProductoResponseDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int Stock { get; set; }
}
```

```csharp
public class CrearProductoDto
{
    [Required]
    [MinLength(2)]
    public string Nombre { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal Precio { get; set; }

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }
}
```

## Capa de persistencia

La capa de datos encapsula la lógica de almacenamiento y acceso a SQL.

```csharp
public interface IProductoRepository
{
    Task<IEnumerable<Producto>> ObtenerTodosAsync();
    Task<Producto?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(Producto producto);
    Task ActualizarAsync(Producto producto);
    Task EliminarAsync(int id);
}
```

La implementación concreta usa SQL y no incluye reglas de negocio.

## Capa de servicio

El servicio aplica reglas de validación y coordina con el repositorio.

```csharp
public interface IProductoService
{
    Task<IEnumerable<ProductoResponseDto>> ObtenerTodosAsync();
    Task<ProductoResponseDto?> ObtenerPorIdAsync(int id);
    Task<ProductoResponseDto> CrearAsync(CrearProductoDto dto);
}
```

Reglas de negocio:

- Nombre obligatorios.
- Precio mayor que cero.
- Stock no puede ser negativo.

## Endpoints REST

```
GET /api/v1/productos
POST /api/v1/productos
```

### POST ejemplo

```http
POST /api/v1/productos
Content-Type: application/json

{
  "nombre": "Laptop Gamer",
  "precio": 2499.99,
  "stock": 10
}
```

### Respuesta esperada

- `201 Created`
- Cabecera `Location: /api/v1/productos/{id}`

## Middleware global

Se recomienda un middleware que capture excepciones no controladas y responda con ProblemDetails con formato RFC 7807.

```csharp
app.UseMiddleware<ExceptionHandlingMiddleware>();
```

## Autenticación y autorización

Se valida JWT en las solicitudes:

```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => { /* configuración */ });
```

Los endpoints protegidos se configuran con políticas de roles:

```csharp
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});
```

## Inyección de dependencias

```csharp
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IProductoService, ProductoService>();
```

## Arquitectura recomendada

```text
TalentMatch.Api
├── Controllers
├── Middleware
├── Program.cs
└── Auth

TalentMatch.Application
├── DTOs
├── Interfaces
├── Services
└── Shared

TalentMatch.Domain
└── Entities

TalentMatch.Infrastructure
├── Data
├── Repositories
└── Services
```

## Stack principal

- ASP.NET Core
- SQL Server / PostgreSQL / MySQL
- JWT
- Repository Pattern
- RFC 7807
- React + TypeScript
- HTTPS / JSON
