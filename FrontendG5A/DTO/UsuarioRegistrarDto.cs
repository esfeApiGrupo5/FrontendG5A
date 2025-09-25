using System.ComponentModel.DataAnnotations;

namespace FrontendG5A.DTO
{
    public class UsuarioRegistrarDto
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "El correo debe ser válido")]
        [Required(ErrorMessage = "El correo de usuario es requerido")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        
        public int Estado { get; set; } = 1; // Por defecto activo
        public int IdRol { get; set; }
    }
}
