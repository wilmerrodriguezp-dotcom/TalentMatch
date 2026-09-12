using TalentMatch.Application.Dtos;
using TalentMatch.Application.Interfaces;
using TalentMatch.Domain.Entities;

namespace TalentMatch.Application.Services;

public class ProductoService : IProductoService
{
    private readonly IProductoRepository _productoRepository;

    public ProductoService(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<IEnumerable<ProductoResponseDto>> ObtenerTodosAsync()
    {
        var productos = await _productoRepository.ObtenerTodosAsync();
        return productos.Select(p => new ProductoResponseDto
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Precio = p.Precio,
            Stock = p.Stock
        });
    }

    public async Task<ProductoResponseDto?> ObtenerPorIdAsync(int id)
    {
        var producto = await _productoRepository.ObtenerPorIdAsync(id);
        if (producto == null)
            return null;

        return new ProductoResponseDto
        {
            Id = producto.Id,
            Nombre = producto.Nombre,
            Precio = producto.Precio,
            Stock = producto.Stock
        };
    }

    public async Task<ProductoResponseDto> CrearAsync(CrearProductoDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre))
            throw new ArgumentException("El nombre es obligatorio.");

        if (dto.Precio <= 0)
            throw new ArgumentException("El precio debe ser mayor que cero.");

        if (dto.Stock < 0)
            throw new ArgumentException("El stock no puede ser negativo.");

        var producto = new Producto
        {
            Nombre = dto.Nombre.Trim(),
            Precio = dto.Precio,
            Stock = dto.Stock
        };

        var id = await _productoRepository.CrearAsync(producto);
        producto.Id = id;

        return new ProductoResponseDto
        {
            Id = producto.Id,
            Nombre = producto.Nombre,
            Precio = producto.Precio,
            Stock = producto.Stock
        };
    }
}
