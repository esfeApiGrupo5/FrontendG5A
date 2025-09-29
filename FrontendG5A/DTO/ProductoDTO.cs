using System.Text.Json.Serialization;

namespace FrontendG5A.DTO
{
    public class ProductoDTO
    {
        public int Id { get; set; }

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; }

        [JsonPropertyName("precio")]
        public double Precio { get; set; }

        [JsonPropertyName("descripcion")]
        public string? Descripcion { get; set; }

        [JsonPropertyName("stock")]
        public int Stock { get; set; }

        [JsonPropertyName("categoria")]
        public string? Categoria { get; set; }

        [JsonPropertyName("urlImagen")]
        public string? UrlImagen { get; set; }

        // Propiedad calculada para mostrar el precio formateado
        public string PrecioFormateado => $"${Precio:N2}";

        // Propiedad calculada para verificar si hay stock disponible
        public bool TieneStock => Stock > 0;

        // Propiedad calculada para el estado del stock
        public string EstadoStock => Stock > 10 ? "Disponible" : Stock > 0 ? "Poco Stock" : "Agotado";

        // 🆕 Propiedad para validar si la URL de imagen es válida
        public bool TieneImagenValida => !string.IsNullOrWhiteSpace(UrlImagen) &&
                                         (UrlImagen.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                                          UrlImagen.StartsWith("https://", StringComparison.OrdinalIgnoreCase));

        // 🆕 Propiedad para obtener la URL de imagen o una imagen por defecto
        public string UrlImagenSegura => TieneImagenValida
            ? UrlImagen!
            : "https://via.placeholder.com/400x300?text=Sin+Imagen";
    }
}