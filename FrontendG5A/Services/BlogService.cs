using FrontendG5A.DTO;
using System.Text.Json;
using System.Net.Http.Json;

namespace FrontendG5A.Services
{
    public class BlogService
    {
        private readonly HttpClient _httpClient;
        private const string BlogBaseEndpoint = "/api/blogs";
        private const string BlogListEndpoint = "/api/blogs/lista"; // Endpoint específico para listar

        public BlogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Obtener todos los blogs - usando el endpoint original que funcionaba
        public async Task<List<BlogDTO>?> ObtenerBlogsAsync()
        {
            try
            {
                Console.WriteLine($"[API Request] Llamando a: {BlogListEndpoint}");
                var response = await _httpClient.GetAsync(BlogListEndpoint);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[API Error] Status: {response.StatusCode}, Content: {errorContent}");
                    return new List<BlogDTO>(); // Devolver lista vacía en lugar de null
                }

                var jsonContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[API Response] JSON: {jsonContent}");
                Console.WriteLine($"[API Success] Respuesta exitosa del servidor");

                var blogs = await response.Content.ReadFromJsonAsync<List<BlogDTO>>();
                Console.WriteLine($"[API Result] Se deserializaron {blogs?.Count ?? 0} blogs");
                return blogs ?? new List<BlogDTO>();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"[API Error] No se pudo conectar: {e.Message}");
                return new List<BlogDTO>();
            }
            catch (JsonException e)
            {
                Console.WriteLine($"[JSON Error] Error de deserialización: {e.Message}");
                return new List<BlogDTO>();
            }
            catch (Exception e)
            {
                Console.WriteLine($"[General Error] Error inesperado: {e.Message}");
                return new List<BlogDTO>();
            }
        }

        // Obtener un blog específico por ID
        public async Task<BlogDTO?> ObtenerBlogPorIdAsync(int id)
        {
            try
            {
                var blog = await _httpClient.GetFromJsonAsync<BlogDTO>($"{BlogBaseEndpoint}/{id}");
                return blog;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"[API Error] No se pudo obtener el blog con ID {id}: {e.Message}");
                return null;
            }
            catch (JsonException e)
            {
                Console.WriteLine($"[JSON Error] Error de deserialización para blog ID {id}: {e.Message}");
                return null;
            }
        }

        // Buscar blogs por título (búsqueda del lado del servidor)
        public async Task<List<BlogDTO>?> BuscarBlogsPorTituloAsync(string titulo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(titulo))
                {
                    return await ObtenerBlogsAsync(); // Si no hay término, devolver todos
                }

                var endpoint = $"{BlogBaseEndpoint}/buscar?titulo={Uri.EscapeDataString(titulo)}";
                Console.WriteLine($"[API Search] Buscando en: {endpoint}");
                var response = await _httpClient.GetAsync(endpoint);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[API Error] Status: {response.StatusCode}, Content: {errorContent}");
                    return new List<BlogDTO>();
                }

                var jsonContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[API Search Response] JSON: {jsonContent}");

                var blogs = await response.Content.ReadFromJsonAsync<List<BlogDTO>>();
                return blogs ?? new List<BlogDTO>();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"[API Error] Error al buscar blogs: {e.Message}");
                return new List<BlogDTO>();
            }
            catch (JsonException e)
            {
                Console.WriteLine($"[JSON Error] Error de deserialización en búsqueda: {e.Message}");
                return new List<BlogDTO>();
            }
            catch (Exception e)
            {
                Console.WriteLine($"[General Error] Error inesperado en búsqueda: {e.Message}");
                return new List<BlogDTO>();
            }
        }

        // Crear un nuevo blog
        public async Task<bool> CrearBlogAsync(BlogCrearDTO blogCrear)
        {
            try
            {
                Console.WriteLine($"[API Create] Creando blog en: {BlogBaseEndpoint}");
                var response = await _httpClient.PostAsJsonAsync(BlogBaseEndpoint, blogCrear);
                Console.WriteLine($"[API Create] Respuesta: {response.StatusCode}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"[API Error] Error al crear blog: {e.Message}");
                return false;
            }
        }

        // Modificar un blog existente
        public async Task<bool> ModificarBlogAsync(int id, BlogModificarDTO blogModificar)
        {
            try
            {
                Console.WriteLine($"[API Update] Modificando blog ID {id} en: {BlogBaseEndpoint}/{id}");
                var response = await _httpClient.PutAsJsonAsync($"{BlogBaseEndpoint}/{id}", blogModificar);
                Console.WriteLine($"[API Update] Respuesta: {response.StatusCode}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"[API Error] Error al modificar blog ID {id}: {e.Message}");
                return false;
            }
        }

        // Eliminar un blog
        public async Task<bool> EliminarBlogAsync(int id)
        {
            try
            {
                Console.WriteLine($"[API Delete] Eliminando blog ID {id} en: {BlogBaseEndpoint}/{id}");
                var response = await _httpClient.DeleteAsync($"{BlogBaseEndpoint}/{id}");
                Console.WriteLine($"[API Delete] Respuesta: {response.StatusCode}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"[API Error] Error al eliminar blog ID {id}: {e.Message}");
                return false;
            }
        }
    }
}