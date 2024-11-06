using System.ComponentModel.DataAnnotations;

namespace WebApplication191024_Shop.Models
{
    public class OrderViewModel
    {
        [Required(ErrorMessage = "Введите ваше имя")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Введите адрес")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Введите штат/область")]
        public string State { get; set; }

        [Required(ErrorMessage = "Введите почтовый индекс")]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Введите корректный почтовый индекс")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Корзина не может быть пустой")]
        public List<CartItemViewModel> CartItems { get; set; }
    }

}
