using API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace API.Respositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id); // Retour nullable
        Task<Category> AddAsync(Category category);
        Task<Category?> UpdateAsync(Category category); // Retour nullable
        Task<Category?> DeleteAsync(int id); // Retour nullable
    }
}
