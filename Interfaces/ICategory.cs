using WebApplication191024_Shop.Models;

namespace WebApplication191024_Shop.Interfaces
{
    public interface ICategory
    {
        IEnumerable<Category> GetAllCategories();
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void UpdateAll(Category[] categories);
        void DeleteCategory(Category category);
    }
}
