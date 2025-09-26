using System.Text.Json.Serialization;

namespace FrontendG5A.DTO
{
    public class UsuarioDTO
    {
        public int Id { get; set; }

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; }

        [JsonPropertyName("correo")]
        public string? Correo { get; set; }

        [JsonPropertyName("estado")]
        public int Estado { get; set; }

        [JsonPropertyName("rol")]
        public RolSalidaDto? Rol { get; set; }

        // Propiedad calculada para mostrar el estado como texto
        public string EstadoTexto => Estado == 1 ? "Activo" : "Inactivo";
    }

    public class RolSalidaDto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
    }
}