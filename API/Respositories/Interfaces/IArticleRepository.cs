using API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace API.Respositories.Interfaces
{
    public interface IArticleRepository
    {
        Task<IEnumerable<Article>> GetAllAsync();
        Task<Article?> GetByIdAsync(int id); // Retour nullable
        Task<Article> AddAsync(Article article);
        Task<Article?> UpdateAsync(Article article); // Retour nullable
        Task<Article?> DeleteAsync(int id); // Retour nullable
        Task<IEnumerable<Article>> SearchAsync(string name, int? categoryId);
    }
}
