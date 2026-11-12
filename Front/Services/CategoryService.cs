using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Front.Models;

namespace Front.Services
{
    public class CategoryService
    {
        private readonly HttpClient _http;

        // CONSTRUCTEUR
        public CategoryService(HttpClient http)
        {
            _http = http;
        }

        // Récupère toutes les catégories
        public async Task<List<Category>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<Category>>("api/Categories");
        }
    }
}