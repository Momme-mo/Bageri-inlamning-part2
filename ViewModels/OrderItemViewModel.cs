namespace eshop.api.ViewModels;

public class OrderItemViewModel

 {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double PricePer { get; set; }
        public double TotalPrice { get; set; }

        
    }