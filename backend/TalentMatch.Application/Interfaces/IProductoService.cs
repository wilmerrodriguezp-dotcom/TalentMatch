using TalentMatch.Application.Dtos;

namespace TalentMatch.Application.Interfaces;

public interface IProductoService
{
    Task<IEnumerable<ProductoResponseDto>> ObtenerTodosAsync();
    Task<ProductoResponseDto?> ObtenerPorIdAsync(int id);
    Task<ProductoResponseDto> CrearAsync(CrearProductoDto dto);
}
