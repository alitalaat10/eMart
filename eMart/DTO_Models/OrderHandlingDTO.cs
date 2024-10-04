using eMart.Models;

namespace eMart.DTO_Models
{
    public class OrderHandlingDTO
    {
        public OrderHandlingDTO(Order order , IEnumerable<OrderProducts> products) 
        {
            this.order = order; 
            this.products = new List<OrderProducts>(products);
           
        }
        public Order order;
        public IEnumerable<OrderProducts> products;
        
        public Review Review { get; set; }
        
    }
}
