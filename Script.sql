--Crear la Base de Datos
CREATE DATABASE TaskManager
WITH 
    OWNER = postgres
    ENCODING = 'UTF8'
    CONNECTION LIMIT = -1;


-- Cambiar el contexto a la base de datos TaskManager
    \c TaskManager; 

--Crear la Tabla de Usuarios
CREATE TABLE Users (
    Id SERIAL PRIMARY KEY,             -- Identificador �nico
    Name VARCHAR(100) NOT NULL,        -- Nombre del usuario
    Email VARCHAR(255) NOT NULL UNIQUE, -- Correo �nico
    Password VARCHAR(255) NOT NULL,    -- Contrase�a (encriptada)
    Role VARCHAR(50) NOT NULL,         -- Rol (Admin, User)
    CreatedAt TIMESTAMP DEFAULT NOW(), -- Fecha de creaci�n
    UpdatedAt TIMESTAMP DEFAULT NOW()  -- �ltima actualizaci�n
);

--Crear la Tabla de Tareas
CREATE TABLE Tasks (
    Id SERIAL PRIMARY KEY,             -- Identificador �nico
    Title VARCHAR(200) NOT NULL,       -- T�tulo de la tarea
    Description TEXT,                  -- Descripci�n de la tarea
    Status VARCHAR(50) NOT NULL,       -- Estado (Pendiente, En Proceso y Completada)
    AssignedUserId INT REFERENCES Users(Id) ON DELETE SET NULL, -- Usuario asignado
    CreatedAt TIMESTAMP DEFAULT NOW(), -- Fecha de creaci�n
    UpdatedAt TIMESTAMP DEFAULT NOW()  -- �ltima actualizaci�n
);

