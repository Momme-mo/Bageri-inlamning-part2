using System.ComponentModel.DataAnnotations;

namespace eshop.api.Entities;


public class Customer
{
    public int CustomerId { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string ContactPerson { get; set; }
    public string DeliveryAddress { get; set; }
    public string InvoiceAddress { get; set; }

    public IList<Order> Orders{ get; set; }

}