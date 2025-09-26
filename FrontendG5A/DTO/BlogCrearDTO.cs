using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FrontendG5A.DTO
{
    public class BlogCrearDTO
    {
        [Required(ErrorMessage = "El título no puede estar vacío")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "El título debe tener entre 5 y 100 caracteres")]
        [JsonPropertyName("titulo")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción no puede estar vacía")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "La descripción debe tener entre 10 y 500 caracteres")]
        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El autor no puede estar vacío")]
        [StringLength(50, ErrorMessage = "El nombre del autor no puede exceder los 50 caracteres")]
        [JsonPropertyName("autor")]
        public string Autor { get; set; } = string.Empty;
    }
}