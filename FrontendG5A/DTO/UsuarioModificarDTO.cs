using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FrontendG5A.DTO
{
    public class UsuarioModificarDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 caracteres")]
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "El correo debe ser válido")]
        [Required(ErrorMessage = "El correo de usuario es requerido")]
        [JsonPropertyName("correo")]
        public string Correo { get; set; } = string.Empty;

        [OptionalStringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres")]
        [DataType(DataType.Password)]
        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;

        [JsonPropertyName("estado")]
        public int Estado { get; set; }

        [Required(ErrorMessage = "El rol es requerido")]
        [JsonPropertyName("idRol")]
        public int IdRol { get; set; }
    }
    // Validador personalizado que solo valida si el campo tiene contenido
    public class OptionalStringLengthAttribute : ValidationAttribute
    {
        private readonly int _maximumLength;
        private readonly int _minimumLength;

        public OptionalStringLengthAttribute(int maximumLength)
        {
            _maximumLength = maximumLength;
            _minimumLength = 0;
        }

        public int MinimumLength
        {
            get => _minimumLength;
            init => _minimumLength = value;
        }

        public int MaximumLength => _maximumLength;

        public override bool IsValid(object? value)
        {
            // Si el valor es null o cadena vacía, es válido (campo opcional)
            if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
            {
                return true;
            }

            // Si tiene contenido, aplicar validación de longitud
            if (value is string stringValue)
            {
                var length = stringValue.Length;
                return length >= _minimumLength && length <= _maximumLength;
            }

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return ErrorMessage ?? $"El campo {name} debe tener entre {_minimumLength} y {_maximumLength} caracteres.";
        }
    }
}
