using Blazored.LocalStorage;
using Front.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Front.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _storage;

        public AuthService(HttpClient http, ILocalStorageService storage)
        {
            _http = http;
            _storage = storage;
        }

        public async Task<bool> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/Account/Register", request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Register error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> LoginAsync(LoginRequest request)
        {
            try
            {
                Console.WriteLine($"Tentative de login: {request.Email}");

                var response = await _http.PostAsJsonAsync("api/Account/Login", request);

                Console.WriteLine($"Status: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Login failed: {error}");
                    return false;
                }

                var jsonString = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response received ({jsonString.Length} chars)");

                // Debug: Afficher le début de la réponse
                Console.WriteLine($"First 200 chars: {jsonString.Substring(0, Math.Min(200, jsonString.Length))}...");

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var result = JsonSerializer.Deserialize<LoginResponse>(jsonString, options);

                if (result == null || string.IsNullOrEmpty(result.Token))
                {
                    Console.WriteLine("No token in response");
                    return false;
                }

                Console.WriteLine($"Token received: {result.Token.Substring(0, Math.Min(20, result.Token.Length))}...");
                Console.WriteLine($"User ID: {result.User.Id}");
                Console.WriteLine($"Email: {result.User.Email}");
                Console.WriteLine($"Roles: {(result.User.Roles != null ? string.Join(", ", result.User.Roles) : "none")}");

                // Sauvegarder le token
                await _storage.SetItemAsync("authToken", result.Token);

                // Configurer le header pour les futures requêtes
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", result.Token);

                // Sauvegarder les infos utilisateur
                await _storage.SetItemAsync("userId", result.User.Id);
                await _storage.SetItemAsync("userEmail", result.User.Email);
                await _storage.SetItemAsync("userFirstName", result.User.FirstName);
                await _storage.SetItemAsync("userLastName", result.User.LastName);

                if (result.User.Roles != null && result.User.Roles.Length > 0)
                {
                    await _storage.SetItemAsync("userRoles", result.User.Roles);
                }

                Console.WriteLine("=== LOGIN SUCCESS ===");
                return true;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Error: {ex.Message}");
                Console.WriteLine($"Path: {ex.Path}, Line: {ex.LineNumber}");
                return false;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login Error: {ex.GetType().Name}: {ex.Message}");
                return false;
            }
        }

        public async Task<T?> GetItemAsync<T>(string key)
        {
            try
            {
                return await _storage.GetItemAsync<T>(key);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting {key}: {ex.Message}");
                return default;
            }
        }

        public async Task<string?> GetTokenAsync()
        {
            return await GetItemAsync<string?>("authToken");
        }

        public async Task<string?> GetUserIdAsync()
        {
            return await GetItemAsync<string?>("userId");
        }

        public async Task<string[]?> GetRolesAsync()
        {
            return await GetItemAsync<string[]?>("userRoles");
        }

        public async Task InitializeAuthHeader()
        {
            var token = await GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
                Console.WriteLine("Authorization header initialized");
            }
        }

        public async Task LogoutAsync()
        {
            var keys = new[] { "authToken", "userId", "userEmail", "userFirstName", "userLastName", "userRoles" };

            foreach (var key in keys)
            {
                try
                {
                    await _storage.RemoveItemAsync(key);
                }
                catch { }
            }

            _http.DefaultRequestHeaders.Authorization = null;
            Console.WriteLine("Logged out");
        }
    }
}