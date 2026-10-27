using System.ComponentModel.DataAnnotations;

namespace ValeraApi.DTOs;

public class CreateValeraDto
{
    [MaxLength(100)]
    public string? Name { get; set; }

    [MaxLength(100)]
    public string? Nickname { get; set; }
}

public class UpdateValeraDto
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = "";

    [MaxLength(100)]
    public string? Nickname { get; set; }
}

public record ValeraDto(
    int Id,
    string Name,
    int Health,
    int Alcohol,
    int Joy,
    int Fatigue,
    int Money,
    string Nickname);

public record ValeraActionResultDto(bool Success, string Message, ValeraDto Valera);