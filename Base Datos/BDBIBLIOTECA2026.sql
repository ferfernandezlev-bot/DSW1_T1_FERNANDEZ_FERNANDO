-- =========================================================
--  DSW1 - T1 - FERNANDEZ FERNANDO
--  Base de datos, tabla, datos de prueba y procedimientos
-- =========================================================
IF DB_ID('BDBIBLIOTECA2026') IS NULL
    CREATE DATABASE BDBIBLIOTECA2026;
GO
USE BDBIBLIOTECA2026;
GO
IF OBJECT_ID('dbo.Libros', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Libros
    (
     Id INT IDENTITY(1,1) PRIMARY KEY,
     NombreLibro VARCHAR(120) NOT NULL,
     Autor VARCHAR(100) NOT NULL,
     NombreEditorial VARCHAR(100) NOT NULL,
     ISDN VARCHAR(20) NOT NULL
    );
END
GO
IF NOT EXISTS (SELECT 1 FROM dbo.Libros)
    INSERT INTO dbo.Libros (NombreLibro, Autor, NombreEditorial, ISDN) VALUES
    ('Cien años de soledad',   'Gabriel García Márquez', 'Sudamericana', '978-0307474728'),
    ('La ciudad y los perros', 'Mario Vargas Llosa',     'Alfaguara',    '978-8420412146'),
    ('Los ríos profundos',     'José María Arguedas',    'Losada',       '978-9972402106');
GO

-- ================= PREGUNTA 1: CRUD =================
CREATE OR ALTER PROCEDURE dbo.usp_ListarLibros
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, NombreLibro, Autor, NombreEditorial, ISDN
    FROM dbo.Libros
    ORDER BY Id;
END
GO
CREATE OR ALTER PROCEDURE dbo.usp_RegistrarLibro
    @NombreLibro VARCHAR(120),
    @Autor VARCHAR(100),
    @NombreEditorial VARCHAR(100),
    @ISDN VARCHAR(20)
AS
BEGIN
    INSERT INTO dbo.Libros (NombreLibro, Autor, NombreEditorial, ISDN)
    VALUES (@NombreLibro, @Autor, @NombreEditorial, @ISDN);
END
GO
CREATE OR ALTER PROCEDURE dbo.usp_ActualizarLibro
    @Id INT,
    @NombreLibro VARCHAR(120),
    @Autor VARCHAR(100),
    @NombreEditorial VARCHAR(100),
    @ISDN VARCHAR(20)
AS
BEGIN
    UPDATE dbo.Libros
    SET NombreLibro = @NombreLibro,
        Autor = @Autor,
        NombreEditorial = @NombreEditorial,
        ISDN = @ISDN
    WHERE Id = @Id;
END
GO
CREATE OR ALTER PROCEDURE dbo.usp_EliminarLibro
    @Id INT
AS
BEGIN
    DELETE FROM dbo.Libros WHERE Id = @Id;
END
GO
CREATE OR ALTER PROCEDURE dbo.usp_BuscarLibro
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, NombreLibro, Autor, NombreEditorial, ISDN
    FROM dbo.Libros
    WHERE Id = @Id;
END
GO

-- ============ PREGUNTA 2: CONSULTA POR AUTOR ============
CREATE OR ALTER PROCEDURE dbo.usp_ListarLibrosPorAutor
    @Autor VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, NombreLibro, Autor, NombreEditorial, ISDN
    FROM dbo.Libros
    WHERE Autor LIKE '%' + @Autor + '%'
    ORDER BY Autor, NombreLibro;
END
GO