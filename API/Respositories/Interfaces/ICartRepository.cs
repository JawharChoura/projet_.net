using API.Models;
using System.Threading.Tasks;

namespace API.Repositories
{
    public interface ICartRepository
    {
        Task<Cart?> GetByUserIdAsync(string userId);
        Task<CartItem> AddItemToCartAsync(string userId, CartItem item);
        Task<CartItem?> UpdateCartItemAsync(int cartItemId, int quantity);
        Task<bool> RemoveItemFromCartAsync(int cartItemId);
        Task<bool> ClearCartAsync(string userId); // Corrigé : Task<bool>
    }
}