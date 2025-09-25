using System.Text.Json.Serialization;

namespace FrontendG5A.DTO
{
    public class blogDTO
    {
        // Las propiedades 'int', 'long', y 'DateTime' no necesitan [JsonPropertyName] si tienen el mismo nombre.
        public int Id { get; set; }

        // Las propiedades de tipo string deben usar [JsonPropertyName] para el mapeo
        [JsonPropertyName("titulo")]
        public string? Titulo { get; set; }

        [JsonPropertyName("descripcion")]
        public string? Descripcion { get; set; }

        [JsonPropertyName("autor")]
        public string? Autor { get; set; }

        public long UsuarioId { get; set; }

        // DateTime mapea correctamente el formato ISO 8601 de la fecha
        public DateTime FechaPublicacion { get; set; }

        // Objeto anidado, lo hacemos nullable (?) debido a que puede ser 'null'
        [JsonPropertyName("usuarioInfo")]
        public UsuarioInfoDto? InfoUsuario { get; set; }
    }
}
