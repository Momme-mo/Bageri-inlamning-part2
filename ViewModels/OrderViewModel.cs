namespace eshop.api.Entities
{
    public class OrderViewModel
    {
        public IEnumerable<Product> ProductOrder { get; set; }
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderNumber { get; set; }

        public Customer Customer { get; set; }
        public IList<OrderItem> OrderItems { get; set; }
    }
}