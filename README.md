# **Task Manager Project**

Este proyecto es una aplicación web para la gestión de tareas, desarrollada utilizando **ASP.NET Core 6.0** para el backend y **Razor Pages** para el frontend. La base de datos utilizada es **PostgreSQL**.

---

## **1. Configuración del Proyecto**

### **Requisitos Previos**
Antes de configurar el proyecto, asegúrate de tener instalados los siguientes programas:

- [SDK de .NET 8.0 o superior](https://dotnet.microsoft.com/download)
- [PostgreSQL 13 o superior](https://www.postgresql.org/download/)
- [Node.js (opcional, para manejar herramientas relacionadas)](https://nodejs.org/)

### **Base de Datos**
1. Crea la base de datos PostgreSQL utilizando el script proporcionado en la carpeta `/ScriptSQL` o ejecutando:
   ```sql
   CREATE DATABASE TaskManager;

2. Configura la conexión en el archivo appsettings.json del proyecto backend:
   ```json
   {
      "ConnectionStrings": {
        "DefaultConnection": "Host=localhost;Database=TaskManager;Username=postgres;Password=yourpassword"
      }
   }

3. (Opcional) Ejecuta las migraciones de Entity Framework Core si las usas:
  ```bash
   dotnet ef database update
  ```
### **Configuración del Backend**
1. Navega al directorio del backend:
   ```bash
   cd TaskManager
2. Restaura los paquetes NuGet:
   ```bash
   dotnet restore
3. Ejecuta el proyecto
  ```bash
  dotnet run
```

### **Configuración del Frontend**
1. Navega al directorio del frontend:
   ```bash
   cd TaskManagerFrontend
2. Restaura los paquetes NuGet:
   ```bash
   dotnet restore
3. Ejecuta el proyecto
  ```bash
  dotnet watch run
```
## **2. Ejecución del Proyecto**

1. Asegúrate de que el backend y el frontend estén ejecutándose en puertos diferentes.
2. Abre tu navegador y accede al frontend:
   ```
   http://localhost:5295/Auth/Login
   
3. Usa el backend (http://localhost:5258) para depuración si es necesario

## **3. Ejecución del Proyecto**
* Implementar validación adicional en el frontend para mejorar la experiencia del usuario.
* Agregar pruebas automatizadas para garantizar la calidad del código.
* Optimizar el manejo de errores tanto en el backend como en el frontend.
