using Front.Models;
using System.Net.Http.Json;

namespace Front.Services
{
    public class CategoryAdminService
    {
        private readonly HttpClient _http;
        private readonly AuthService _authService;

        public CategoryAdminService(HttpClient http, AuthService authService)
        {
            _http = http;
            _authService = authService;
        }

        // Récupérer toutes les catégories (pour admin)
        public async Task<List<Category>> GetAllAsync()
        {
            try
            {
                await _authService.InitializeAuthHeader();
                return await _http.GetFromJsonAsync<List<Category>>("api/Categories") ?? new List<Category>();
            }
            catch
            {
                return new List<Category>();
            }
        }

        // Récupérer une catégorie par ID
        public async Task<Category?> GetByIdAsync(int id)
        {
            try
            {
                await _authService.InitializeAuthHeader();
                return await _http.GetFromJsonAsync<Category>($"api/Categories/{id}");
            }
            catch
            {
                return null;
            }
        }

        // Créer une nouvelle catégorie
        public async Task<Category?> CreateAsync(Category category)
        {
            try
            {
                await _authService.InitializeAuthHeader();
                var response = await _http.PostAsJsonAsync("api/Categories", category);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Category>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        // Mettre à jour une catégorie
        public async Task<Category?> UpdateAsync(Category category)
        {
            try
            {
                await _authService.InitializeAuthHeader();
                var response = await _http.PutAsJsonAsync("api/Categories", category);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Category>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        // Supprimer une catégorie
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await _authService.InitializeAuthHeader();
                var response = await _http.DeleteAsync($"api/Categories/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
