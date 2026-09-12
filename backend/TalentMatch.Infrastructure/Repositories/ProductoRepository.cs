using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TalentMatch.Application.Interfaces;
using TalentMatch.Domain.Entities;

namespace TalentMatch.Infrastructure.Repositories;

public class ProductoRepository : IProductoRepository
{
    private readonly string _connectionString;

    public ProductoRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("No se ha configurado la cadena de conexión.");
    }

    public async Task<IEnumerable<Producto>> ObtenerTodosAsync()
    {
        var productos = new List<Producto>();

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("SELECT Id, Nombre, Precio, Stock FROM Productos ORDER BY Id", connection);
        await connection.OpenAsync();

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            productos.Add(new Producto
            {
                Id = reader.GetInt32("Id"),
                Nombre = reader.GetString("Nombre"),
                Precio = reader.GetDecimal("Precio"),
                Stock = reader.GetInt32("Stock")
            });
        }

        return productos;
    }

    public async Task<Producto?> ObtenerPorIdAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("SELECT Id, Nombre, Precio, Stock FROM Productos WHERE Id = @Id", connection);
        command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return new Producto
        {
            Id = reader.GetInt32("Id"),
            Nombre = reader.GetString("Nombre"),
            Precio = reader.GetDecimal("Precio"),
            Stock = reader.GetInt32("Stock")
        };
    }

    public async Task<int> CrearAsync(Producto producto)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "INSERT INTO Productos (Nombre, Precio, Stock) OUTPUT INSERTED.Id VALUES (@Nombre, @Precio, @Stock)",
            connection);

        command.Parameters.Add("@Nombre", SqlDbType.VarChar, 150).Value = producto.Nombre;
        command.Parameters.Add("@Precio", SqlDbType.Decimal).Value = producto.Precio;
        command.Parameters.Add("@Stock", SqlDbType.Int).Value = producto.Stock;

        await connection.OpenAsync();
        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public async Task ActualizarAsync(Producto producto)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "UPDATE Productos SET Nombre = @Nombre, Precio = @Precio, Stock = @Stock WHERE Id = @Id",
            connection);

        command.Parameters.Add("@Id", SqlDbType.Int).Value = producto.Id;
        command.Parameters.Add("@Nombre", SqlDbType.VarChar, 150).Value = producto.Nombre;
        command.Parameters.Add("@Precio", SqlDbType.Decimal).Value = producto.Precio;
        command.Parameters.Add("@Stock", SqlDbType.Int).Value = producto.Stock;

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task EliminarAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("DELETE FROM Productos WHERE Id = @Id", connection);
        command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }
}
