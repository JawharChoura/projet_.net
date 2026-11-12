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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories
                .Include(c => c.Articles)
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.Articles)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category> AddAsync(Category category)
        {
            var result = await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            var result = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == category.Id);

            if (result == null) return null;

            result.Name = category.Name;
            result.Description = category.Description;
            result.ImageUrl = category.ImageUrl;

            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<Category?> DeleteAsync(int id)
        {
            var result = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id);

            if (result == null) return null;

            _context.Categories.Remove(result);
            await _context.SaveChangesAsync();
            return result;
        }
    }
}