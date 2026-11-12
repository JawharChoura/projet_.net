using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Front.Models;

namespace Front.Services
{
    // Service pour gérer les opérations CRUD sur les articles
    // Ce service communique avec l'API pour récupérer, créer, modifier et supprimer des articles
    public class ArticleService
    {
        private readonly HttpClient _http;

        // 1. CONSTRUCTEUR : initialise le HttpClient pour les appels API
        public ArticleService(HttpClient http)
        {
            _http = http;
        }

        // 2. CRÉATION : Envoie POST à l'API pour créer un article (réservé aux admins)
        public async Task<Article> AddAsync(Article article)
        {
            var response = await _http.PostAsJsonAsync("api/articles", article);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Article>();
        }

        // 3. LECTURE (TOUS) : Récupère tous les articles disponibles dans le menu
        public async Task<List<Article>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<Article>>("api/articles");
        }

        // 4. LECTURE (UN SEUL) : Récupère un article par ID pour l'édition
        public async Task<Article> GetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<Article>($"api/articles/{id}");
        }

        // 5. MISE À JOUR : Modifie un article existant (réservé aux admins)
        public async Task<Article> UpdateAsync(int id, Article article)
        {
            var response = await _http.PutAsJsonAsync($"api/articles/{id}", article);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Article>();
        }

        // 6. SUPPRESSION : Supprime un article (réservé aux admins)
        public async Task DeleteAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/articles/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}