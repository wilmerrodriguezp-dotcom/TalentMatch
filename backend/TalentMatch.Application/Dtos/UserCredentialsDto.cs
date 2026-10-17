namespace TalentMatch.Application.Dtos;

public sealed record UserCredentialsDto(string UserName, string PasswordHash, string Role);
