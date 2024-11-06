using Microsoft.EntityFrameworkCore;
using WebApplication191024_Shop.Data;
using WebApplication191024_Shop.Interfaces;
using WebApplication191024_Shop.Models;

namespace WebApplication191024_Shop.Repository
{
    public class OrderRepository : IOrder
    {
        private ApplicationContext _context;
        public OrderRepository(ApplicationContext context)
        {
            _context = context;
        }
        public IEnumerable<Order> GetAllOrders()
        {
            return _context.Orders.Include(e => e.Lines).ThenInclude(e => e.Product);
        }
        public Order GetOrder(int id)
        {
            return _context.Orders.Include(e => e.Lines).FirstOrDefault(e => e.Id == id);
        }
        public void AddOrder(Order order)
        {
            try
            {
                foreach (var line in order.Lines)
                {
                    line.Id = 0; // Обнуляем идентификатор OrderLine
                                 // Убеждаемся, что ProductId уже установлен
                    if (line.Product != null)
                    {
                        line.ProductId = line.Product.Id;
                        line.Product = null;
                    }
                }

                _context.Orders.Add(order);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message;
                Console.WriteLine($"DbUpdateException: {ex.Message}, InnerException: {innerExceptionMessage}");
                throw new Exception("Произошла ошибка при сохранении заказа", ex);
            }
        }

        public void DeleteOrder(Order order)
        {
            _context.Orders.Remove(order);
            _context.SaveChanges();
        }
        public void UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }
    }
}
