using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Dolcecuore.Application.Common;
using Dolcecuore.Services.Catalog.Api.Commands;
using Dolcecuore.Services.Catalog.Api.Entities;
using Dolcecuore.Services.Catalog.Api.Models;
using Dolcecuore.Services.Catalog.Api.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dolcecuore.Services.Catalog.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CatalogController : ControllerBase
{
    private readonly Dispatcher _dispatcher;

    public CatalogController(Dispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }
        
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await _dispatcher.DispatchAsync(new GetProductsQuery());
        return Ok(products);
    }
        
    [HttpGet("{id:guid}", Name = "GetProduct")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ProductModel>> GetProductById(Guid id)
    {
        var product = await _dispatcher.DispatchAsync(new GetProductQuery(id, true));
        var model = product.ToModel();
        return Ok(model);
    }
        
    [Route("[action]/{category}", Name = "GetProductByCategory")]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public ActionResult<IEnumerable<Product>> GetProductByCategory(string category)
    {
        return Ok();
    }
        
    [Route("[action]/{name}", Name = "GetProductByName")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public ActionResult<IEnumerable<Product>> GetProductByName(string name)
    {
        return NotFound();
    }
        
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] ProductModel model)
    {
        var product = model.ToEntity();
        await _dispatcher.DispatchAsync(new AddUpdateProductCommand(product));
        model = product.ToModel();
        return CreatedAtRoute("GetProduct", new { id = model.Id }, model);
    }
        
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct([FromBody] ProductModel model, Guid id)
    {
        var product = await _dispatcher.DispatchAsync(new GetProductQuery(id, true));

        product.Description = model.Description;
        product.Category = model.Category;
        product.Name = model.Name;
        product.ImagePath = model.ImagePath;
        product.Price = model.Price;

        await _dispatcher.DispatchAsync(new AddUpdateProductCommand(product));
        
        return NoContent();
    }

    [HttpDelete("{id:guid}", Name = "DeleteProduct")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProductById(Guid id)
    {
        var product = await _dispatcher.DispatchAsync(new GetProductQuery(id, true));

        await _dispatcher.DispatchAsync(new DeleteProductCommand(product));

        return NoContent();
    }
}