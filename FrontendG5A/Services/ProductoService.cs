using FrontendG5A.DTO;
using System.Text.Json;
using System.Net.Http.Json;

namespace FrontendG5A.Services
{
    public class ProductoService
    {
        private readonly HttpClient _httpClient;
        private const string ProductoBaseEndpoint = "/api/productos";

        public ProductoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Obtener todos los productos
        public async Task<List<ProductoDTO>?> ObtenerProductosAsync()
        {
            try
            {
                Console.WriteLine($"[API Request] Llamando a: {ProductoBaseEndpoint}/lista");
                var response = await _httpClient.GetAsync($"{ProductoBaseEndpoint}/lista");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[API Error] Status: {response.StatusCode}, Content: {errorContent}");
                    return new List<ProductoDTO>();
                }

                var jsonContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[API Response] JSON: {jsonContent}");
                Console.WriteLine($"[API Success] Respuesta exitosa del servidor");

                var productos = await response.Content.ReadFromJsonAsync<List<ProductoDTO>>();
                Console.WriteLine($"[API Result] Se deserializaron {productos?.Count ?? 0} productos");
                return productos ?? new List<ProductoDTO>();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"[API Error] No se pudo conectar: {e.Message}");
                return new List<ProductoDTO>();
            }
            catch (JsonException e)
            {
                Console.WriteLine($"[JSON Error] Error de deserialización: {e.Message}");
                return new List<ProductoDTO>();
            }
            catch (Exception e)
            {
                Console.WriteLine($"[General Error] Error inesperado: {e.Message}");
                return new List<ProductoDTO>();
            }
        }

        // Obtener un producto específico por ID
        public async Task<ProductoDTO?> ObtenerProductoPorIdAsync(int id)
        {
            try
            {
                var producto = await _httpClient.GetFromJsonAsync<ProductoDTO>($"{ProductoBaseEndpoint}/{id}");
                return producto;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"[API Error] No se pudo obtener el producto con ID {id}: {e.Message}");
                return null;
            }
            catch (JsonException e)
            {
                Console.WriteLine($"[JSON Error] Error de deserialización para producto ID {id}: {e.Message}");
                return null;
            }
        }

        // Buscar productos por nombre
        public async Task<List<ProductoDTO>?> BuscarProductosPorNombreAsync(string nombre)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombre))
                {
                    return await ObtenerProductosAsync();
                }

                var endpoint = $"{ProductoBaseEndpoint}/buscar?nombre={Uri.EscapeDataString(nombre)}";
                Console.WriteLine($"[API Search] Buscando en: {endpoint}");
                var response = await _httpClient.GetAsync(endpoint);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[API Error] Status: {response.StatusCode}, Content: {errorContent}");
                    return new List<ProductoDTO>();
                }

                var jsonContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[API Search Response] JSON: {jsonContent}");

                var productos = await response.Content.ReadFromJsonAsync<List<ProductoDTO>>();
                return productos ?? new List<ProductoDTO>();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"[API Error] Error al buscar productos: {e.Message}");
                return new List<ProductoDTO>();
            }
            catch (JsonException e)
            {
                Console.WriteLine($"[JSON Error] Error de deserialización en búsqueda: {e.Message}");
                return new List<ProductoDTO>();
            }
            catch (Exception e)
            {
                Console.WriteLine($"[General Error] Error inesperado en búsqueda: {e.Message}");
                return new List<ProductoDTO>();
            }
        }

        // Crear un nuevo producto
        public async Task<bool> CrearProductoAsync(ProductoCrearDTO productoCrear)
        {
            try
            {
                Console.WriteLine($"[API Create] Creando producto en: {ProductoBaseEndpoint}");
                var response = await _httpClient.PostAsJsonAsync(ProductoBaseEndpoint, productoCrear);
                Console.WriteLine($"[API Create] Respuesta: {response.StatusCode}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"[API Error] Error al crear producto: {e.Message}");
                return false;
            }
        }

        // Modificar un producto existente
        public async Task<bool> ModificarProductoAsync(int id, ProductoModificarDTO productoModificar)
        {
            try
            {
                Console.WriteLine($"[API Update] Modificando producto ID {id} en: {ProductoBaseEndpoint}/{id}");
                var response = await _httpClient.PutAsJsonAsync($"{ProductoBaseEndpoint}/{id}", productoModificar);
                Console.WriteLine($"[API Update] Respuesta: {response.StatusCode}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"[API Error] Error al modificar producto ID {id}: {e.Message}");
                return false;
            }
        }

        // Eliminar un producto
        public async Task<bool> EliminarProductoAsync(int id)
        {
            try
            {
                Console.WriteLine($"[API Delete] Eliminando producto ID {id} en: {ProductoBaseEndpoint}/{id}");
                var response = await _httpClient.DeleteAsync($"{ProductoBaseEndpoint}/{id}");
                Console.WriteLine($"[API Delete] Respuesta: {response.StatusCode}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"[API Error] Error al eliminar producto ID {id}: {e.Message}");
                return false;
            }
        }
    }
}