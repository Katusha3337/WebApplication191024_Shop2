using Microsoft.AspNetCore.Mvc;
using WebApplication191024_Shop.Interfaces;
using WebApplication191024_Shop.Models.Pages;
using WebApplication191024_Shop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace WebApplication191024_Shop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProduct _products;
        private readonly ICategory _categories;
        private readonly IOrder _orders;

        public ProductsController(IProduct products, ICategory categories, IOrder orders)
        {
            _products = products;
            _categories = categories;
            _orders = orders;
        }

        public IActionResult Index(QueryOptions options)
        {
            var products = _products.GetProducts(options);
            return View(products);
        }

        public IActionResult Detail(int id)
        {
            var product = _products.GetProductWithCategory(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            var product = _products.GetProductWithCategory(productId);
            if (product == null)
            {
                return NotFound();
            }

            var cart = GetCart();
            var existingProduct = cart.FirstOrDefault(p => p.ProductId == productId);
            if (existingProduct != null)
            {
                existingProduct.Quantity += 1;
            }
            else
            {
                cart.Add(new CartItemViewModel
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Quantity = 1,
                    Price = product.RetailPrice
                });
            }
            SaveCart(cart);
            return RedirectToAction(nameof(Cart));
        }


        public IActionResult Cart()
        {
            var cart = GetCart();
            return View(cart);
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCart();
            var product = cart.FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                cart.Remove(product);
                SaveCart(cart);
            }
            return RedirectToAction(nameof(Cart));
        }

        [HttpPost]
        public IActionResult UpdateCart([FromBody] CartItemViewModel model)
        {
            var cart = GetCart();
            var product = cart.FirstOrDefault(p => p.ProductId == model.ProductId);
            if (product != null)
            {
                if (model.Quantity <= 0)
                {
                    cart.Remove(product);
                }
                else
                {
                    product.Quantity = model.Quantity;
                }
                SaveCart(cart);
            }
            return Json(new { success = true });
        }

        public IActionResult Checkout()
        {
            var cart = GetCart();
            if (cart.Count == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var orderViewModel = new OrderViewModel
            {
                CartItems = cart.Select(p => new CartItemViewModel
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Quantity = p.Quantity,
                    Price = p.Price
                }).ToList()
            };

            return View("~/Views/Orders/Checkout.cshtml", orderViewModel);
        }

        [HttpPost]
        public IActionResult Checkout(OrderViewModel orderViewModel)
        {
            var cart = GetCart();
            if (cart.Count == 0)
            {
                Console.WriteLine("Корзина пуста, перенаправление на Index.");
                return RedirectToAction(nameof(Index));
            }

            // Логируем содержимое корзины
            Console.WriteLine("Содержимое корзины:");
            foreach (var item in cart)
            {
                Console.WriteLine($"ProductId: {item.ProductId}, ProductName: {item.ProductName}, Quantity: {item.Quantity}, Price: {item.Price}");
            }

            if (!ModelState.IsValid)
            {
                Console.WriteLine("Модель не валидна. Ошибки:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                orderViewModel.CartItems = cart.Select(p => new CartItemViewModel
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Quantity = p.Quantity,
                    Price = p.Price
                }).ToList();

                return View("~/Views/Orders/Checkout.cshtml", orderViewModel);
            }

            var order = new Order
            {
                CustomerName = orderViewModel.CustomerName,
                Address = orderViewModel.Address,
                State = orderViewModel.State,
                ZipCode = orderViewModel.ZipCode,
                OrderDate = DateTime.Now,
                Lines = cart.Select(p => new OrderLine
                {
                    ProductId = p.ProductId,
                    Quantity = p.Quantity,
                    Product = _products.GetProduct(p.ProductId) // Загружаем объект Product для каждой строки
                }).ToList()
            };

            Console.WriteLine("Создание заказа:");

            try
            {
                _orders.AddOrder(order);
                SaveCart(new List<CartItemViewModel>());
                Console.WriteLine("Заказ успешно создан.");
                return RedirectToAction("OrderConfirmation", "Orders", new { orderId = order.Id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при оформлении заказа: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Произошла ошибка при оформлении заказа. Пожалуйста, попробуйте еще раз.");
                orderViewModel.CartItems = cart.Select(p => new CartItemViewModel
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Quantity = p.Quantity,
                    Price = p.Price
                }).ToList();
                return View("~/Views/Orders/Checkout.cshtml", orderViewModel);
            }
        }
        private List<CartItemViewModel> GetCart()
        {
            var cart = HttpContext.Session.Get<List<CartItemViewModel>>("Cart");
            return cart ?? new List<CartItemViewModel>();
        }

        private void SaveCart(List<CartItemViewModel> cart)
        {
            HttpContext.Session.Set("Cart", cart);
        }

        [HttpGet]
        public IActionResult UpdateAllProducts()
        {
            ViewBag.Categories = _categories.GetAllCategories().ToList(); // Загружаем категории
            var products = _products.GetAllProducts().ToList(); // Загружаем продукты
            return View(products);
        }

        [HttpPost]
        public IActionResult UpdateAllProducts([FromBody] List<Product> products)
        {
            try
            {
                _products.UpdateAll(products.ToArray());
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public IActionResult AddProductInline([FromBody] Product product)
        {
            if (ModelState.IsValid)
            {
                _products.AddProduct(product);
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Invalid product data" });
        }

    }
}
