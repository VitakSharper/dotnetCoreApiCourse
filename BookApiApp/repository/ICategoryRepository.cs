using BookApiApp.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookApiApp.repository
{
    public interface ICategoryRepository
    {
        Task<ICollection<Category>> GetCategories();
        Task<Category> GetCategory(int id);
        Task<ICollection<Category>> GetCategoriesOfABook(int id);
        Task<ICollection<Book>> GetBooksForCategory(int id);
        Task<bool> CategoryExists(int id);
        Task<bool> CategoryExistsByName(string categoryName);
        Task<bool> IsCategoryDuplicate(string categoryName, int id);

    }
}
