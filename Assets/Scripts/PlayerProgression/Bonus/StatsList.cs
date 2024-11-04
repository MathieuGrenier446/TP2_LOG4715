
using System;
using System.Collections.Generic;


// Kind of an enum
public static class StatsList
{
    public const string AttackFactor = "attackFactor";
    public const string HealthFactor = "healthFactor";
    private static readonly List<string> AllStats = new List<string>
    {
        AttackFactor,
        HealthFactor,
    };
    public static (string, string) PickTwoRandomStats()
    {
        Random random = new Random();
        int firstIndex = random.Next(AllStats.Count);
        int secondIndex;
        do
        {
            secondIndex = random.Next(AllStats.Count);
        } while (secondIndex == firstIndex);
        return (AllStats[firstIndex], AllStats[secondIndex]);
    }
}
