USE master;
GO

IF DB_ID('TalentMatchDb') IS NULL
BEGIN
    CREATE DATABASE TalentMatchDb;
END
GO

USE TalentMatchDb;
GO

IF OBJECT_ID('dbo.Productos', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Productos (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(150) NOT NULL,
        Precio DECIMAL(18,2) NOT NULL CHECK (Precio > 0),
        Stock INT NOT NULL CHECK (Stock >= 0)
    );
END
GO

INSERT INTO dbo.Productos (Nombre, Precio, Stock)
VALUES
    (N'Laptop Gamer', 2499.99, 10),
    (N'Monitor 27', 899.50, 18),
    (N'Teclado Mecánico', 149.00, 30)
GO
