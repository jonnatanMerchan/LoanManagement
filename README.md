# 🏦 Loan Management System

Sistema completo de gestión de préstamos desarrollado con .NET Core 8 y Angular 17+, implementando arquitectura limpia y mejores prácticas de desarrollo.

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![Angular](https://img.shields.io/badge/Angular-17+-DD0031?logo=angular)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-13+-336791?logo=postgresql)
![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?logo=docker)

## 📋 Tabla de Contenidos

- [Descripción](#-descripción)
- [Características](#-características)
- [Arquitectura](#️-arquitectura)
- [Requisitos Previos](#-requisitos-previos)
- [Instalación y Configuración](#-instalación-y-configuración)
- [Ejecución del Proyecto](#-ejecución-del-proyecto)
- [Estructura del Proyecto](#-estructura-del-proyecto)
- [API Endpoints](#-api-endpoints)
- [Testing](#-testing)
- [Tecnologías](#-tecnologías)
- [Contribución](#-contribución)

## 📖 Descripción

Sistema integral para la gestión de préstamos que permite administrar clientes, solicitudes de préstamos, aprobaciones y registro de pagos. Desarrollado con arquitectura limpia, separación de responsabilidades y principios SOLID.

## ✨ Características

### Gestión de Clientes
- ✅ Crear, editar y eliminar clientes
- ✅ Búsqueda y filtrado de clientes
- ✅ Validación de datos de contacto
- ✅ Soft delete para mantener integridad referencial

### Gestión de Préstamos
- ✅ Solicitud de préstamos con cálculo automático de cuotas
- ✅ Flujo de aprobación/rechazo
- ✅ Estados: Pending, Approved, Rejected, Active, Completed
- ✅ Visualización detallada del historial
- ✅ Filtrado por estado y cliente

### Gestión de Pagos
- ✅ Registro de pagos con validación
- ✅ Cálculo automático de balance restante
- ✅ Historial completo de transacciones
- ✅ Referencias únicas de transacción

## 🏗️ Arquitectura

### Backend - Clean Architecture
```
LoanManagement/
├── LoanManagement.API/           # Capa de presentación (Controllers, Middleware)
├── LoanManagement.Application/   # Lógica de negocio (Services, DTOs, Validators)
├── LoanManagement.Domain/        # Entidades y contratos (Entities, Interfaces)
├── LoanManagement.Infrastructure/# Implementación (Repositories, DbContext)
└── LoanManagement.Tests/         # Pruebas unitarias y de integración
```

### Frontend - Feature-Based Architecture
```
loan-management-ui/
├── src/app/
│   ├── core/          # Servicios singleton, guards, interceptors
│   ├── shared/        # Componentes, pipes y directivas compartidas
│   ├── features/      # Módulos de funcionalidades (customers, loans, payments)
│   └── models/        # Interfaces y tipos TypeScript
```

## 🔧 Requisitos Previos

Asegúrate de tener instalado lo siguiente antes de comenzar:

### Opción 1: Con Docker (Recomendado)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (20.10+)
- [Docker Compose](https://docs.docker.com/compose/install/) (2.0+)

### Opción 2: Instalación Local
- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (18+ LTS)
- [Angular CLI](https://angular.io/cli) (17+)
  ```bash
  npm install -g @angular/cli@17
  ```
- [PostgreSQL](https://www.postgresql.org/download/) (13+) o [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads)
- [Git](https://git-scm.com/downloads)

### Herramientas Opcionales
- [Visual Studio 2022](https://visualstudio.microsoft.com/) o [VS Code](https://code.visualstudio.com/)
- [Postman](https://www.postman.com/) para testing de APIs
- [pgAdmin](https://www.pgadmin.org/) para gestión de PostgreSQL

## 📥 Instalación y Configuración

### 1. Clonar el Repositorio

```bash
git clone https://github.com/tu-usuario/loan-management-system.git
cd loan-management-system
```

### 2. Configuración con Docker (Recomendado)

#### Paso 1: Configurar Variables de Entorno

Crea un archivo `.env` en la raíz del proyecto:

```env
# Database
POSTGRES_USER=loanadmin
POSTGRES_PASSWORD=SecurePass123!
POSTGRES_DB=LoanManagementDB
DB_PORT=5432

# API
API_PORT=5000
ASPNETCORE_ENVIRONMENT=Development

# Frontend
FRONTEND_PORT=4200
```

#### Paso 2: Levantar los Servicios

```bash
# Construir y levantar todos los contenedores
docker-compose up --build

# O en modo detached (segundo plano)
docker-compose up -d
```

Espera a que todos los servicios estén listos (aproximadamente 2-3 minutos en el primer arranque).

#### Paso 3: Verificar los Servicios

- **Frontend:** http://localhost:4200
- **Backend API:** http://localhost:5000/swagger
- **Base de Datos:** localhost:5432

```bash
# Ver logs de los contenedores
docker-compose logs -f

# Ver estado de los servicios
docker-compose ps
```

#### Paso 4: Aplicar Migraciones (Automático)

Las migraciones se aplican automáticamente al iniciar el contenedor del backend. Si necesitas aplicarlas manualmente:

```bash
docker-compose exec api dotnet ef database update
```

### 3. Configuración Local (Sin Docker)

#### Backend

##### Paso 1: Restaurar Dependencias

```bash
cd LoanManagement
dotnet restore
```

##### Paso 2: Configurar Base de Datos

Edita `appsettings.Development.json` en el proyecto `LoanManagement.API`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=LoanManagementDB;Username=tu_usuario;Password=tu_password"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

##### Paso 3: Aplicar Migraciones

```bash
cd LoanManagement.API
dotnet ef database update --project ../LoanManagement.Infrastructure

# O si tienes dotnet-ef instalado globalmente
dotnet tool install --global dotnet-ef
dotnet ef database update
```

##### Paso 4: Ejecutar el Backend

```bash
cd LoanManagement.API
dotnet run

# O con hot reload
dotnet watch run
```

La API estará disponible en: http://localhost:5000/swagger

#### Frontend

##### Paso 1: Instalar Dependencias

```bash
cd loan-management-ui
npm install
```

##### Paso 2: Configurar API Endpoint

Edita `src/environments/environment.development.ts`:

```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000/api'
};
```

##### Paso 3: Ejecutar el Frontend

```bash
npm start

# O directamente con Angular CLI
ng serve
```

La aplicación estará disponible en: http://localhost:4200

## 🚀 Ejecución del Proyecto

### Comandos Útiles de Docker

```bash
# Iniciar servicios
docker-compose up

# Detener servicios
docker-compose down

# Reiniciar un servicio específico
docker-compose restart api

# Ver logs de un servicio
docker-compose logs -f frontend

# Reconstruir imágenes
docker-compose up --build

# Limpiar todo (contenedores, volúmenes, imágenes)
docker-compose down -v --rmi all
```

### Comandos Útiles del Backend

```bash
# Compilar
dotnet build

# Ejecutar tests
dotnet test

# Crear nueva migración
dotnet ef migrations add NombreMigracion --project LoanManagement.Infrastructure

# Revertir última migración
dotnet ef database update PreviousMigration

# Generar script SQL
dotnet ef migrations script

# Verificar migraciones pendientes
dotnet ef migrations list
```

### Comandos Útiles del Frontend

```bash
# Desarrollo
npm start

# Build para producción
npm run build

# Tests unitarios
npm test

# Tests con coverage
npm run test:coverage

# Linting
npm run lint

# Formatear código
npm run format
```

## 📁 Estructura del Proyecto

### Backend

```
LoanManagement/
├── LoanManagement.API/
│   ├── Controllers/              # Controladores REST
│   │   ├── CustomersController.cs
│   │   ├── LoansController.cs
│   │   └── PaymentsController.cs
│   ├── Middleware/               # Middleware personalizado
│   ├── Program.cs                # Configuración de la aplicación
│   └── appsettings.json         # Configuración
│
├── LoanManagement.Application/
│   ├── DTOs/                     # Data Transfer Objects
│   ├── Services/                 # Servicios de aplicación
│   ├── Interfaces/               # Contratos de servicios
│   └── Validators/               # FluentValidation validators
│
├── LoanManagement.Domain/
│   ├── Entities/                 # Entidades del dominio
│   │   ├── Customer.cs
│   │   ├── Loan.cs
│   │   └── Payment.cs
│   ├── Enums/                    # Enumeraciones
│   └── Interfaces/               # Repositorios (contratos)
│
├── LoanManagement.Infrastructure/
│   ├── Data/                     # DbContext y configuraciones
│   │   ├── AppDbContext.cs
│   │   └── Configurations/       # Entity configurations
│   ├── Repositories/             # Implementación de repositorios
│   └── Migrations/               # Migraciones de EF Core
│
└── LoanManagement.Tests/
    ├── Unit/                     # Tests unitarios
    └── Integration/              # Tests de integración
```

### Frontend

```
loan-management-ui/
├── src/
│   ├── app/
│   │   ├── core/
│   │   │   ├── services/         # Servicios HTTP
│   │   │   ├── guards/           # Route guards
│   │   │   ├── interceptors/     # HTTP interceptors
│   │   │   └── models/           # Modelos compartidos
│   │   │
│   │   ├── features/
│   │   │   ├── customers/        # Módulo de clientes
│   │   │   │   ├── components/
│   │   │   │   └── services/
│   │   │   ├── loans/            # Módulo de préstamos
│   │   │   └── payments/         # Módulo de pagos
│   │   │
│   │   ├── shared/
│   │   │   ├── components/       # Componentes reutilizables
│   │   │   ├── pipes/            # Pipes personalizados
│   │   │   └── directives/       # Directivas
│   │   │
│   │   └── app.component.ts      # Componente raíz
│   │
│   ├── assets/                   # Recursos estáticos
│   ├── environments/             # Configuración de entornos
│   └── styles.scss               # Estilos globales
│
├── angular.json                  # Configuración de Angular
├── package.json                  # Dependencias npm
└── tsconfig.json                 # Configuración TypeScript
```

## 🔌 API Endpoints

### Customers

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/customers` | Listar clientes (paginado) |
| GET | `/api/customers/{id}` | Obtener cliente por ID |
| POST | `/api/customers` | Crear nuevo cliente |
| PUT | `/api/customers/{id}` | Actualizar cliente |
| DELETE | `/api/customers/{id}` | Eliminar cliente (soft delete) |
| GET | `/api/customers/search?q={query}` | Buscar clientes |

### Loans

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/loans` | Listar préstamos (filtros opcionales) |
| GET | `/api/loans/{id}` | Obtener préstamo por ID |
| POST | `/api/loans` | Crear solicitud de préstamo |
| PUT | `/api/loans/{id}/approve` | Aprobar préstamo |
| PUT | `/api/loans/{id}/reject` | Rechazar préstamo |
| GET | `/api/loans/customer/{customerId}` | Préstamos de un cliente |

### Payments

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/payments/loan/{loanId}` | Pagos de un préstamo |
| POST | `/api/payments` | Registrar nuevo pago |
| GET | `/api/payments/{id}` | Obtener pago por ID |

### Ejemplos de Uso

#### Crear Cliente

```bash
curl -X POST http://localhost:5000/api/customers \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "Juan",
    "lastName": "Pérez",
    "email": "juan.perez@email.com",
    "phone": "+57 300 1234567",
    "address": "Calle 123 #45-67"
  }'
```

#### Crear Préstamo

```bash
curl -X POST http://localhost:5000/api/loans \
  -H "Content-Type: application/json" \
  -d '{
    "customerId": "uuid-del-cliente",
    "amount": 10000000,
    "interestRate": 1.5,
    "termInMonths": 12
  }'
```

## 🧪 Testing

### Backend Tests

```bash
# Ejecutar todos los tests
dotnet test

# Con cobertura
dotnet test /p:CollectCoverage=true

# Tests de un proyecto específico
dotnet test LoanManagement.Tests/LoanManagement.Tests.csproj

# Ver resultados detallados
dotnet test --logger "console;verbosity=detailed"
```

### Frontend Tests

```bash
# Tests unitarios
npm test

# Tests en modo watch
npm run test:watch

# Cobertura
npm run test:coverage

# E2E tests
npm run e2e
```

## 🛠️ Tecnologías

### Backend
- **.NET 8.0** - Framework principal
- **Entity Framework Core 8** - ORM
- **PostgreSQL** - Base de datos
- **AutoMapper** - Mapeo de objetos
- **FluentValidation** - Validaciones
- **Serilog** - Logging estructurado
- **Swagger/OpenAPI** - Documentación de API
- **xUnit** - Testing framework
- **Moq** - Mocking para tests

### Frontend
- **Angular 17+** - Framework SPA
- **Angular Material** - Componentes UI
- **RxJS** - Programación reactiva
- **TypeScript** - Lenguaje tipado
- **Tailwind CSS** - Utilidades CSS
- **Chart.js** - Gráficos y visualizaciones
- **Jasmine + Karma** - Testing

### DevOps
- **Docker** - Contenedorización
- **Docker Compose** - Orquestación local
- **GitHub Actions** - CI/CD
- **Nginx** - Servidor web para frontend

## 🐛 Solución de Problemas

### Error: No se puede conectar a la base de datos

```bash
# Verificar que PostgreSQL esté corriendo
docker-compose ps

# Ver logs de la base de datos
docker-compose logs db

# Reiniciar el servicio de base de datos
docker-compose restart db
```

### Error: Puerto 5000 ya está en uso

```bash
# Cambiar el puerto en docker-compose.yml
ports:
  - "5001:80"  # Usar 5001 en lugar de 5000

# O detener el proceso que usa el puerto
# Windows
netstat -ano | findstr :5000
taskkill /PID <PID> /F

# Linux/Mac
lsof -ti:5000 | xargs kill -9
```

### Error: npm install falla

```bash
# Limpiar caché de npm
npm cache clean --force

# Eliminar node_modules y package-lock.json
rm -rf node_modules package-lock.json

# Reinstalar
npm install
```

### Error: Migraciones no se aplican

```bash
# Verificar estado de migraciones
dotnet ef migrations list

# Aplicar manualmente
cd LoanManagement.API
dotnet ef database update --verbose

# Si persiste, eliminar la base de datos y recrear
dotnet ef database drop
dotnet ef database update
```

## 📝 Variables de Entorno

### Backend (appsettings.json)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "tu_connection_string"
  },
  "Jwt": {
    "Key": "tu_jwt_secret_key",
    "Issuer": "LoanManagementAPI",
    "Audience": "LoanManagementClient"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

### Frontend (environment.ts)

```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000/api',
  pageSize: 10
};
```

## 🤝 Contribución

1. Fork el proyecto
2. Crea tu feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push al branch (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📄 Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo `LICENSE` para más detalles.

## 👥 Autores

- **Tu Nombre** - *Desarrollo inicial* - [tu-github](https://github.com/tu-usuario)

## 📞 Soporte

Para soporte, envía un email a soporte@loanmanagement.com o abre un issue en GitHub.

---

⭐ Si este proyecto te fue útil, considera darle una estrella en GitHub!