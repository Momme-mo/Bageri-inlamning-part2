namespace eshop.api.Entities;

  public class ProductViewModel
    {
        public string ProductName { get; set; }
        public double PricePer { get; set; }
        public int ItemNumber { get; set; }
        public string BestBeforeDate { get; set; }
        public string ManufacturingDate { get; set; }
        public IList<Product> ProductOrder { get; set; }
    }