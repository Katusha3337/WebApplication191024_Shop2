using Microsoft.AspNetCore.Mvc;
using WebApplication191024_Shop.Interfaces;
using WebApplication191024_Shop.Models;
using WebApplication191024_Shop.Repository;

namespace WebApplication191024_Shop.ViewComponents
{
    public class TopSellingProductsViewComponent : ViewComponent 
    { 
        private readonly IProduct _productRepository; 
        public TopSellingProductsViewComponent(IProduct productRepository) 
        { 
            _productRepository = productRepository; 
        } 
        public IViewComponentResult Invoke()
        { 
            var topProducts = _productRepository.GetTopSellingProducts(10); 
            return View(topProducts); 
        } 
    }
}
