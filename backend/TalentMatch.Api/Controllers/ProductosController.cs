using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentMatch.Application.Dtos;
using TalentMatch.Application.Interfaces;

namespace TalentMatch.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _productoService;

    public ProductosController(IProductoService productoService)
    {
        _productoService = productoService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(IEnumerable<ProductoResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerProductos()
    {
        var productos = await _productoService.ObtenerTodosAsync();
        return Ok(productos);
    }

    [HttpGet("admin-check")]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult AdminCheck()
    {
        return Ok(new { mensaje = "Acceso permitido para administradores." });
    }

    [HttpGet("user-check")]
    [Authorize(Policy = "UserOrAdmin")]
    public IActionResult UserCheck()
    {
        return Ok(new { mensaje = "Acceso permitido para usuarios y administradores." });
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ProductoResponseDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CrearProducto([FromBody] CrearProductoDto dto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var productoCreado = await _productoService.CrearAsync(dto);
        return Created($"/api/v1/productos/{productoCreado.Id}", productoCreado);
    }
}
