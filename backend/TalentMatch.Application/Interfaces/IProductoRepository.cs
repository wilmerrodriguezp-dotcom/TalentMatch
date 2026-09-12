using TalentMatch.Domain.Entities;

namespace TalentMatch.Application.Interfaces;

public interface IProductoRepository
{
    Task<IEnumerable<Producto>> ObtenerTodosAsync();
    Task<Producto?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(Producto producto);
    Task ActualizarAsync(Producto producto);
    Task EliminarAsync(int id);
}
