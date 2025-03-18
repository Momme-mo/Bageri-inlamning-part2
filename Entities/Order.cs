
namespace eshop.api.Entities;

public class Order
{
      public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public int ProductId { get; set; }
    public DateTime OrderDate { get; set; }
    public string OrderNumber { get; set; }
    // Add navigation property for Customer
    public Customer Customer { get; set; }
    public IList<Product> ProductOrder { get; set; }

    public IList<OrderItem> OrderItems { get; set; }
}