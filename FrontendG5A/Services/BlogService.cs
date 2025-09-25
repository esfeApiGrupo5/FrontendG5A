using FrontendG5A.DTO;
using System.Text.Json;

namespace FrontendG5A.Services
{
    public class BlogService
    {
        private readonly HttpClient _httpClient;
        private const string BlogListEndpoint = "/api/blogs/lista";

        // HttpClient se inyecta y ya tiene la BaseAddress configurada desde Program.cs.
        public BlogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<BlogDTO>?> ObtenerBlogsAsync()
        {
            try
            {
                // Petición y deserialización automática.
                var blogs = await _httpClient.GetFromJsonAsync<List<BlogDTO>>(BlogListEndpoint);

                return blogs;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"[API Error] No se pudo conectar o hubo un error HTTP: {e.Message}");
                return null;
            }
            catch (JsonException e)
            {
                Console.WriteLine($"[JSON Error] Error de deserialización: {e.Message}");
                return null;
            }
        }
    }
}
