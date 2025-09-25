using System.Text.Json.Serialization;
namespace FrontendG5A.DTO
{
    public class UsuarioInfoDto
    {
        public int Id { get; set; }

        [JsonPropertyName("nombre")]
        public string? NombreUsuario { get; set; }

        [JsonPropertyName("correo")]
        public string? CorreoElectronico { get; set; }

        [JsonPropertyName("rolNombre")]
        public string? Rol { get; set; }
    }
}
