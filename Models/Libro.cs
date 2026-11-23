using System.ComponentModel.DataAnnotations;

namespace DSW1_T1_FERNANDEZ_FERNANDO.Models
{
    public class Libro
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del libro es obligatorio.")]
        [StringLength(120, ErrorMessage = "Máximo 120 caracteres.")]
        [Display(Name = "Nombre del libro")]
        public string NombreLibro { get; set; } = string.Empty;

        [Required(ErrorMessage = "El autor es obligatorio.")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
        [Display(Name = "Autor")]
        public string Autor { get; set; } = string.Empty;

        [Required(ErrorMessage = "La editorial es obligatoria.")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
        [Display(Name = "Editorial")]
        public string NombreEditorial { get; set; } = string.Empty;

        [Required(ErrorMessage = "El ISDN es obligatorio.")]
        [StringLength(20, ErrorMessage = "Máximo 20 caracteres.")]
        [Display(Name = "ISDN")]
        public string ISDN { get; set; } = string.Empty;
    }
}