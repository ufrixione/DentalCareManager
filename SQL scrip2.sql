USE [master]
GO

-- 1. Crear la base de datos limpia sin rutas rígidas
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'ConsultorioDental')
    DROP DATABASE [ConsultorioDental]
GO

CREATE DATABASE [ConsultorioDental]
GO

USE [ConsultorioDental]
GO

-- 2. Tabla Pacientes básica (Es requerida primero por las llaves foráneas)
CREATE TABLE Pacientes (
    IdPaciente INT IDENTITY(1,1) PRIMARY KEY,
    Cedula VARCHAR(20) NULL,
    Nombre VARCHAR(50) NOT NULL,
    Apellido VARCHAR(50) NOT NULL,
    Telefono VARCHAR(20) NULL,
    Direccion VARCHAR(200) NULL,
    FechaNacimiento DATE NULL,
    FechaRegistro DATETIME NULL,
    Activo BIT NULL
);
GO

-- 3. Tabla Dentista (Singular)
CREATE TABLE Dentista (
    IdDentista INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
    Apellido VARCHAR(50) NOT NULL,
    Activo BIT NULL
);
GO

-- 4. Tabla Dentistas (Plural)
CREATE TABLE Dentistas (
    IdDentistas INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
    Apellido VARCHAR(50) NOT NULL,
    Especialidad VARCHAR(100) NULL,
    Activo BIT NULL
);
GO

-- 5. Tabla PacientesApp
CREATE TABLE PacientesApp (
    IdPacienteApp INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NULL,
    Telefono VARCHAR(20) NULL
);
GO

-- 6. Tabla Citas (Apunta a la tabla 'Pacientes')
CREATE TABLE Citas (
    IdCita INT IDENTITY(1,1) PRIMARY KEY,
    Fecha DATE NOT NULL,
    Hora TIME(7) NOT NULL,
    Estado VARCHAR(20) NULL,
    IdPaciente INT NOT NULL FOREIGN KEY REFERENCES Pacientes(IdPaciente),
    IdDentista INT NOT NULL
);
GO

-- 7. Tabla CitasApp
CREATE TABLE CitasApp (
    IdCitaApp INT IDENTITY(1,1) PRIMARY KEY,
    FechaHora DATETIME NULL,
    Detalle VARCHAR(200) NULL
);
GO

-- 8. Tabla HistorialClinico
CREATE TABLE HistorialClinico (
    IdHistorial INT IDENTITY(1,1) PRIMARY KEY,
    IdPaciente INT NOT NULL FOREIGN KEY REFERENCES Pacientes(IdPaciente),
    FechaConsulta DATETIME NOT NULL,
    Diagnostico VARCHAR(MAX) NULL,
    Tratamiento VARCHAR(MAX) NULL
);
GO

-- 9. Tabla Pagos (Se crea al final porque depende de que exista 'Citas' primero)
CREATE TABLE Pagos (
    IdPago INT IDENTITY(1,1) PRIMARY KEY,
    IdCita INT NOT NULL FOREIGN KEY REFERENCES Citas(IdCita),
    Monto DECIMAL(18,2) NOT NULL,
    FechaPago DATETIME NOT NULL,
    Estado VARCHAR(20) NULL
);
GO