# DSW1 - T1 - Fernández Fernando

Aplicación **ASP.NET Core MVC (.NET 8)** con **SQL Server**, usando **ADO.NET** y **procedimientos almacenados**.

## Contenido
- **Pregunta 1:** CRUD de la tabla `Libros` (listar, registrar, editar y eliminar con confirmación) — `LibroController`.
- **Pregunta 2:** Consulta de libros por autor con coincidencia parcial (`LIKE`) — `usp_ListarLibrosPorAutor` y acción `ConsultarPorAutor`.

## Cómo ejecutar
1. En SQL Server Management Studio, abrir y ejecutar `Base Datos/BDBIBLIOTECA2026.sql` (crea la base `BDBIBLIOTECA2026`, la tabla, datos de prueba y los procedimientos).
2. Revisar la cadena de conexión `cn` en `appsettings.json` (por defecto `Server=localhost`, autenticación de Windows).
3. En la carpeta del proyecto ejecutar: `dotnet run` 
4. Abrir en el navegador la dirección que muestra la terminal (por ejemplo `http://localhost:5246`).

## Tecnologías
.NET 8 · ASP.NET Core MVC · SQL Server · Microsoft.Data.SqlClient · Bootstrap 5
