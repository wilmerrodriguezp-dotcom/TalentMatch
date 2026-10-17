# Conectar MySQL en desarrollo

La API usa MySQL para validar usuarios y obtener sus roles. El servicio local debe escuchar en `localhost:3306`.

## Crear esquema y cuentas de prueba

Conéctate a MySQL con una cuenta administradora desde MySQL Workbench y ejecuta [database/create-users.sql](../database/create-users.sql) para crear la base y la tabla. Para poblar cuentas locales de prueba, ejecuta también [database/seed-development-users.sql](../database/seed-development-users.sql); ese segundo script no se usa en producción.

- `admin` / `123456`, rol `Admin`.
- `user` / `123456`, rol `User`.

Las contraseñas están almacenadas como PBKDF2-SHA256 con sales aleatorias; no se guardan en texto plano. Estas credenciales son solo para desarrollo.

Después, crea una cuenta de aplicación con permiso de lectura sobre la tabla:

```sql
CREATE USER 'talentmatch_app'@'localhost' IDENTIFIED BY 'REEMPLAZA_CON_UNA_CLAVE_LOCAL';
GRANT SELECT, INSERT ON TalentMatchDb.Users TO 'talentmatch_app'@'localhost';
```

Si el usuario ya existe, omite `CREATE USER` y actualiza su clave con `ALTER USER`.

## Configurar la API

Guarda la cadena de conexión en User Secrets, no en `appsettings.json`:

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Port=3306;Database=TalentMatchDb;User ID=talentmatch_app;Password=REEMPLAZA_CON_TU_CLAVE;SslMode=Preferred" --project backend/TalentMatch.Api
```

Inicia la API en `Development` y prueba `POST /api/v1/login` desde Swagger o `backend/TalentMatch.Api/requests.http`. El inicio falla con un mensaje explícito si falta la conexión.

En el despliegue Docker, Compose inicia MySQL con volumen persistente y configura la conexión de la API automáticamente. El primer administrador se crea desde `BOOTSTRAP_ADMIN_USER` y `BOOTSTRAP_ADMIN_PASSWORD`. Consulta [docs/publicacion-continua.md](publicacion-continua.md).