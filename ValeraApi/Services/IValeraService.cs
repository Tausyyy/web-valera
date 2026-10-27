using ValeraApi.DTOs;

namespace ValeraApi.Services;

public interface IValeraService
{
    Task<IReadOnlyList<ValeraDto>> GetAllAsync();
    Task<ValeraDto?> GetByIdAsync(int id);
    Task<ValeraDto> CreateAsync(CreateValeraDto dto);
    Task<ValeraDto?> UpdateAsync(int id, UpdateValeraDto dto);
    Task<bool> DeleteAsync(int id);

    // Действия Валеры. null — Валера не найден.
    Task<ValeraActionResultDto?> GoToWorkAsync(int id);
    Task<ValeraActionResultDto?> ContemplateNatureAsync(int id);
    Task<ValeraActionResultDto?> DrinkWineWatchSeriesAsync(int id);
    Task<ValeraActionResultDto?> GoToBarAsync(int id);
    Task<ValeraActionResultDto?> DrinkWithMarginalsAsync(int id);
    Task<ValeraActionResultDto?> SingInMetroAsync(int id);
    Task<ValeraActionResultDto?> SleepAsync(int id);
}
