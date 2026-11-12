using API.Models;
using API.Respositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // GET: api/Orders/user/{userId} - Récupérer les commandes d'un utilisateur
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserOrders(string userId)
        {
            try
            {
                var orders = await _orderRepository.GetByUserIdAsync(userId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la récupération des commandes: {ex.Message}");
            }
        }

        // GET: api/Orders/{id} - Récupérer une commande par ID
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(id);
                if (order == null)
                    return NotFound("Commande non trouvée");

                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la récupération de la commande: {ex.Message}");
            }
        }

        // POST: api/Orders - Créer une nouvelle commande
        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            try
            {
                if (order == null || order.OrderItems == null || !order.OrderItems.Any())
                    return BadRequest("Commande invalide");

                var createdOrder = await _orderRepository.AddAsync(order);
                return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.Id }, createdOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la création de la commande: {ex.Message}");
            }
        }

        // PUT: api/Orders/{id} - Mettre à jour une commande (pour admin)
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrder(int id, Order order)
        {
            try
            {
                if (id != order.Id)
                    return BadRequest("ID de commande ne correspond pas");

                var updatedOrder = await _orderRepository.UpdateAsync(order);
                if (updatedOrder == null)
                    return NotFound("Commande non trouvée");

                return Ok(updatedOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la mise à jour de la commande: {ex.Message}");
            }
        }

        // DELETE: api/Orders/{id} - Supprimer une commande (pour admin)
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var deletedOrder = await _orderRepository.DeleteAsync(id);
                if (deletedOrder == null)
                    return NotFound("Commande non trouvée");

                return Ok(deletedOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la suppression de la commande: {ex.Message}");
            }
        }

        // GET: api/Orders - Récupérer toutes les commandes (pour admin)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orders = await _orderRepository.GetAllAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la récupération des commandes: {ex.Message}");
            }
        }
    }
}
