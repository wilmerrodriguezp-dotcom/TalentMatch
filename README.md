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

El cliente web consume la API mediante HTTPS y JSON para autenticar usuarios y mostrar el estado de la sesión.

## Endpoints principales

- `POST /api/v1/login`
- `GET /api/v1/admin-check`
- `GET /api/v1/user-check`


## Credenciales de prueba

- Usuario administrador: `admin` / `123456`
- Usuario normal: `user` / `123456`

Estas credenciales solo existen al ejecutar el seed de desarrollo descrito en [docs/conectar-base-datos.md](docs/conectar-base-datos.md).

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
├── database/
│   ├── create-users.sql
│   └── seed-development-users.sql
├── frontend/
│   ├── src/
│   ├── index.html
│   ├── package.json
│   ├── tsconfig.json
│   ├── tsconfig.app.json
│   ├── tsconfig.node.json
│   └── vite.config.ts
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
- MySQL
- JWT
- Repository pattern
- RFC 7807
- Inyección de dependencias
- Swagger


## Conexión a MySQL

Sigue [docs/conectar-base-datos.md](docs/conectar-base-datos.md) para crear el esquema local y configurar la cadena mediante User Secrets.

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

## Publicación continua

El workflow para publicar imágenes en GHCR y desplegarlas en un servidor Docker se configura siguiendo [docs/publicacion-continua.md](docs/publicacion-continua.md).

## Pruebas con Swagger o HTTP

Puedes probar el flujo de login y permisos con el archivo:

- [backend/TalentMatch.Api/requests.http](backend/TalentMatch.Api/requests.http)

## Flujos de validación

- Admin puede acceder a rutas protegidas de administración.
- User y Admin pueden acceder a rutas protegidas para usuarios.
- El middleware global captura errores y responde con `ProblemDetails`.
- La autenticación se valida con JWT en todas las solicitudes protegidas.

## Siguientes pasos recomendados

1. Añadir entidades persistentes para proyectos, habilidades y equipos.
2. Integrar un servicio real de correo electrónico.
3. Ampliar la capa de autorización con políticas más específicas por módulo.
