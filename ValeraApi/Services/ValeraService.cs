using Microsoft.EntityFrameworkCore;
using ValeraApi.Data;
using ValeraApi.DTOs;
using ValeraApi.Models;

namespace ValeraApi.Services;

public class ValeraService : IValeraService
{
    private readonly AppDbContext _db;

    public ValeraService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<ValeraDto>> GetAllAsync()
    {
        var items = await _db.Valeras.AsNoTracking().OrderBy(v => v.Id).ToListAsync();
        return items.Select(ToDto).ToList();
    }

    public async Task<ValeraDto?> GetByIdAsync(int id)
    {
        var valera = await _db.Valeras.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
        return valera is null ? null : ToDto(valera);
    }

    public async Task<ValeraDto> CreateAsync(CreateValeraDto dto)
    {
        var valera = new Valera();
        if (!string.IsNullOrWhiteSpace(dto.Name))
            valera.Name = dto.Name.Trim();

        if (!string.IsNullOrWhiteSpace(dto.Nickname))
            valera.Nickname = dto.Nickname.Trim();

        _db.Valeras.Add(valera);
        await _db.SaveChangesAsync();
        return ToDto(valera);
    }
    public async Task<ValeraDto?> UpdateAsync(int id, UpdateValeraDto dto)
{
    var valera = await _db.Valeras.FindAsync(id);
    if (valera is null)
        return null;

    valera.Name = dto.Name.Trim();
    valera.Nickname = dto.Nickname?.Trim() ?? "";

    await _db.SaveChangesAsync();
    return ToDto(valera);
}

    public async Task<bool> DeleteAsync(int id)
    {
        var valera = await _db.Valeras.FindAsync(id);
        if (valera is null)
            return false;

        _db.Valeras.Remove(valera);
        await _db.SaveChangesAsync();
        return true;
    }

    public Task<ValeraActionResultDto?> GoToWorkAsync(int id) =>
        PerformAsync(id, "Валера сходил на работу.",
            "Валера не может пойти на работу: нужно алкоголь < 50 и усталость < 10.",
            v => v.GoToWork());

    public Task<ValeraActionResultDto?> ContemplateNatureAsync(int id) =>
        PerformAsync(id, "Валера созерцает природу.", null, v => { v.ContemplateNature(); return true; });

    public Task<ValeraActionResultDto?> DrinkWineWatchSeriesAsync(int id) =>
        PerformAsync(id, "Валера пьёт вино и смотрит сериал.", null, v => { v.DrinkWineWatchSeries(); return true; });

    public Task<ValeraActionResultDto?> GoToBarAsync(int id) =>
        PerformAsync(id, "Валера сходил в бар.", null, v => { v.GoToBar(); return true; });

    public Task<ValeraActionResultDto?> DrinkWithMarginalsAsync(int id) =>
        PerformAsync(id, "Валера выпил с маргинальными личностями.", null, v => { v.DrinkWithMarginals(); return true; });

    public Task<ValeraActionResultDto?> SingInMetroAsync(int id) =>
        PerformAsync(id, "Валера спел в метро.", null, v => { v.SingInMetro(); return true; });

    public Task<ValeraActionResultDto?> SleepAsync(int id) =>
        PerformAsync(id, "Валера поспал.", null, v => { v.Sleep(); return true; });

    private async Task<ValeraActionResultDto?> PerformAsync(
        int id, string successMessage, string? failMessage, Func<Valera, bool> action)
    {
        var valera = await _db.Valeras.FindAsync(id);
        if (valera is null)
            return null;

        var success = action(valera);
        if (success)
            await _db.SaveChangesAsync();

        return new ValeraActionResultDto(
            success,
            success ? successMessage : failMessage ?? "Действие не выполнено.",
            ToDto(valera));
    }

    private static ValeraDto ToDto(Valera v) =>
        new(v.Id, v.Name, v.Health, v.Alcohol, v.Cheerfulness, v.Fatigue, v.Money, v.Nickname);
}
