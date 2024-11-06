using Microsoft.AspNetCore.Mvc;
using WebApplication191024_Shop.Data;
using WebApplication191024_Shop.Interfaces;
using WebApplication191024_Shop.Models;

namespace WebApplication191024_Shop.Repository
{
    public class CategoryRepository : ICategory
    {
        private ApplicationContext _context;

        public CategoryRepository(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _context.Categories;
        }

        public void AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        public void UpdateAll(Category[] categories)
        {
            Dictionary<int, Category> data = categories.ToDictionary(e => e.Id);
            IEnumerable<Category> baseline = _context.Categories.Where(e => data.Keys.Contains(e.Id));

            foreach (Category category in baseline)
            {
                Category requestCategory = data[category.Id];
                category.Name = requestCategory.Name;
                category.Description = requestCategory.Description;
            }

            _context.SaveChanges();
        }

        public void DeleteCategory(Category category)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }
    }

}
