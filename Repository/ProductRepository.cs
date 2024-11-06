using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication191024_Shop.Data;
using WebApplication191024_Shop.Interfaces;
using WebApplication191024_Shop.Models;
using WebApplication191024_Shop.Models.Pages;

namespace WebApplication191024_Shop.Repository
{
    public class ProductRepository : IProduct
    {
        private ApplicationContext _context;

        public ProductRepository(ApplicationContext context)
        {
            _context = context;
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public PagedList<Product> GetProducts(QueryOptions options)
        {
            return new PagedList<Product>(_context.Products.Include(e => e.Category), options);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products.Include(e => e.Category).ToList();
        }


        public Product GetProduct(int id)
        {
            return _context.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
        }

        public void UpdateProduct(Product product)
        {
            Product product2 = GetProduct(product.Id);
            product2.Name = product.Name;
            product2.CategoryId = product.CategoryId;
            product2.RetailPrice = product.RetailPrice;
            product2.PurchasePrice = product.PurchasePrice;
            product2.Quantity = product.Quantity;

            _context.SaveChanges();
        }

        public void UpdateAll(Product[] products)
        {
            Dictionary<int, Product> data = products.ToDictionary(e => e.Id);
            IEnumerable<Product> baseline = _context.Products.Where(e => data.Keys.Contains(e.Id));
            foreach (Product product in baseline)
            {
                Product requestProduct = data[product.Id];
                product.Name = requestProduct.Name;
                product.CategoryId = requestProduct.CategoryId;
                product.RetailPrice = requestProduct.RetailPrice;
                product.PurchasePrice = requestProduct.PurchasePrice;
                product.Quantity = requestProduct.Quantity;
            }
            _context.SaveChanges();
        }

        public void DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        public Product GetProductWithCategory(int id)
        {
            return _context.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
        }
        public IEnumerable<Product> GetTopSellingProducts(int topN) 
        { 
            return _context.Products
                            .Include(p => p.OrderLines)
                            .OrderByDescending(p => p.OrderLines.Sum(ol => ol.Quantity))
                            .Take(topN)
                            .ToList(); 
        }
    }

}
