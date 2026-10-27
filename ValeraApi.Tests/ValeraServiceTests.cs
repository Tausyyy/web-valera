using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ValeraApi.Data;
using ValeraApi.DTOs;
using ValeraApi.Services;
using Xunit;

namespace ValeraApi.Tests;

public class ValeraServiceTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly AppDbContext _db;
    private readonly ValeraService _service;

    public ValeraServiceTests()
    {
        // In-memory SQLite живёт, пока открыто соединение
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(_connection).Options;
        _db = new AppDbContext(options);
        _db.Database.EnsureCreated();
        _service = new ValeraService(_db);
    }

    public void Dispose()
    {
        _db.Dispose();
        _connection.Dispose();
    }

    [Fact]
    public async Task Create_PersistsValeraWithDefaults()
    {
        var created = await _service.CreateAsync(new CreateValeraDto { Name = "Тест" });

        var loaded = await _service.GetByIdAsync(created.Id);

        Assert.NotNull(loaded);
        Assert.Equal("Тест", loaded!.Name);
        Assert.Equal(100, loaded.Health);
        Assert.Equal(0, loaded.Money);
    }

    [Fact]
    public async Task Work_PersistsNewState()
    {
        var created = await _service.CreateAsync(new CreateValeraDto());

        var result = await _service.GoToWorkAsync(created.Id);
        _db.ChangeTracker.Clear(); // читаем заново из БД
        var loaded = await _service.GetByIdAsync(created.Id);

        Assert.True(result!.Success);
        Assert.Equal(100, loaded!.Money);
        Assert.Equal(70, loaded.Fatigue);
    }

    [Fact]
    public async Task Work_Rejected_WhenTired_DoesNotChangeState()
    {
        var created = await _service.CreateAsync(new CreateValeraDto());
        await _service.ContemplateNatureAsync(created.Id); // усталость 10

        var result = await _service.GoToWorkAsync(created.Id);

        Assert.False(result!.Success);
        Assert.Equal(0, result.Valera.Money);
    }

    [Fact]
    public async Task Action_ReturnsNull_WhenValeraNotFound()
    {
        Assert.Null(await _service.SleepAsync(999));
    }

    [Fact]
    public async Task Delete_RemovesValera()
    {
        var created = await _service.CreateAsync(new CreateValeraDto());

        Assert.True(await _service.DeleteAsync(created.Id));
        Assert.False(await _service.DeleteAsync(created.Id));
    }
}
