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
    }
}