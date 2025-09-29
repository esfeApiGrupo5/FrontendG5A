using FrontendG5A.DTO;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;

namespace FrontendG5A.Services
{
    public class AuthService
    {
        private readonly ProtectedSessionStorage _localStorage;
        private readonly HttpClient _httpClient;
        private string? _token;

        public AuthService(ProtectedSessionStorage localStorage, HttpClient httpClient)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
        }

        // 🆕 Método de Registro Actualizado: Ahora devuelve Task<bool>
        public async Task<bool> Register(UsuarioRegistrarDto usuarioRegistrarDto)
        {
            // El endpoint que especificaste es "/registrar"
            var response = await _httpClient.PostAsJsonAsync("api/auth/registrar", usuarioRegistrarDto);

            
            if (response.IsSuccessStatusCode)
            {
                // Registro exitoso.
                return true;
            }

            // Opcional: Manejar el cuerpo del error si la API lo proporciona
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error de registro: {errorContent}");

            return false; // Indica que el registro falló
        }

        // Guarda el token en el almacenamiento local y envio a los endpoints
        public async Task<string> login(UsuarioDTOSession usuarioDTOSession)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", usuarioDTOSession);
            if (response.IsSuccessStatusCode)
            {
                var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
                return authResponse.Token;
            }
            return null;
        }

        // Guarda el token en el almacenamiento local y envio a los endpoints
        public async Task SetToken(string token)
        {
            _token = token;
            await _localStorage.SetAsync("authToken", token);
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
        // Obtiene el token del almacenamiento local
        public async Task<string?> GetToken()
        {
            if (string.IsNullOrEmpty(_token))
            {
                var localStorageResult = await _localStorage.GetAsync<string>("authToken");
                if (!localStorageResult.Success || string.IsNullOrEmpty(localStorageResult.Value))
                {
                    _token = null;
                    return null;
                }
                _token = localStorageResult.Value;
            }
            return _token;
        }

        //Verifica si el usuario esta autenticado
        public async Task<bool> IsAuthenticated()
        {
            var token = await GetToken();

            return !string.IsNullOrEmpty(token) && !IsTokenExpired(token);
        }
        //Verifica si el token ha expirado
        public bool IsTokenExpired(string token)
        {
            var jwtToken = new JwtSecurityToken(token);
            return jwtToken.ValidTo < DateTime.UtcNow;  
        }

        //cerrar sesion
        public async Task Logout()
        {
            _token = null;
            await _localStorage.DeleteAsync("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        //tiene rol admin
        public async Task<bool> IsAdmin()
        {
            var token = await GetToken();
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                // 1. AJUSTAR: El nombre del claim de rol en tu JWT es "roles" (en minúsculas)
                const string RoleClaimType = "roles";

                // 2. AJUSTAR: El nombre del rol de administrador en tu JWT es "ROLE_ADMINISTRADOR"
                const string AdminRoleName = "ROLE_ADMINISTRADOR";

                // 3. BUSCAR: Buscamos cualquier claim con el tipo "roles"
                // y verificamos si alguno de ellos tiene el valor de Admin.
                var isAdmin = jwtToken.Claims
                    .Any(c => c.Type.Equals(RoleClaimType, StringComparison.OrdinalIgnoreCase) &&
                              c.Value.Equals(AdminRoleName, StringComparison.OrdinalIgnoreCase));

                return isAdmin;
            }
            catch (Exception ex)
            {
                // Esto captura errores si el token está mal formado o no es un JWT válido
                Console.WriteLine($"Error al verificar el rol del token: {ex.Message}");
                return false;
            }
        }
    }
}
