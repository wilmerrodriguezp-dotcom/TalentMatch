# Publicación continua con Docker

El workflow `.github/workflows/publish.yml` publica las imágenes de API y frontend en GitHub Container Registry y, al subir cambios a `main`, las despliega en el servidor por SSH.

## Requisitos del servidor

- Linux con Docker Engine y el plugin Docker Compose v2.
- DNS del dominio apuntando al servidor.
- Puertos TCP 80 y 443 abiertos. Caddy obtiene y renueva el certificado TLS automáticamente.
- Una cuenta/clave SSH de despliegue con permiso para ejecutar Docker.

Crea el directorio de despliegue, por ejemplo `/opt/talentmatch`, y dentro crea un archivo `.env` con permisos restringidos:

```dotenv
IMAGE_OWNER=usuario-de-github-en-minusculas
DOMAIN_NAME=talentmatch.example.com
JWT_KEY=una-clave-aleatoria-de-al-menos-32-caracteres
JWT_ISSUER=TalentMatch
JWT_AUDIENCE=TalentMatchUsers
SWAGGER_ENABLED=true
BOOTSTRAP_ADMIN_USER=admin
BOOTSTRAP_ADMIN_PASSWORD=clave_aleatoria_que_retiraras_despues_del_primer_inicio
MYSQL_USER=talentmatch_app
MYSQL_PASSWORD=clave_hex_aleatoria_para_la_app
MYSQL_ROOT_PASSWORD=otra_clave_hex_aleatoria_para_root
```

Protege el archivo con `chmod 600 /opt/talentmatch/.env`. No lo guardes en Git.
Usa contraseñas aleatorias en hexadecimal para evitar caracteres de interpolación de Compose.

`BOOTSTRAP_ADMIN_USER` y `BOOTSTRAP_ADMIN_PASSWORD` crean el primer administrador si todavía no existe. Tras el primer arranque, elimina `BOOTSTRAP_ADMIN_PASSWORD` del `.env`; la cuenta permanece en MySQL y los reinicios no reemplazan su clave.

## Secretos de GitHub

En **Settings > Secrets and variables > Actions**, crea estos repository secrets:

- `DEPLOY_HOST`: IP o nombre del servidor.
- `DEPLOY_USER`: usuario SSH.
- `DEPLOY_PATH`: ruta absoluta del directorio de despliegue, por ejemplo `/opt/talentmatch`.
- `DEPLOY_SSH_KEY`: clave privada cuyo par público está autorizado en el servidor.
- `DEPLOY_KNOWN_HOSTS`: clave host verificada del servidor, obtenida con `ssh-keyscan -H <host>` y contrastada con la huella publicada por el proveedor.

El workflow usa `GITHUB_TOKEN` para publicar y descargar los paquetes de GHCR; no hace falta crear un token adicional.

## Ejecución

Al hacer push a `main`, GitHub Actions construye y publica ambas imágenes etiquetadas con el SHA del commit y `latest`, copia Compose, Caddyfile y el esquema de usuarios al servidor y ejecuta `docker compose pull` y `docker compose up -d`.

La aplicación queda disponible en `https://<DOMAIN_NAME>` y Swagger en `https://<DOMAIN_NAME>/swagger/`. También puedes iniciar el workflow manualmente desde la pestaña **Actions**; el despliegue se ejecuta cuando se lanza desde `main`.

MySQL usa el volumen persistente `mysql_data`. `create-users.sql` se ejecuta automáticamente solo al inicializar un volumen vacío; no borra ni vuelve a sembrar usuarios en despliegues posteriores.
