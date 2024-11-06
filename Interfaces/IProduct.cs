using WebApplication191024_Shop.Models;
using WebApplication191024_Shop.Models.Pages;

namespace WebApplication191024_Shop.Interfaces
{
    public interface IProduct
    {
        PagedList<Product> GetProducts(QueryOptions options);
        IEnumerable<Product> GetAllProducts();
        void AddProduct(Product product);
        Product GetProduct(int id);
        void UpdateProduct(Product product);
        void UpdateAll(Product[] products);
        void DeleteProduct(Product product);
        Product GetProductWithCategory(int id);
        IEnumerable<Product> GetTopSellingProducts(int topN);
    }
}
