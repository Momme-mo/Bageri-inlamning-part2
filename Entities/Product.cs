namespace eshop.api.Entities;

public class Product
{
  public int ProductId { get; set; }
  public string ProductName { get; set; }
  public double PricePer { get; set; }
  public string ItemNumber { get; set; }
  public string BestBeforeDate { get; set; }
  public string ManufacturingDate { get; set; }
  public ICollection<OrderItem> OrderItems { get; set; }
  
}
