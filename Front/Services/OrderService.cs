using Front.Models;
using System.Net.Http.Json;

namespace Front.Services;
public class OrderService
{
    private readonly HttpClient _http;
    private readonly AuthService _authService;

    public OrderService(HttpClient http, AuthService authService)
    {
        _http = http;
        _authService = authService;
    }

    // Récupérer les commandes de l'utilisateur connecté
    public async Task<List<Order>> GetMyOrdersAsync(string userId)
    {
        try
        {
            await _authService.InitializeAuthHeader();
            return await _http.GetFromJsonAsync<List<Order>>($"api/Orders/user/{userId}") ?? new List<Order>();
        }
        catch
        {
            return new List<Order>();
        }
    }

    // Créer une nouvelle commande
    public async Task<Order?> CreateOrderAsync(Order order)
    {
        try
        {
            await _authService.InitializeAuthHeader();
            var response = await _http.PostAsJsonAsync("api/Orders", order);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Order>();
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    // Récupérer une commande par ID
    public async Task<Order?> GetOrderByIdAsync(int orderId)
    {
        try
        {
            await _authService.InitializeAuthHeader();
            return await _http.GetFromJsonAsync<Order>($"api/Orders/{orderId}");
        }
        catch
        {
            return null;
        }
    }
}
