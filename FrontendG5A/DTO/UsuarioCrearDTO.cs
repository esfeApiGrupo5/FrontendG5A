using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FrontendG5A.DTO
{
    public class UsuarioCrearDTO
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 caracteres")]
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "El correo debe ser válido")]
        [Required(ErrorMessage = "El correo de usuario es requerido")]
        [JsonPropertyName("correo")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres")]
        [DataType(DataType.Password)]
        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;

        [JsonPropertyName("estado")]
        public int Estado { get; set; } = 1; // Por defecto activo

        [Required(ErrorMessage = "El rol es requerido")]
        [JsonPropertyName("idRol")]
        public int IdRol { get; set; }
    }
}