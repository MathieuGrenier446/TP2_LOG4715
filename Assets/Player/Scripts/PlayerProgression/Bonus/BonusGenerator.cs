using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class BonusGenerator
{
    public const float TOTAL_FACTOR = 0.5f;
    public const float MIN_FACTOR = 0.1f;
    public const float MAX_FACTOR = 0.6f;
    public const float FACTOR_ROUNDER = 0.05f;
    public Bonus GenerateBonus()
    {
        Bonus bonus = new Bonus();
        (string firstStat, string secondStat) = StatsList.PickTwoRandomStats();
        
        PropertyInfo[] properties = typeof(Bonus).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        PropertyInfo firstProperty = properties.FirstOrDefault(property => property.Name == firstStat);
        PropertyInfo secondProperty = properties.FirstOrDefault(property => property.Name == secondStat);

        float firstFactor = UnityEngine.Random.Range(MIN_FACTOR, MAX_FACTOR);

        // Rounding to the nearest 0.05
        float remainder = firstFactor % FACTOR_ROUNDER;
        firstFactor = remainder < FACTOR_ROUNDER / 2 ? firstFactor - remainder : firstFactor + FACTOR_ROUNDER - remainder;
        float secondFactor = TOTAL_FACTOR - firstFactor;

        firstProperty.SetValue(bonus, firstFactor);
        secondProperty.SetValue(bonus, secondFactor);

        return bonus;
    }
}