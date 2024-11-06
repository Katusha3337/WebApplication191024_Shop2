using WebApplication191024_Shop.Models;

namespace WebApplication191024_Shop.Interfaces
{
    public interface IOrder
    {
        IEnumerable<Order> GetAllOrders();
        Order GetOrder(int id);
        void AddOrder(Order order);
        void UpdateOrder(Order order);
        void DeleteOrder(Order order);
    }
}
