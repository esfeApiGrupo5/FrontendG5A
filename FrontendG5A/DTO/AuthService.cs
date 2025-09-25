using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.IdentityModel.Tokens.Jwt;

namespace FrontendG5A.DTO
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

        // Guarda el token en el almacenamiento local y envio a los endpoints
        public async Task<string> login(UsuarioDTOSession usuarioDTOSession)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", usuarioDTOSession);
            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadFromJsonAsync<string>();
                return token;
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
    }
}
