namespace eshop.api.ViewModels;

    public class CustomerViewModel
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ContactPerson { get; set; }
        public string DeliveryAddress { get; set; }
        public string InvoiceAddress { get; set; }
        public IList<CustomerOrderPostViewModel> Orders { get; set; }

    }