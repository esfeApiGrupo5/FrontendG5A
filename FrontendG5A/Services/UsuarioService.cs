using FrontendG5A.DTO;
using System.Text.Json;
using System.Net.Http.Json;

namespace FrontendG5A.Services
{
    public class UsuarioService
    {
        private readonly HttpClient _httpClient;
        private const string UsuarioBaseEndpoint = "/api/usuarios";

        public UsuarioService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Obtener todos los usuarios
        public async Task<List<UsuarioDTO>?> ObtenerUsuariosAsync()
        {
            try
            {
                Console.WriteLine($"[API Request] Llamando a: {UsuarioBaseEndpoint}/all");
                var response = await _httpClient.GetAsync($"{UsuarioBaseEndpoint}/all");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[API Error] Status: {response.StatusCode}, Content: {errorContent}");
                    return new List<UsuarioDTO>(); // Devolver lista vacía en lugar de null
                }

                var jsonContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[API Response] JSON: {jsonContent}");
                Console.WriteLine($"[API Success] Respuesta exitosa del servidor");

                var usuarios = await response.Content.ReadFromJsonAsync<List<UsuarioDTO>>();
                Console.WriteLine($"[API Result] Se deserializaron {usuarios?.Count ?? 0} usuarios");
                return usuarios ?? new List<UsuarioDTO>();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"[API Error] No se pudo conectar: {e.Message}");
                return new List<UsuarioDTO>();
            }
            catch (JsonException e)
            {
                Console.WriteLine($"[JSON Error] Error de deserialización: {e.Message}");
                return new List<UsuarioDTO>();
            }
            catch (Exception e)
            {
                Console.WriteLine($"[General Error] Error inesperado: {e.Message}");
                return new List<UsuarioDTO>();
            }
        }

        // Obtener un usuario específico por ID
        public async Task<UsuarioDTO?> ObtenerUsuarioPorIdAsync(int id)
        {
            try
            {
                var usuario = await _httpClient.GetFromJsonAsync<UsuarioDTO>($"{UsuarioBaseEndpoint}/{id}");
                return usuario;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"[API Error] No se pudo obtener el usuario con ID {id}: {e.Message}");
                return null;
            }
            catch (JsonException e)
            {
                Console.WriteLine($"[JSON Error] Error de deserialización para usuario ID {id}: {e.Message}");
                return null;
            }
        }

        // Buscar usuarios por nombre (búsqueda del lado del cliente)
        public async Task<List<UsuarioDTO>?> BuscarUsuariosPorNombreAsync(string nombre)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombre))
                {
                    return await ObtenerUsuariosAsync(); // Si no hay término, devolver todos
                }

                // Como no hay endpoint de búsqueda, obtenemos todos y filtramos
                var todosLosUsuarios = await ObtenerUsuariosAsync();

                if (todosLosUsuarios == null)
                {
                    return new List<UsuarioDTO>();
                }

                // Filtrar por nombre (búsqueda insensible a mayúsculas)
                var usuariosFiltrados = todosLosUsuarios
                    .Where(u => u.Nombre != null && u.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                Console.WriteLine($"[Client Filter] Se encontraron {usuariosFiltrados.Count} usuarios con el término '{nombre}'");
                return usuariosFiltrados;
            }
            catch (Exception e)
            {
                Console.WriteLine($"[General Error] Error inesperado en búsqueda: {e.Message}");
                return new List<UsuarioDTO>();
            }
        }

        // Crear un nuevo usuario
        public async Task<bool> CrearUsuarioAsync(UsuarioCrearDTO usuarioCrear)
        {
            try
            {
                Console.WriteLine($"[API Create] Creando usuario en: {UsuarioBaseEndpoint}");
                var response = await _httpClient.PostAsJsonAsync(UsuarioBaseEndpoint, usuarioCrear);
                Console.WriteLine($"[API Create] Respuesta: {response.StatusCode}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"[API Error] Error al crear usuario: {e.Message}");
                return false;
            }
        }

        // Modificar un usuario existente
        public async Task<bool> ModificarUsuarioAsync(UsuarioModificarDTO usuarioModificar)
        {
            try
            {
                Console.WriteLine($"[API Update] Modificando usuario ID {usuarioModificar.Id} en: {UsuarioBaseEndpoint}");
                var response = await _httpClient.PutAsJsonAsync(UsuarioBaseEndpoint, usuarioModificar);
                Console.WriteLine($"[API Update] Respuesta: {response.StatusCode}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"[API Error] Error al modificar usuario ID {usuarioModificar.Id}: {e.Message}");
                return false;
            }
        }

        // Eliminar un usuario
        public async Task<bool> EliminarUsuarioAsync(int id)
        {
            try
            {
                Console.WriteLine($"[API Delete] Eliminando usuario ID {id} en: {UsuarioBaseEndpoint}/{id}");
                var response = await _httpClient.DeleteAsync($"{UsuarioBaseEndpoint}/{id}");
                Console.WriteLine($"[API Delete] Respuesta: {response.StatusCode}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"[API Error] Error al eliminar usuario ID {id}: {e.Message}");
                return false;
            }
        }
    }
}