using System;

public class PlayerStats
{
	public event Action OnStatsChanged; // Event to notify the UI

	private static PlayerStats instance;

	private float baseAttack;
	private float currentHealth;
	private float maxHealth;

	private float experience;

	public PlayerStats(float attack, float health, float experience = 0)
	{
		baseAttack = attack;
		maxHealth = health;
		currentHealth = health;
		this.experience = experience;
	}

	public static PlayerStats Instance
    {
        get
        {
            if (instance == null)
            {
				// Declare base values
                instance = new PlayerStats(10, 100);
            }
            return instance;
        }
		set
		{
			instance = value;
		}
    }

	public static PlayerStats operator *(PlayerStats playerStats, PlayerStatsFactors factors)
	{
		float healthDifference = playerStats.maxHealth - playerStats.currentHealth;

		// Apply the factors
		playerStats.baseAttack *= factors.GetAttackFactor();
		playerStats.maxHealth *= factors.GetHealthFactor();
		
		// Round the values
		playerStats.baseAttack = (float)Math.Round(playerStats.baseAttack, 0);
		playerStats.maxHealth = (float)Math.Round(playerStats.maxHealth, 0);
		playerStats.currentHealth = playerStats.maxHealth;

		// Update the current health
		playerStats.currentHealth = playerStats.maxHealth - healthDifference;

		return playerStats;
	}

	public void NotifyUI() => OnStatsChanged?.Invoke();

	public float GetAttack() => baseAttack;
	public float GetHealth() => currentHealth;
	public float GetMaxHealth() => maxHealth;
	public float GetExperience() => experience;

	public void CurentHealthMod(float mod) {
		currentHealth += mod;
		NotifyUI();
	}

}