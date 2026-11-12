using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;
using API.Respositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Respositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ApplicationDbContext _context;

        public ArticleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Article>> GetAllAsync()
        {
            return await _context.Articles
                .Include(a => a.Category)
                .ToListAsync();
        }

        public async Task<Article?> GetByIdAsync(int id)
        {
            return await _context.Articles
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Article> AddAsync(Article article)
        {
            var result = await _context.Articles.AddAsync(article);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Article?> UpdateAsync(Article article)
        {
            var result = await _context.Articles
                .FirstOrDefaultAsync(a => a.Id == article.Id);

            if (result == null) return null;

            result.Name = article.Name;
            result.Description = article.Description;
            result.Price = article.Price;
            result.CategoryId = article.CategoryId;
            result.ImageUrl = article.ImageUrl;
            result.IsAvailable = article.IsAvailable;

            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<Article?> DeleteAsync(int id)
        {
            var result = await _context.Articles
                .FirstOrDefaultAsync(a => a.Id == id);

            if (result == null) return null;

            _context.Articles.Remove(result);
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<IEnumerable<Article>> SearchAsync(string name, int? categoryId)
        {
            IQueryable<Article> query = _context.Articles.Include(a => a.Category);

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(a => a.Name.Contains(name) ||
                                        a.Description.Contains(name));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(a => a.CategoryId == categoryId.Value);
            }

            return await query.ToListAsync();
        }
    }
}
