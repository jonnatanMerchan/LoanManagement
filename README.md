# 🏦 Loan Management System

Sistema completo de gestión de préstamos desarrollado con .NET Core 8 y Angular 17+, implementando arquitectura limpia y mejores prácticas de desarrollo.

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![Angular](https://img.shields.io/badge/Angular-17+-DD0031?logo=angular)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-13+-336791?logo=postgresql)

## 📋 Tabla de Contenidos

- [Descripción](#-descripción)
- [Características](#-características)
- [Arquitectura](#️-arquitectura)
- [Requisitos Previos](#-requisitos-previos)
- [Instalación y Configuración](#-instalación-y-configuración)
- [Ejecución del Proyecto](#-ejecución-del-proyecto)
- [Estructura del Proyecto](#-estructura-del-proyecto)
- [API Endpoints](#-api-endpoints)
- [Tecnologías](#-tecnologías)

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
└── LoanManagement.Infrastructure/# Implementación (Repositories, DbContext)
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

Para ejecutar este proyecto necesitas tener instalado:

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (18+ LTS)
- [Angular CLI](https://angular.io/cli) (17+)
  ```bash
  npm install -g @angular/cli@17
  ```
- [PostgreSQL](https://www.postgresql.org/download/) (13+)
- [Git](https://git-scm.com/downloads)

## 📥 Instalación y Configuración

### 1. Clonar el Repositorio

Abre tu terminal y clona el proyecto:

```bash
git clone https://github.com/tu-usuario/loan-management-system.git
cd loan-management-system
```

### 2. Configurar la Base de Datos

Primero, asegúrate de que PostgreSQL esté corriendo. Puedes verificarlo ejecutando:

```bash
psql --version
```

Crea la base de datos:

```bash
psql -U postgres
CREATE DATABASE LoanManagementDb;
\q
```

### 3. Configurar el Backend

Ve a la carpeta del backend:

```bash
cd LoanManagement
```

Edita el archivo `appsettings.json` y actualiza la cadena de conexión con tus credenciales de PostgreSQL:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=LoanManagementDb;Username=postgres;Password=TU_PASSWORD"
  }
}
```

Restaura las dependencias del proyecto:

```bash
dotnet restore
```

Aplica las migraciones para crear las tablas en la base de datos:

```bash
dotnet ef database update
```

### 4. Configurar el Frontend

En otra terminal, ve a la carpeta del frontend:

```bash
cd loan-management-ui
```

Instala las dependencias de npm:

```bash
npm install
```

Edita el archivo `src/environments/environment.development.ts` para que apunte a tu API local:

```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000/api'
};
```

## 🚀 Ejecución del Proyecto

### Iniciar el Backend

En la terminal del backend, ejecuta:

```bash
dotnet run --project LoanManagement.API
```

El backend estará corriendo en: http://localhost:5000

Puedes verificar que funciona visitando: http://localhost:5000/swagger

### Iniciar el Frontend

En la terminal del frontend, ejecuta:

```bash
npm start
```

O si prefieres usar Angular CLI directamente:

```bash
ng serve
```

El frontend estará disponible en: http://localhost:4200

### Verificar que Todo Funciona

Abre tu navegador y ve a http://localhost:4200. Deberías ver la interfaz del sistema de gestión de préstamos. Prueba creando un cliente nuevo para verificar que la comunicación entre frontend y backend funciona correctamente.

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
└── LoanManagement.Infrastructure/
    ├── Data/                     # DbContext y configuraciones
    │   ├── AppDbContext.cs
    │   └── Configurations/       # Entity configurations
    ├── Repositories/             # Implementación de repositorios
    └── Migrations/               # Migraciones de EF Core
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

## 🛠️ Tecnologías

### Backend
- **.NET 8.0** - Framework principal
- **Entity Framework Core 8** - ORM
- **PostgreSQL** - Base de datos
- **AutoMapper** - Mapeo de objetos
- **FluentValidation** - Validaciones
- **Serilog** - Logging estructurado
- **Swagger/OpenAPI** - Documentación de API

### Frontend
- **Angular 17+** - Framework SPA
- **Angular Material** - Componentes UI
- **RxJS** - Programación reactiva
- **TypeScript** - Lenguaje tipado
- **Tailwind CSS** - Utilidades CSS
- **Chart.js** - Gráficos y visualizaciones

## 👥 Autores

- **Jonnatan Merchan** 