using Microsoft.AspNetCore.Mvc;
using WebApplication191024_Shop.Interfaces;
using WebApplication191024_Shop.Models;

namespace WebApplication191024_Shop.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategory _categories;

        public CategoriesController(ICategory categories)
        {
            _categories = categories;
        }
        public IActionResult Index()
        {
            return View(_categories.GetAllCategories());
        }
        [HttpPost]
        public IActionResult AddCategory(Category category)
        {
            _categories.AddCategory(category);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult EditCategory(long id)
        {
            ViewBag.Editid = id;
            return View(nameof(Index), _categories.GetAllCategories());
        }
        [HttpPost]
        public IActionResult UpdateCategory(Category category)
        {
            _categories.UpdateCategory(category);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult DeleteCategory(Category category)
        {
            _categories.DeleteCategory(category);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult UpdateAllCategories()
        {
            return View(_categories.GetAllCategories());
        }

        [HttpPost]
        public IActionResult UpdateAllCategories(List<Category> categories) 
        {
            try
            {
                _categories.UpdateAll(categories.ToArray());
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(categories);
            }
        }


    }
}
