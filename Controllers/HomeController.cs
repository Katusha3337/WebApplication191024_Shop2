using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication191024_Shop.Interfaces;
using WebApplication191024_Shop.Models;
using WebApplication191024_Shop.Models.Pages;

namespace WebApplication191024_Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProduct _products;
        private readonly ICategory _categories;
        public HomeController(IProduct products, ICategory categories)
        {
            _products = products;
            _categories = categories;
        }

        [HttpGet]
        public IActionResult Index(QueryOptions options)
        {
            return View(_products.GetProducts(options));
        }

        [HttpGet]
        public IActionResult UpdateProduct(int id)
        {
            ViewBag.Categories = _categories.GetAllCategories();
            return View(id == 0 ? new Product() : _products.GetProduct(id));
        }

        [HttpPost]
        public IActionResult UpdateProduct(Product product)
        {
            if (product.Id == 0)
            {
                _products.AddProduct(product);
            }
            else
            {
                _products.UpdateProduct(product);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult DeleteProduct(Product product)
        {
            _products.DeleteProduct(product);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                _products.AddProduct(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [HttpGet]
        public IActionResult UpdateAllProducts()
        {
            ViewBag.Categories = _categories.GetAllCategories();
            return View(_products.GetAllProducts());
        }

        [HttpPost]
        public IActionResult UpdateAllProducts(List<Product> products) 
        {
            try
            {
                _products.UpdateAll(products.ToArray());
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(products);
            }
        }

        public IActionResult TopSellingProducts()
        {
            return View();
        }

    }
}
