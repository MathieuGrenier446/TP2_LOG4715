using Unity.VisualScripting;

public class PlayerStatsFactors
{
    private static PlayerStatsFactors instance;
    private float attackFactor;
    private float healthFactor;
    public PlayerStatsFactors()
    {
        attackFactor = 1.0f;
        healthFactor = 1.0f;
    }

    public static PlayerStatsFactors Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerStatsFactors();
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }

    public static PlayerStatsFactors operator +(PlayerStatsFactors playerStatsFactors, Bonus bonus)
    {
        playerStatsFactors.attackFactor += bonus.attackFactor;
        playerStatsFactors.healthFactor += bonus.healthFactor;

        return playerStatsFactors;
    }

    // Getters might not be used

    public float GetAttackFactor() => attackFactor;
    public float GetHealthFactor() => healthFactor;

    public void ResetFactors()
    {
        attackFactor = 1.0f;
        healthFactor = 1.0f;
    }
}