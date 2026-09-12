# TalentMatch

Repositorio para almacenar código para proyecto de ingeniería web TalentMatch.

## Objetivo general

TalentMatch es una plataforma web para conectar estudiantes, profesores y administradores alrededor de proyectos académicos, habilidades, intereses y equipos colaborativos.

## Arquitectura propuesta

La solución está organizada en dos grandes bloques:

- Backend en ASP.NET Core con arquitectura en capas.
- Frontend en React + TypeScript.

### Backend

Se implementa una API REST en ASP.NET Core con estructura basada en capas:

- Dominio: entidades y reglas de negocio.
- Application: DTOs, interfaces, servicios.
- Infrastructure: repositorios y acceso a SQL.
- API: controladores, configuración y middleware global.

### Frontend

El cliente web consume la API mediante HTTPS y JSON, presenta los productos, captura datos del usuario y gestiona respuestas y errores.

## Endpoints principales

- `POST /api/v1/login`
- `GET /api/v1/productos`
- `POST /api/v1/productos`
- `GET /api/v1/productos/admin-check`
- `GET /api/v1/productos/user-check`

## Reglas de negocio del módulo de productos

- Nombre obligatorio.
- Precio mayor que cero.
- Stock no negativo.

## Credenciales de prueba

- Usuario administrador: `admin` / `123456`
- Usuario normal: `user` / `123456`

## Estructura del proyecto

```text
TalentMatch/
├── backend/
│   ├── TalentMatch.Api/
│   ├── TalentMatch.Application/
│   ├── TalentMatch.Domain/
│   ├── TalentMatch.Infrastructure/
│   ├── TalentMatch.sln
│   └── TalentMatch.Api/requests.http
├── frontend/
│   ├── src/
│   ├── index.html
│   ├── package.json
│   ├── tsconfig.json
│   ├── tsconfig.app.json
│   ├── tsconfig.node.json
│   └── vite.config.ts
├── database/
│   └── create-productos-table.sql
├── docs/
│   └── arquitectura-productos.md
├── Diagrama de arquitectura.mmd
├── Diagrama de clases.mmd
├── Diagrama de componentes.mmd
├── Diagrama de contexto.mmd
├── Diagrama de flujo.mmd
├── README.md
└── .gitignore
```

## Tecnologías principales

- React + TypeScript
- ASP.NET Core
- SQL Server / SQL
- JWT
- Repository pattern
- RFC 7807
- Inyección de dependencias
- Swagger

## Documentación adicional

La guía de la capa de productos y la arquitectura asociada se encuentran en [docs/arquitectura-productos.md](docs/arquitectura-productos.md).

## Configuración de la base de datos

1. Ejecuta el script [database/create-productos-table.sql](database/create-productos-table.sql).
2. Asegúrate de que SQL Server esté disponible en `localhost`.
3. Ajusta la cadena de conexión si usas otro usuario o servidor.

## Ejecución del backend

```bash
cd backend
 dotnet restore
 dotnet build
 dotnet run --project TalentMatch.Api
```

La API quedará disponible en:

- `https://localhost:7001`
- Swagger: `https://localhost:7001/swagger`

## Ejecución del frontend

```bash
cd frontend
npm install
npm run dev
```

La aplicación quedará disponible en:

- `http://localhost:5173`

## Pruebas con Swagger o HTTP

Puedes probar el flujo de login y permisos con el archivo:

- [backend/TalentMatch.Api/requests.http](backend/TalentMatch.Api/requests.http)

## Flujos de validación

- Admin puede ver y crear productos.
- User puede ver productos, pero no crear.
- El middleware global captura errores y responde con `ProblemDetails`.
- La autenticación se valida con JWT en todas las solicitudes protegidas.

## Siguientes pasos recomendados

1. Conectar la aplicación a SQL Server real si la tienes configurada.
2. Añadir persistencia completa con más entidades del negocio.
3. Integrar un servicio real de correo electrónico.
4. Ampliar la capa de autorización con políticas más específicas por módulo.
