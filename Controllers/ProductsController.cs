using eshop.api.Data;
using eshop.api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eshop.api.ViewModels;
namespace eshop.api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ProductsController(DataContext context) : ControllerBase
{
    private readonly DataContext _context = context;

    [HttpGet]
    public async Task<ActionResult> GetAllProducts()
    {
        var products = await _context.Products
            .Select(p => new
            {
                p.ProductId,
                p.ProductName,
                p.PricePer,
                p.ItemNumber,
                p.BestBeforeDate,
                p.ManufacturingDate

            })
            .ToListAsync();

        return Ok(new { success = true, data = products });
    }


    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound(new { success = false, StatusCode = 404, message = $"Produkten med id {id} kunde inte hittas." });
        }

        var productData = new
        {
            product.ProductId,
            product.ProductName,
            product.PricePer,
            product.ItemNumber,
            product.BestBeforeDate,
            product.ManufacturingDate

        };

        return Ok(new { success = true, StatusCode = 200, data = productData });
    }





    [HttpPost]
    public async Task<ActionResult> AddProduct(AddProductViewModel model)
    {
        var product = new Product
        {
            ProductName = model.ProductName,
            PricePer = model.PricePer,
            BestBeforeDate = model.BestBeforeDate,
            ManufacturingDate = model.ManufacturingDate,
            ItemNumber = GenerateItemNumber(model.ProductName)
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var productResponse = new
        {
            product.ProductId,
            product.ProductName,
            product.PricePer,
            product.ItemNumber,
            product.BestBeforeDate,
            product.ManufacturingDate
        };

        return CreatedAtAction(
            nameof(GetById),
            new { id = product.ProductId },
            new { success = true, StatusCode = 201, data = productResponse }
        );
    }

    private string GenerateItemNumber(string productName)
    {
        string prefix = !string.IsNullOrEmpty(productName) && productName.Length >= 2
            ? productName.Substring(0, 2).ToUpper()
            : "PR";

        return $"{prefix}{DateTime.Now.ToString("yyyyMMddHHmmss")}";
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePrice(int id, double newPrice)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound(new { success = false, StatusCode = 404, message = $"Produkten med id {id} kunde inte hittas." });
        }

        product.PricePer = newPrice;
        await _context.SaveChangesAsync();

        return NoContent();
    }

}