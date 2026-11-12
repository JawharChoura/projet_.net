using Front.Models;
using System.Net.Http.Json;

namespace Front.Services
{
    public class ArticleAdminService
    {
        private readonly HttpClient _http;
        private readonly AuthService _authService;

        public ArticleAdminService(HttpClient http, AuthService authService)
        {
            _http = http;
            _authService = authService;
        }

        // Récupérer tous les articles (pour admin)
        public async Task<List<Article>> GetAllAsync()
        {
            try
            {
                await _authService.InitializeAuthHeader();
                return await _http.GetFromJsonAsync<List<Article>>("api/Articles") ?? new List<Article>();
            }
            catch
            {
                return new List<Article>();
            }
        }

        // Récupérer un article par ID
        public async Task<Article?> GetByIdAsync(int id)
        {
            try
            {
                await _authService.InitializeAuthHeader();
                return await _http.GetFromJsonAsync<Article>($"api/Articles/{id}");
            }
            catch
            {
                return null;
            }
        }

        // Créer un nouvel article
        public async Task<Article?> CreateAsync(Article article)
        {
            try
            {
                await _authService.InitializeAuthHeader();
                var response = await _http.PostAsJsonAsync("api/Articles", article);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Article>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        // Mettre à jour un article
        public async Task<Article?> UpdateAsync(int id, Article article)
        {
            try
            {
                await _authService.InitializeAuthHeader();
                var response = await _http.PutAsJsonAsync($"api/Articles/{id}", article);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Article>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        // Supprimer un article
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await _authService.InitializeAuthHeader();
                var response = await _http.DeleteAsync($"api/Articles/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
