# Rover Mission Planner

Aplicación Full Stack desarrollada para planificar y gestionar tareas asignadas a rovers en Marte.

## Descripción

Esta solución permite:
- Ingresar tareas para rovers con validaciones avanzadas.
- Visualizar la agenda diaria de un rover específico.
- Evitar solapamiento de tareas mediante validación de horarios.
- Interfaz SPA profesional desarrollada en Angular 17+.

##  Tecnologías Utilizadas

### Backend (.NET 8 - API REST)
- ASP.NET Core 8
- Arquitectura en capas (Domain, Application, Infrastructure, API)
- FluentValidation
- Middleware de manejo de excepciones
- Tests unitarios (xUnit / NUnit)

### Frontend (Angular 17+)
- Angular Standalone Components
- Formularios reactivos (`FormGroup`)
- Validaciones en frontend
- Bootstrap 5 para diseño visual

---

## ⚙️ Instrucciones para correr el proyecto

### Backend

1. Ir al directorio del backend:
   ```bash
   cd RoverMissionPlanner.API

    Restaurar dependencias y ejecutar:

    dotnet restore
    dotnet run

    API disponible en: https://localhost:7025

Frontend

    Ir al directorio del frontend:

cd RoverMissionPlannerFrontend

Instalar dependencias y ejecutar:

    npm install
    ng serve

    SPA disponible en: http://localhost:4200
 Funcionalidades clave

Ingreso de tareas para un rover (con validaciones)

Visualización diaria por rover

Prevención de solapamientos en horarios

Carga dinámica de rovers desde API

Uso de Bootstrap para diseño profesional

Formateo de fechas según es-CL

    Pruebas unitarias con más del 70% de cobertura

 Validaciones Implementadas
Backend (.NET)

    Solapamiento entre tareas (por roverName)

    Duración mínima mayor a 0

    Coordenadas válidas (latitud y longitud)

    Formato y fecha de inicio correctos

Frontend (Angular)

    Todos los campos requeridos

    Mensajes de error personalizados

    Fechas y horas con formato amigable
 Tests

Para correr las pruebas unitarias en backend:

cd RoverMissionPlanner.Tests
dotnet test

Capturas

Incluye capturas del listado, formulario y validaciones si se requiere en la entrega (screenshots en archivo word).