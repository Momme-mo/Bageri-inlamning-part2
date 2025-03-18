using eshop.api.Entities;

namespace eshop.api.ViewModels;

 public class CustomerOrderPostViewModel
    {
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public IList<ProductOrderViewModel> OrderProducts { get; set; }
    }