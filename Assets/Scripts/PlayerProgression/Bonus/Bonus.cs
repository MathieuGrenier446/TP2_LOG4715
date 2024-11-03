using System.Collections.Generic;

public class Bonus
{
    public float attackFactor { get; set; }
    public float healthFactor { get; set; }

    public Bonus()
    {
        attackFactor = 0.0f;
        healthFactor = 0.0f;
    }

    public Bonus(float attackFactor, float healthFactor)
    {
        this.attackFactor = attackFactor;
        this.healthFactor = healthFactor;
    }

    public (string firstText, string secondText) GetStatTextTuple()
    {
        string firstText = "";
        string secondText = "";

        if (attackFactor != 0.0f) AddStatText(ref firstText, ref secondText, "Attack", attackFactor);
        if (healthFactor != 0.0f) AddStatText(ref firstText, ref secondText, "Health", healthFactor);


        return (firstText.Trim(), secondText.Trim());
    }

    private void AddStatText(ref string firstText, ref string secondText, string statName, float factor)
    {
        if (factor > 0.0f)
        {
            firstText += $"{statName}: +{factor * 100}% ";
        }
        else if (factor < 0.0f)
        {
            secondText += $"{statName}: {factor * 100}% ";
        }
    }
}	