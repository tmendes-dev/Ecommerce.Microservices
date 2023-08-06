using System.Net;
using System.Reflection;
using System.Text.Json;
using Catalog.API.Entities;
using Catalog.API.Interfaces.Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CatalogController : ControllerBase
{
    private readonly ILogger<CatalogController> _logger;
    private readonly IProductRepository _repository;

    public CatalogController(ILogger<CatalogController> logger, IProductRepository productRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _repository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        try
        {
            var products = await _repository.GetAll();
            if (!products.Any())
                return NoContent();

            return Ok(products);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"An error ocurred in method {MethodBase.GetCurrentMethod()!.Name}");
            return StatusCode(500);
        }
    }

    [HttpGet("[action]/{category}", Name = "GetProductByCategory")]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
    {
        try
        {
            var products = await _repository.GetByCategory(category);
            if (!products.Any())
                return NoContent();

            return Ok(products);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"An error ocurred in method {MethodBase.GetCurrentMethod()!.Name}");
            return StatusCode(500);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
    {
        try
        {
            await _repository.Create(product);

            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"An error ocurred in method {MethodBase.GetCurrentMethod()!.Name}");
            return StatusCode(500);
        }
    }

    [HttpPut]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> UpdateProduct([FromBody] Product product)
    {
        try
        {
            var success = await _repository.Update(product);

            if (!success)
                throw new ApplicationException(
                    $"Failed to update product. Details:{JsonSerializer.Serialize(product)}");

            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"An error ocurred in method {MethodBase.GetCurrentMethod()!.Name}");
            return StatusCode(500);
        }
    }

    [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> DeleteProductById(string id)
    {
        try
        {
            var success = await _repository.Delete(id);
            if (!success)
                throw new ApplicationException(
                    $"Failed to delete product. ProductId:{JsonSerializer.Serialize(id)}");

            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"An error ocurred in method {MethodBase.GetCurrentMethod()!.Name}");
            return StatusCode(500);
        }
    }
}