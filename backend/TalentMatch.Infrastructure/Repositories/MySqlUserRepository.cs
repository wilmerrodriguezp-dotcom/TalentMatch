using Microsoft.Extensions.Configuration;
using MySqlConnector;
using TalentMatch.Application.Dtos;
using TalentMatch.Application.Interfaces;

namespace TalentMatch.Infrastructure.Repositories;

public sealed class MySqlUserRepository : IUserRepository
{
    private readonly string _connectionString;

    public MySqlUserRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("No se configuró ConnectionStrings:DefaultConnection.");
    }

    public async Task<UserCredentialsDto?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        const string sql = "SELECT UserName, PasswordHash, Role FROM Users WHERE UserName = @userName AND IsActive = 1 LIMIT 1";
        await using var command = new MySqlCommand(sql, connection);
        command.Parameters.Add("@userName", MySqlDbType.VarChar, 100).Value = userName;

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
        {
            return null;
        }

        return new UserCredentialsDto(reader.GetString(0), reader.GetString(1), reader.GetString(2));
    }

    public async Task CreateAsync(UserCredentialsDto user, CancellationToken cancellationToken = default)
    {
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        const string sql = "INSERT INTO Users (UserName, PasswordHash, Role) VALUES (@userName, @passwordHash, @role)";
        await using var command = new MySqlCommand(sql, connection);
        command.Parameters.Add("@userName", MySqlDbType.VarChar, 100).Value = user.UserName;
        command.Parameters.Add("@passwordHash", MySqlDbType.VarChar, 255).Value = user.PasswordHash;
        command.Parameters.Add("@role", MySqlDbType.VarChar, 20).Value = user.Role;
        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}
