<div align="center">

# ⚙️ SISTEM-API

### Backend del sistema SISTEM-DATE, construido con ASP.NET Core y Clean Architecture

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://learn.microsoft.com/dotnet/csharp/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-Neon-4169E1?style=for-the-badge&logo=postgresql&logoColor=white)](https://neon.tech/)
[![Entity Framework Core](https://img.shields.io/badge/EF_Core-8.0.14-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://learn.microsoft.com/ef/core/)
[![Swagger](https://img.shields.io/badge/Swagger-Swashbuckle-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)](https://swagger.io/)
[![Render](https://img.shields.io/badge/Deploy-Render-46E3B7?style=for-the-badge&logo=render&logoColor=white)](https://render.com/)

</div>

---

## 📋 Tabla de contenidos

- [Acerca del proyecto](#-acerca-del-proyecto)
- [Stack tecnológico](#-stack-tecnológico)
- [Arquitectura](#-arquitectura)
- [Estructura del proyecto](#-estructura-del-proyecto)
- [Requisitos previos](#-requisitos-previos)
- [Instalación](#-instalación)
- [Configuración de la base de datos](#-configuración-de-la-base-de-datos)
- [Servidor de desarrollo](#-servidor-de-desarrollo)
- [Migraciones (EF Core)](#-migraciones-ef-core)
- [Swagger / Documentación de la API](#-swagger--documentación-de-la-api)
- [Tests](#-tests)
- [Conexión con el frontend](#-conexión-con-el-frontend)
- [Ramas del repositorio](#-ramas-del-repositorio)
- [Despliegue](#-despliegue)
- [Estado actual / Roadmap](#-estado-actual--roadmap)

---

## 📖 Acerca del proyecto

**SISTEM-API** es el backend del sistema **SISTEM-DATE**, construido con **ASP.NET Core (.NET 8)** usando **Minimal APIs** y **Clean Architecture** aplicada desde el inicio, con separación estricta de capas: `Domain` no depende de nada, `Application` no conoce EF Core, `Infrastructure` implementa persistencia con **Npgsql**, y `Api` expone endpoints delgados sin lógica de negocio.

> 🚧 **Estado actual:** en desarrollo activo. Actualmente existe un módulo de ejemplo end-to-end (`Dish`) como piloto de la arquitectura, más un módulo base de autenticación (`Auth`, `User`).

---

## 🛠 Stack tecnológico

| Tecnología | Versión | Uso |
|---|---|---|
| <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/dotnetcore/dotnetcore-original.svg" width="20"/> .NET | 8.0 | Framework principal |
| <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/csharp/csharp-original.svg" width="20"/> C# | — | Lenguaje base |
| Npgsql.EntityFrameworkCore.PostgreSQL | 8.0.10 | Proveedor EF Core para PostgreSQL |
| Microsoft.EntityFrameworkCore.Tools | 8.0.14 | Herramientas de migraciones |
| Microsoft.EntityFrameworkCore.Design | 8.0.14 | Soporte de diseño para EF Core |
| Microsoft.AspNetCore.OpenApi | 8.0.25 | Especificación OpenAPI |
| <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/swagger/swagger-original.svg" width="20"/> Swashbuckle.AspNetCore | 6.6.2 | UI de Swagger |
| <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/postgresql/postgresql-original.svg" width="20"/> PostgreSQL (Neon) | serverless | Base de datos |

---

## 🏛 Arquitectura

Clean Architecture en 4 proyectos, con dependencias en una sola dirección:

```
SistemApi.Api  →  SistemApi.Infrastructure  →  SistemApi.Application  →  SistemApi.Domain
```

- **`SistemApi.Domain`** — Cero dependencias externas. Modelos ricos (constructores con validación, métodos de dominio), interfaces de repositorio, `Result<T>`.
- **`SistemApi.Application`** — Sin paquetes de EF Core. Casos de uso / servicios que dependen solo de interfaces del `Domain`.
- **`SistemApi.Infrastructure`** — Implementación de persistencia (EF Core + Npgsql), `DbContext`, entidades, configuraciones, repositorios, seguridad (JWT, hashing, Google token validation).
- **`SistemApi.Api`** — Minimal API endpoints delgados, sin lógica de negocio. Registra Swagger, CORS e inyección de dependencias.

> `SistemApi.Domain` no referencia ningún otro proyecto — el núcleo del negocio está totalmente aislado de infraestructura.

---

## 📁 Estructura del proyecto

```
SISTEM-API/
├── SistemApi.Domain/
│   ├── Commom/                  # Result<T>, HttpStatusCode
│   ├── Models/                  # DishModel, UserModel (modelos ricos)
│   ├── Repositories/            # IDishRepository, IUserRepository
│   └── Services/                # IJwtTokenGenerator, IPasswordHasher, IGoogleTokenValidator
│
├── SistemApi.Application/
│   ├── Services/                # DishService, AuthService (CRUD/casos de uso)
│   └── Dto/                     # AuthDtos, etc.
│
├── SistemApi.Infrastructure/
│   ├── Database/EntityFramework/
│   │   ├── Context/             # SistemApiDbContext
│   │   ├── Entities/            # DishEntity, UserEntity
│   │   ├── Configurations/      # DishConfiguration, UserConfiguration
│   │   ├── Repositories/        # DishRepository, UserRepository
│   │   └── Migrations/          # Migraciones de EF Core
│   ├── Security/                # JwtTokenGenerator, PasswordHasher, GoogleTokenValidator
│   └── Ioc/                     # DependencyInjection.cs
│
├── SistemApi.Api/
│   ├── Endpoints/
│   │   ├── Dish/                # DishGroupEndpoint (GET, POST, PUT, DELETE)
│   │   ├── Auth/                # AuthGroupEndpoint
│   │   └── Common/              # ResultExtensions (ToApiResult())
│   ├── Program.cs               # CORS, Swagger, AddInfrastructure
│   └── appsettings.json
│
└── SistemApi.slnx
```

---

## ✅ Requisitos previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Cuenta en [Neon](https://neon.tech/) (PostgreSQL serverless)
- (Opcional) [Rider](https://www.jetbrains.com/rider/) o [Visual Studio](https://visualstudio.microsoft.com/)

---

## 📦 Instalación

```bash
git clone https://github.com/chipans/SISTEM-API.git
cd SISTEM-API
dotnet restore
```

---

## 🗄 Configuración de la base de datos

> ⚠️ **Nunca** pongas la cadena de conexión real en `appsettings.json`. Usa **user-secrets** en local y variables de entorno en producción.

**En local:**

```bash
cd SistemApi.Api
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=<tu-host-neon>;Database=<db>;Username=<user>;Password=<pass>;SSL Mode=Require;Channel Binding=Require"
```

**En producción (Render):**

Configura la variable de entorno:

```
ConnectionStrings__DefaultConnection=<tu-connection-string-de-neon>
```

---

## 🚀 Servidor de desarrollo

```bash
cd SistemApi.Api
dotnet run
```

Puertos de desarrollo:

| Protocolo | URL |
|---|---|
| HTTP | `http://localhost:5246` |
| HTTPS | `https://localhost:7020` |

> Si usas certificados HTTPS de desarrollo por primera vez: `dotnet dev-certs https --trust`

---

## 🔄 Migraciones (EF Core)

Crear una nueva migración:

```bash
dotnet ef migrations add NombreDeLaMigracion --project SistemApi.Infrastructure --startup-project SistemApi.Api
```

Aplicar migraciones a la base de datos:

```bash
dotnet ef database update --project SistemApi.Infrastructure --startup-project SistemApi.Api
```

---

## 📑 Swagger / Documentación de la API

Con el servidor corriendo, abre:

```
https://localhost:7020/swagger
```

Ahí puedes explorar y probar todos los endpoints disponibles (`api/dish`, `api/auth`, etc.).

---

## 🧪 Tests

```bash
dotnet test
```

---

## 🔗 Conexión con el frontend

Este backend expone la API consumida por [**SISTEM-WEB**](../SISTEM-WEB) (Angular).

- CORS habilitado para `http://localhost:4200` en desarrollo
- Contrato de respuesta estandarizado: `ApiResponse<T>` (`isSuccess`, `data`, `errors`)

```
[SISTEM-WEB] --HTTP--> [SISTEM-API] --Npgsql--> [Neon PostgreSQL]
   Netlify                 Render
```

---

## 🌿 Ramas del repositorio

| Rama | Propósito |
|---|---|
| `main` | Código estable / producción |
| `dev` | Desarrollo activo |
| `testdev` | Pruebas antes de pasar a `main` |

Flujo de trabajo: `feature/*` → `dev` → `testdev` → `main`

---

## ☁️ Despliegue

Este proyecto se despliega en **[Render](https://render.com/)** mediante Docker, conectado a **[Neon](https://neon.tech/)** como base de datos PostgreSQL serverless.

> 🚧 Dockerfile y configuración de despliegue en Render aún pendientes.

---

## 🗺 Estado actual / Roadmap

- [x] Arquitectura Clean Architecture en 4 capas
- [x] Módulo de ejemplo end-to-end (`Dish`)
- [x] Conexión verificada a Neon (migraciones aplicadas)
- [x] Base de autenticación (`Auth`, `User`, JWT, hashing, Google token validation)
- [ ] Dockerfile y despliegue en Render
- [ ] Prueba de flujo completo navegador → Angular → API → Neon
- [ ] Módulos de negocio adicionales

<div align="center">

---

Hecho con 💜 usando ASP.NET Core

</div>