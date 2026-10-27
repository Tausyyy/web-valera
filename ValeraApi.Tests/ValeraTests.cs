using ValeraApi.Models;
using Xunit;

namespace ValeraApi.Tests;

public class ValeraTests
{
    [Fact]
    public void NewValera_HasExpectedDefaults()
    {
        var valera = new Valera();

        Assert.Equal(100, valera.Health);
        Assert.Equal(0, valera.Alcohol);
        Assert.Equal(0, valera.Cheerfulness);
        Assert.Equal(0, valera.Fatigue);
        Assert.Equal(0, valera.Money);
    }

    [Fact]
    public void GoToWork_Succeeds_WhenSoberAndRested()
    {
        var valera = new Valera();

        var result = valera.GoToWork();

        Assert.True(result);
        Assert.Equal(-5, valera.Cheerfulness);
        Assert.Equal(0, valera.Alcohol);       // 0 - 30, clamped to 0
        Assert.Equal(100, valera.Money);
        Assert.Equal(70, valera.Fatigue);
    }

    [Fact]
    public void GoToWork_Fails_WhenTooDrunk()
    {
        var valera = new Valera();
        valera.GoToBar(); // alcohol -> 60 (>= 50)
        var alcoholBefore = valera.Alcohol;
        var moneyBefore = valera.Money;

        var result = valera.GoToWork();

        Assert.False(result);
        // Состояние не должно измениться, если условие не выполнено
        Assert.Equal(alcoholBefore, valera.Alcohol);
        Assert.Equal(moneyBefore, valera.Money);
    }

    [Fact]
    public void GoToWork_Fails_WhenTooTired()
    {
        var valera = new Valera();
        valera.ContemplateNature(); // fatigue -> 10 (>= 10)
        var fatigueBefore = valera.Fatigue;

        var result = valera.GoToWork();

        Assert.False(result);
        Assert.Equal(fatigueBefore, valera.Fatigue);
    }

    [Fact]
    public void ContemplateNature_AppliesExpectedDeltas()
    {
        var valera = new Valera();

        valera.ContemplateNature();

        Assert.Equal(1, valera.Cheerfulness);
        Assert.Equal(0, valera.Alcohol); // 0 - 10, clamped to 0
        Assert.Equal(10, valera.Fatigue);
    }

    [Fact]
    public void DrinkWineWatchSeries_AppliesExpectedDeltas()
    {
        var valera = new Valera();

        valera.DrinkWineWatchSeries();

        Assert.Equal(-1, valera.Cheerfulness);
        Assert.Equal(30, valera.Alcohol);
        Assert.Equal(10, valera.Fatigue);
        Assert.Equal(95, valera.Health);
        Assert.Equal(-20, valera.Money);
    }

    [Fact]
    public void GoToBar_AppliesExpectedDeltas()
    {
        var valera = new Valera();

        valera.GoToBar();

        Assert.Equal(1, valera.Cheerfulness);
        Assert.Equal(60, valera.Alcohol);
        Assert.Equal(40, valera.Fatigue);
        Assert.Equal(90, valera.Health);
        Assert.Equal(-100, valera.Money);
    }

    [Fact]
    public void DrinkWithMarginals_ClampsHealthAtZero()
    {
        var valera = new Valera();

        valera.DrinkWithMarginals(); // health 100 -> 20
        valera.DrinkWithMarginals(); // health 20 - 80 => -60, clamped to 0

        Assert.Equal(0, valera.Health);
        Assert.Equal(100, valera.Alcohol);      // 90 + 90 = 180, clamped to 100
        Assert.Equal(-300, valera.Money);
        Assert.Equal(100, valera.Fatigue);      // 80 + 80 = 160, clamped to 100
        Assert.Equal(10, valera.Cheerfulness);  // 5 + 5, within [-10,10]
    }

    [Fact]
    public void SingInMetro_NoBonus_WhenAlcoholOutsideRange()
    {
        var valera = new Valera(); // alcohol = 0, вне диапазона (40;70)

        valera.SingInMetro();

        Assert.Equal(10, valera.Money);
        Assert.Equal(10, valera.Alcohol);
        Assert.Equal(20, valera.Fatigue);
        Assert.Equal(1, valera.Cheerfulness);
    }

    [Fact]
    public void SingInMetro_GivesBonus_WhenAlcoholInOpenRange()
    {
        var valera = new Valera();
        valera.DrinkWineWatchSeries(); // alcohol -> 30
        valera.DrinkWineWatchSeries(); // alcohol -> 60, попадает в (40;70)

        var moneyBefore = valera.Money;
        valera.SingInMetro();

        Assert.Equal(moneyBefore + 10 + 50, valera.Money);
        Assert.Equal(70, valera.Alcohol);
    }

    [Fact]
    public void SingInMetro_NoBonus_AtRangeBoundaries()
    {
        // Ровно 40 не считается бонусом (строгое неравенство)
        var valera = new Valera();
        valera.DrinkWithMarginals();      // alcohol -> 90
        valera.Sleep();                   // alcohol -> 40 (90 - 50)

        var moneyBefore = valera.Money;
        valera.SingInMetro();

        Assert.Equal(moneyBefore + 10, valera.Money); // без бонуса, т.к. 40 не > 40
    }

    [Fact]
    public void Sleep_RestoresHealth_WhenAlcoholBelowThirty()
    {
        var valera = new Valera(); // alcohol = 0 < 30

        valera.Sleep();

        Assert.Equal(100, valera.Health); // 100 + 90, clamped to 100
        Assert.Equal(0, valera.Alcohol);  // 0 - 50, clamped to 0
        Assert.Equal(0, valera.Fatigue);  // 0 - 70, clamped to 0
        Assert.Equal(0, valera.Cheerfulness); // условие > 70 не выполнено
    }

    [Fact]
    public void Sleep_ReducesCheerfulness_WhenAlcoholAboveSeventy_AndDoesNotRestoreHealth()
    {
        var valera = new Valera();
        valera.DrinkWithMarginals(); // alcohol -> 90, health -> 20, cheerfulness -> 5, fatigue -> 80

        valera.Sleep();

        Assert.Equal(20, valera.Health);        // алкоголь(90) не < 30 => здоровье не восстанавливается
        Assert.Equal(2, valera.Cheerfulness);   // 5 - 3, т.к. алкоголь(90) > 70
        Assert.Equal(40, valera.Alcohol);       // 90 - 50
        Assert.Equal(10, valera.Fatigue);       // 80 - 70
    }

    [Fact]
    public void Sleep_NeitherEffect_WhenAlcoholBetweenThirtyAndSeventyInclusive()
    {
        var valera = new Valera();
        valera.DrinkWineWatchSeries(); // alcohol -> 30 (не < 30, значит здоровье не растёт)

        var healthBefore = valera.Health;
        var cheerBefore = valera.Cheerfulness;

        valera.Sleep();

        Assert.Equal(healthBefore, valera.Health);   // 30 не < 30 => без восстановления
        Assert.Equal(cheerBefore, valera.Cheerfulness); // 30 не > 70 => без штрафа
        Assert.Equal(0, valera.Alcohol);             // 30 - 50, clamped to 0
    }

    [Fact]
    public void Cheerfulness_NeverExceedsUpperBound()
    {
        var valera = new Valera();

        for (var i = 0; i < 20; i++)
            valera.ContemplateNature(); // +1 каждый раз

        Assert.Equal(10, valera.Cheerfulness);
    }

    [Fact]
    public void Fatigue_NeverExceedsUpperBound()
    {
        var valera = new Valera();

        for (var i = 0; i < 5; i++)
            valera.GoToBar(); // +40 усталости каждый раз

        Assert.Equal(100, valera.Fatigue);
    }
}
