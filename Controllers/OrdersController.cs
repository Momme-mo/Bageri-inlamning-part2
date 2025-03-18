using eshop.api.Data;
using eshop.api.Entities;
using eshop.api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eshop.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(DataContext context) : ControllerBase
{
    private readonly DataContext _context = context;


    [HttpGet]
    public async Task<ActionResult> ListAll()
    {
        var orders = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Select(order => new
            {
                order.OrderNumber,
                order.OrderDate,
                CustomerName = order.Customer.Name,
                ProductOrders = order.OrderItems.Select(oi => new
                {
                    oi.Product.ProductName,
                    oi.Product.PricePer,
                    oi.Quantity,
                    LineSum = oi.Product.PricePer * oi.Quantity
                })
            })
            .ToListAsync();
        return Ok(new { success = true, StatusCode = 200, data = orders });
    }



    [HttpGet("{orderNumber}")]
    public async Task<IActionResult> GetByOrderNumber(string orderNumber)
    {
        var order = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Select(order => new
            {
                order.OrderNumber,
                order.OrderDate,
                CustomerName = order.Customer.Name,
                ProductOrders = order.OrderItems.Select(oi => new
                {
                    oi.Product.ProductName,
                    oi.Product.PricePer,
                    oi.Quantity,
                    LineSum = oi.Product.PricePer * oi.Quantity
                })
            })
            .SingleOrDefaultAsync(o => o.OrderNumber == orderNumber);

        if (order is null)
            return NotFound(new { success = false, message = $"Could not find an order for ordernumber: {orderNumber}" });
        else
            return Ok(new { success = true, order });
    }

   [HttpGet("getbydate/{orderDate}")]
public async Task<IActionResult> GetByDate(DateTime orderDate)
{
    // Convert parameter to date only (without time)
    var dateOnly = orderDate.Date;
    
    var orders = await _context.Orders
        .Include(o => o.Customer)
        .Include(o => o.OrderItems)
        .ThenInclude(oi => oi.Product)
        .Where(o => o.OrderDate.Date == dateOnly)
        .Select(order => new
        {
            order.OrderDate,
            order.OrderNumber,
            CustomerName = order.Customer.Name,
            ProductOrders = order.OrderItems.Select(oi => new
            {
                oi.Product.ProductName,
                oi.Product.PricePer,
                oi.Quantity,
                LineSum = oi.Product.PricePer * oi.Quantity
            })
        })
        .ToListAsync();

    if (!orders.Any())
        return NotFound(new { success = false, message = $"Could not find any orders for date: {dateOnly:yyyy-MM-dd}" });
    else
        return Ok(new { success = true, data = orders });
}

   [HttpPost]
public async Task<ActionResult> AddOrder(CustomerOrderPostViewModel orderModel)
{
    var customer = await _context.Customers.FindAsync(orderModel.CustomerId);
    if (customer == null)
    {
        return BadRequest(new { success = false, StatusCode = 400, message = $"Customer with id: {orderModel.CustomerId} could not be found." });
    }

    string orderNumber = $"ORD-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8)}";
    
    var newOrder = new Order
    {
        OrderDate = orderModel.OrderDate,
        CustomerId = orderModel.CustomerId,
        OrderNumber = orderNumber,
        OrderItems = []
    };
    
    foreach (var product in orderModel.OrderProducts)
    {
        var prod = await _context.Products.SingleOrDefaultAsync(p => p.ProductId == product.ProductId);
        if (prod == null)
        {
            return BadRequest(new { success = false, StatusCode = 400, message = $"Product with id: {product.ProductId} could not be found." });
        }
        
        var item = new OrderItem
        {
            ProductId = product.ProductId,
            Quantity = product.Quantity,
         
        };
        
        newOrder.OrderItems.Add(item);
    }
    

    if (orderModel.OrderProducts.Any())
    {
        newOrder.ProductId = orderModel.OrderProducts.First().ProductId;
    }
    
    try
    {
        _context.Orders.Add(newOrder);
        await _context.SaveChangesAsync();
        
        return Ok(new { 
            success = true, 
            StatusCode = 201, 
            message = "Order has been made.",
            data = new {
                newOrder.CustomerId,
                newOrder.OrderNumber,
                newOrder.OrderDate,
                OrderItems = newOrder.OrderItems.Select(oi => new {
                oi.ProductId,
                oi.Quantity
                }),
                newOrder.OrderId
            }
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return BadRequest(new { success = false, StatusCode = 500, message = "Something went wrong when creating the order." });
    }
}
}