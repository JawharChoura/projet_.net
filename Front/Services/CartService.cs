using Front.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Front.Services
{
    public class CartService
    {
        private readonly HttpClient _http;
        private readonly AuthService _authService;

        public CartService(HttpClient http, AuthService authService)
        {
            _http = http;
            _authService = authService;
        }

        // Récupérer le panier de l'utilisateur
        public async Task<Cart?> GetCartAsync(string userId)
        {
            try
            {
                // Vérifier l'authentification
                await _authService.InitializeAuthHeader();
                // Correction: Utiliser l'URL avec userId comme dans l'API
                return await _http.GetFromJsonAsync<Cart>($"api/Cart/{userId}");
            }
            catch
            {
                return null;
            }
        }

        // Ajouter un article au panier
        public async Task<bool> AddItemAsync(string userId, CartItem item)
        {
            try
            {
                await _authService.InitializeAuthHeader();
                // Correction: Utiliser l'URL avec userId et passer CartItem
                var response = await _http.PostAsJsonAsync($"api/Cart/{userId}", item);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // Mettre à jour la quantité d'un item du panier
        public async Task<bool> UpdateCartItemAsync(int cartItemId, int quantity)
        {
            try
            {
                await _authService.InitializeAuthHeader();
                // Correction: Supprimer "items/" pour matcher l'API
                var response = await _http.PutAsJsonAsync($"api/Cart/{cartItemId}", new { quantity });
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // Supprimer un item du panier
        public async Task<bool> RemoveItemFromCartAsync(int cartItemId)
        {
            try
            {
                await _authService.InitializeAuthHeader();
                // Correction: Supprimer "items/" pour matcher l'API
                var response = await _http.DeleteAsync($"api/Cart/{cartItemId}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // Vider le panier
        public async Task<bool> ClearCartAsync(string userId)
        {
            try
            {
                await _authService.InitializeAuthHeader();
                // Correction: Utiliser l'URL avec userId
                var response = await _http.DeleteAsync($"api/Cart/clear/{userId}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // Classe DTO pour ajouter un item (maintenant obsolète, on utilise CartItem directement)
        // public class AddCartItemDto
        // {
        //     public int ArticleId { get; set; }
        //     public int Quantity { get; set; } = 1;
        // }
    }
}