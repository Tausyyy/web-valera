namespace ValeraApi.Models;

public class Valera
{
    private const int HealthMin = 0, HealthMax = 100;
    private const int AlcoholMin = 0, AlcoholMax = 100;
    private const int CheerfulnessMin = -10, CheerfulnessMax = 10;
    private const int FatigueMin = 0, FatigueMax = 100;


    public int Id { get; set; }

    public string Nickname { get; set;} = "rebeel";

    public string Name { get; set; } = "Валера";

    public int Health { get; private set; } = 100;

    public int Alcohol { get; private set; } = 0;

    public int Cheerfulness { get; private set; } = 0;

    public int Fatigue { get; private set; } = 0;

    public int Money { get; private set; } = 0;

    private static int Clamp(int value, int min, int max) => Math.Max(min, Math.Min(max, value));

    private void ClampAll()
    {
        Health = Clamp(Health, HealthMin, HealthMax);
        Alcohol = Clamp(Alcohol, AlcoholMin, AlcoholMax);
        Cheerfulness = Clamp(Cheerfulness, CheerfulnessMin, CheerfulnessMax);
        Fatigue = Clamp(Fatigue, FatigueMin, FatigueMax);
    }

    public bool GoToWork()
    {
        if (Alcohol >= 50 || Fatigue >= 10)
            return false;

        Cheerfulness -= 5;
        Alcohol -= 30;
        Money += 100;
        Fatigue += 70;
        ClampAll();
        return true;
    }

    public void ContemplateNature()
    {
        Cheerfulness += 1;
        Alcohol -= 10;
        Fatigue += 10;
        ClampAll();
    }

    public void DrinkWineWatchSeries()
    {
        Cheerfulness -= 1;
        Alcohol += 30;
        Fatigue += 10;
        Health -= 5;
        Money -= 20;
        ClampAll();
    }

    public void GoToBar()
    {
        Cheerfulness += 1;
        Alcohol += 60;
        Fatigue += 40;
        Health -= 10;
        Money -= 100;
        ClampAll();
    }

    public void DrinkWithMarginals()
    {
        Cheerfulness += 5;
        Health -= 80;
        Alcohol += 90;
        Fatigue += 80;
        Money -= 150;
        ClampAll();
    }

    public void SingInMetro()
    {
        var bonusEligible = Alcohol > 40 && Alcohol < 70;

        Cheerfulness += 1;
        Alcohol += 10;
        Money += 10 + (bonusEligible ? 50 : 0);
        Fatigue += 20;
        ClampAll();
    }

    public void Sleep()
    {
        if (Alcohol < 30)
            Health += 90;

        if (Alcohol > 70)
            Cheerfulness -= 3;

        Alcohol -= 50;
        Fatigue -= 70;
        ClampAll();
    }
}
