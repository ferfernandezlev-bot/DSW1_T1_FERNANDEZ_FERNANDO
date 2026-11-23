using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using DSW1_T1_FERNANDEZ_FERNANDO.Models;

namespace DSW1_T1_FERNANDEZ_FERNANDO.Controllers
{
    public class LibroController : Controller
    {
        private readonly string _cadena;

        public LibroController(IConfiguration configuration)
        {
            _cadena = configuration.GetConnectionString("cn")
                ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'cn'.");
        }

        // =====================================================
        //  ACCESO A DATOS: ADO.NET + PROCEDIMIENTOS ALMACENADOS
        // =====================================================

        // Método IEnumerable para listar los libros
        private IEnumerable<Libro> ListarLibros()
        {
            var lista = new List<Libro>();
            using var cn = new SqlConnection(_cadena);
            using var cmd = new SqlCommand("dbo.usp_ListarLibros", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cn.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new Libro
                {
                    Id = dr.GetInt32(0),
                    NombreLibro = dr.GetString(1),
                    Autor = dr.GetString(2),
                    NombreEditorial = dr.GetString(3),
                    ISDN = dr.GetString(4)
                });
            }
            return lista;
        }

        private Libro? BuscarLibro(int id)
        {
            using var cn = new SqlConnection(_cadena);
            using var cmd = new SqlCommand("dbo.usp_BuscarLibro", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            cn.Open();
            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                return new Libro
                {
                    Id = dr.GetInt32(0),
                    NombreLibro = dr.GetString(1),
                    Autor = dr.GetString(2),
                    NombreEditorial = dr.GetString(3),
                    ISDN = dr.GetString(4)
                };
            }
            return null;
        }

        private int RegistrarLibro(Libro libro)
        {
            using var cn = new SqlConnection(_cadena);
            using var cmd = new SqlCommand("dbo.usp_RegistrarLibro", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@NombreLibro", SqlDbType.VarChar, 120).Value = libro.NombreLibro;
            cmd.Parameters.Add("@Autor", SqlDbType.VarChar, 100).Value = libro.Autor;
            cmd.Parameters.Add("@NombreEditorial", SqlDbType.VarChar, 100).Value = libro.NombreEditorial;
            cmd.Parameters.Add("@ISDN", SqlDbType.VarChar, 20).Value = libro.ISDN;
            cn.Open();
            return cmd.ExecuteNonQuery(); // filas afectadas
        }

        private int ActualizarLibro(Libro libro)
        {
            using var cn = new SqlConnection(_cadena);
            using var cmd = new SqlCommand("dbo.usp_ActualizarLibro", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = libro.Id;
            cmd.Parameters.Add("@NombreLibro", SqlDbType.VarChar, 120).Value = libro.NombreLibro;
            cmd.Parameters.Add("@Autor", SqlDbType.VarChar, 100).Value = libro.Autor;
            cmd.Parameters.Add("@NombreEditorial", SqlDbType.VarChar, 100).Value = libro.NombreEditorial;
            cmd.Parameters.Add("@ISDN", SqlDbType.VarChar, 20).Value = libro.ISDN;
            cn.Open();
            return cmd.ExecuteNonQuery();
        }

        private int EliminarLibro(int id)
        {
            using var cn = new SqlConnection(_cadena);
            using var cmd = new SqlCommand("dbo.usp_EliminarLibro", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            cn.Open();
            return cmd.ExecuteNonQuery();
        }

        // =====================================================
        //  ACCIONES
        // =====================================================

        // GET: /Libro  -> listado
        public IActionResult Index()
        {
            // El mensaje viaja en TempData durante la redirección y se muestra con ViewBag
            ViewBag.Mensaje = TempData["Mensaje"];
            ViewBag.Error = TempData["Error"];
            try
            {
                return View(ListarLibros());
            }
            catch (SqlException ex)
            {
                ViewBag.Error = "No se pudo obtener el listado de libros: " + ex.Message;
                return View(new List<Libro>());
            }
        }

        // GET: /Libro/Crear
        [HttpGet]
        public IActionResult Crear()
        {
            return View(new Libro());
        }

        // POST: /Libro/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Libro libro)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Complete correctamente todos los campos obligatorios.";
                return View(libro);
            }
            try
            {
                if (RegistrarLibro(libro) > 0)
                {
                    TempData["Mensaje"] = $"El libro \"{libro.NombreLibro}\" se registró correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.Error = "No se pudo registrar el libro.";
            }
            catch (SqlException ex)
            {
                ViewBag.Error = "Error al registrar el libro: " + ex.Message;
            }
            return View(libro);
        }

        // GET: /Libro/Editar/5
        [HttpGet]
        public IActionResult Editar(int id)
        {
            try
            {
                var libro = BuscarLibro(id);
                if (libro == null)
                {
                    TempData["Error"] = $"No existe un libro con Id {id}.";
                    return RedirectToAction(nameof(Index));
                }
                return View(libro);
            }
            catch (SqlException ex)
            {
                TempData["Error"] = "No se pudo cargar el libro: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: /Libro/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Libro libro)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Complete correctamente todos los campos obligatorios.";
                return View(libro);
            }
            try
            {
                if (ActualizarLibro(libro) > 0)
                {
                    TempData["Mensaje"] = $"El libro con Id {libro.Id} se actualizó correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.Error = $"No se encontró el libro con Id {libro.Id}; no se actualizó.";
            }
            catch (SqlException ex)
            {
                ViewBag.Error = "Error al actualizar el libro: " + ex.Message;
            }
            return View(libro);
        }

        // POST: /Libro/Eliminar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(int id)
        {
            try
            {
                if (EliminarLibro(id) > 0)
                    TempData["Mensaje"] = $"El libro con Id {id} se eliminó correctamente.";
                else
                    TempData["Error"] = $"No se encontró el libro con Id {id}; no se eliminó.";
            }
            catch (SqlException ex)
            {
                TempData["Error"] = "No se pudo eliminar el libro: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
        
        // =====================================================
        //  PREGUNTA 2: CONSULTA DE LIBROS POR AUTOR
        // =====================================================

        // Método IEnumerable que ejecuta usp_ListarLibrosPorAutor
        private IEnumerable<Libro> ListarLibrosPorAutor(string autor)
        {
            var lista = new List<Libro>();
            using var cn = new SqlConnection(_cadena);
            using var cmd = new SqlCommand("dbo.usp_ListarLibrosPorAutor", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Autor", SqlDbType.VarChar, 100).Value = autor;
            cn.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new Libro
                {
                    Id = dr.GetInt32(0),
                    NombreLibro = dr.GetString(1),
                    Autor = dr.GetString(2),
                    NombreEditorial = dr.GetString(3),
                    ISDN = dr.GetString(4)
                });
            }
            return lista;
        }

        // GET: /Libro/ConsultarPorAutor?autor=Vargas
        [HttpGet]
        public IActionResult ConsultarPorAutor(string? autor)
        {
            ViewBag.Autor = autor;

            if (string.IsNullOrWhiteSpace(autor))
            {
                ViewBag.Error = "Ingrese el nombre (o parte del nombre) del autor para buscar.";
                return View(new List<Libro>());
            }

            try
            {
                var lista = ListarLibrosPorAutor(autor.Trim());
                if (lista.Any())
                    ViewBag.Mensaje = $"Se encontraron {lista.Count()} libro(s) para el autor \"{autor.Trim()}\".";
                else
                    ViewBag.Error = $"No se encontraron libros para el autor \"{autor.Trim()}\".";
                return View(lista);
            }
            catch (SqlException ex)
            {
                ViewBag.Error = "No se pudo realizar la consulta: " + ex.Message;
                return View(new List<Libro>());
            }
        }
    }
}