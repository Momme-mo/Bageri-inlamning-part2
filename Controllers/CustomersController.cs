using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eshop.api.Data;
using eshop.api.Entities;
using eshop.api.ViewModels;

namespace eshop.api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class CustomersController(DataContext context) : ControllerBase
{
    private readonly DataContext _context = context;

    [HttpGet()]
    public async Task<ActionResult> ListAll()
    {

        var customer = await _context.Customers
          .Select(customer => new
          {
              customer.CustomerId,
              customer.Name,
              customer.Phone,
              customer.Email,
              customer.ContactPerson,
              customer.DeliveryAddress,
              customer.InvoiceAddress
          })
          .ToListAsync();

        return Ok(new { success = true, statusCode = 200, data = customer });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetCustomer(int id)
    {
        try
        {
            var customer = await _context.Customers
                .Where(c => c.CustomerId == id)
                .Include(c => c.Orders)
                .ThenInclude(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Select(c => new
                {
                    c.CustomerId,
                    c.Name,
                    c.Phone,
                    c.Email,
                    c.ContactPerson,
                    c.DeliveryAddress,
                    c.InvoiceAddress,
                    Orders = c.Orders.Select(o => new
                    {
                        o.OrderId,
                        o.OrderDate,
                        o.OrderNumber,
                        ProductOrders = o.OrderItems.Select(oi => new
                        {
                            oi.Product.ProductName,
                            oi.Product.ProductId,
                            oi.Product.ItemNumber,
                            oi.Quantity,
                            oi.Product.PricePer,
                            oi.Product.BestBeforeDate,
                            oi.Product.ManufacturingDate
                        }).ToList()
                    }).ToList()
                })
                .SingleOrDefaultAsync();

            if (customer is null)
                return NotFound(new { success = false, message = $"Customer with id: {id} could not be found." });

            return Ok(new { success = true, data = customer });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }


    [HttpPost]
    public async Task<ActionResult<Customer>> CreateCustomer(CustomerPostViewModel model)
    {
        var prod = await _context.Customers.FirstOrDefaultAsync(c => c.Name == model.Name);
        if (prod != null)
        {
            return BadRequest(new { success = false, StatusCode = 400, message = $"Customer with name: {model.Name} already exists." });
        }

        var customer = new Customer
        {
            Name = model.Name,
            Phone = model.Phone,
            Email = model.Email,
            ContactPerson = model.ContactPerson,
            DeliveryAddress = model.DeliveryAddress,
            InvoiceAddress = model.InvoiceAddress
        };
        try
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();



            return Ok( new{succes = true, data = model} );
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }

    }


    [HttpPut("{id}")]
    public async Task<IActionResult> ContactPerson(int id, string Nykontakt)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
        {
            return NotFound(new { success = true, StatusCode = 404, message = $"Customer with id: {id} could not be found." });
        }

        customer.ContactPerson = Nykontakt;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}