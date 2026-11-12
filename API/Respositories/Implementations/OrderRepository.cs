using API.Data;
using API.Models;
using API.Respositories.Interfaces;
using API.Respositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Article)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Article)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Article)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<Order> AddAsync(Order order)
        {
            // Générer un numéro de commande unique
            order.OrderNumber = GenerateOrderNumber();

            var result = await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Order?> UpdateAsync(Order order)
        {
            var result = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == order.Id);

            if (result == null) return null;

            result.Status = order.Status;
            result.PaymentStatus = order.PaymentStatus;
            result.DeliveryAddress = order.DeliveryAddress;
            result.CustomerNotes = order.CustomerNotes;

            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<Order?> DeleteAsync(int id)
        {
            var result = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == id);

            if (result == null) return null;

            _context.Orders.Remove(result);
            await _context.SaveChangesAsync();
            return result;
        }

        private string GenerateOrderNumber()
        {
            return $"CMD-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";
        }
    }
}