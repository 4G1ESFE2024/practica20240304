-- Crear la base de datos Practica20240304
CREATE DATABASE Practica20240304;
GO

-- Usar la base de datos creada
USE Practica20240304;
GO

-- Crear la tabla Factura
CREATE TABLE Factura (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Correlativo VARCHAR(50),
    Fecha DATE
);
GO

-- Crear la tabla DetFactura
CREATE TABLE DetFactura (
    Id INT PRIMARY KEY IDENTITY(1,1),
    IdFactura INT FOREIGN KEY REFERENCES Factura(Id),
    Producto VARCHAR(100),
    Cantidad INT,
    Precio DECIMAL(18,2)
);
GO
