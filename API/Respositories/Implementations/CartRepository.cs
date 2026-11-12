using API.Data;
using API.Models;
using API.Respositories.Interfaces;
using API.Respositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories.Implementations
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cart?> GetByUserIdAsync(string userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Article)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<CartItem> AddItemToCartAsync(string userId, CartItem item)
        {
            // Vérifier si l'utilisateur a déjà un panier
            var cart = await GetByUserIdAsync(userId);

            if (cart == null)
            {
                // Créer un nouveau panier
                cart = new Cart
                {
                    UserId = userId,
                    TotalAmount = 0,
                    LastUpdated = DateTime.UtcNow
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            // Vérifier si l'article est déjà dans le panier
            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cart.Id && ci.ArticleId == item.ArticleId);

            if (existingItem != null)
            {
                // Mettre à jour la quantité
                existingItem.Quantity += item.Quantity;
                existingItem.UnitPrice = item.UnitPrice;
            }
            else
            {
                // Ajouter un nouvel item
                item.CartId = cart.Id;
                _context.CartItems.Add(item);
            }

            // Mettre à jour le total du panier
            await UpdateCartTotalAsync(cart.Id);

            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<CartItem?> UpdateCartItemAsync(int cartItemId, int quantity)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId);

            if (cartItem == null) return null;

            cartItem.Quantity = quantity;

            // Mettre à jour le total du panier
            await UpdateCartTotalAsync(cartItem.CartId);

            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task<bool> RemoveItemFromCartAsync(int cartItemId)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId);

            if (cartItem == null) return false;

            _context.CartItems.Remove(cartItem);

            // Mettre à jour le total du panier
            await UpdateCartTotalAsync(cartItem.CartId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ClearCartAsync(string userId)
        {
            var cart = await GetByUserIdAsync(userId);

            if (cart == null) return false;

            // Supprimer tous les items du panier
            _context.CartItems.RemoveRange(cart.CartItems);

            // Réinitialiser le total
            cart.TotalAmount = 0;
            cart.LastUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        private async Task UpdateCartTotalAsync(int cartId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.Id == cartId);

            if (cart != null)
            {
                cart.TotalAmount = cart.CartItems.Sum(ci => ci.Quantity * ci.UnitPrice);
                cart.LastUpdated = DateTime.UtcNow;
            }
        }
    }
}