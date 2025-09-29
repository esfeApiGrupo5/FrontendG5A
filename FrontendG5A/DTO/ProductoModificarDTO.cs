using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FrontendG5A.DTO
{
    public class ProductoModificarDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre no puede estar vacío")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        [JsonPropertyName("precio")]
        public double Precio { get; set; }

        [Required(ErrorMessage = "La descripción no puede estar vacía")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "La descripción debe tener entre 10 y 500 caracteres")]
        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El stock es requerido")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        [JsonPropertyName("stock")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "La categoría no puede estar vacía")]
        [StringLength(50, ErrorMessage = "La categoría no puede exceder los 50 caracteres")]
        [JsonPropertyName("categoria")]
        public string Categoria { get; set; } = string.Empty;

        [Required(ErrorMessage = "La URL de la imagen es requerida")]
        [Url(ErrorMessage = "Debe ser una URL válida")]
        [JsonPropertyName("urlImagen")]
        public string UrlImagen { get; set; } = string.Empty;
    }
}