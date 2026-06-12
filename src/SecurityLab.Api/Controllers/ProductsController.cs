using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurityLab.Api.Contracts;
using SecurityLab.Api.Models;

namespace SecurityLab.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService) => _productService = productService;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetAll(CancellationToken ct)
    {
        return Ok(await _productService.GetAllAsync(ct));
    }

    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<Product>>> Search(
        [FromQuery] string name   = "",
        [FromQuery] string sortBy = "Name",
        CancellationToken  ct     = default)
    {
        return Ok(await _productService.SearchAsync(name, sortBy, ct));
    }
}
