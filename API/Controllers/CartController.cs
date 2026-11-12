using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _repository;
        public CartController(ICartRepository repository) => _repository = repository;

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart(string userId)
        {
            var cart = await _repository.GetByUserIdAsync(userId);
            return cart is null ? NotFound() : Ok(cart);
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> AddItem(string userId, CartItem item)
        {
            var added = await _repository.AddItemToCartAsync(userId, item);
            return Ok(added);
        }

        [HttpPut("{cartItemId}")]
        public async Task<IActionResult> UpdateItem(int cartItemId, [FromBody] int quantity)
        {
            var updated = await _repository.UpdateCartItemAsync(cartItemId, quantity);
            return updated is null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{cartItemId}")]
        public async Task<IActionResult> RemoveItem(int cartItemId)
        {
            var success = await _repository.RemoveItemFromCartAsync(cartItemId);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("clear/{userId}")]
        public async Task<IActionResult> ClearCart(string userId)
        {
            var success = await _repository.ClearCartAsync(userId);
            return success ? NoContent() : NotFound();
        }
    }

}
