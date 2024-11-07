using System;
using UnityEngine.SceneManagement;

public class PlayerStats
{
	public event Action OnStatsChanged; // Event to notify the UI

	private static PlayerStats instance;

	public static float DEFAULT_ATTACK = 10;
	public static float DEFAULT_HEALTH = 100;

	private const uint BOSS_KILL_EXPERIENCE = 100;
	private const uint CHECKPOINT_EXPERIENCE = 50;
	private const uint ENEMY_KILL_EXPERIENCE = 10;

	private const uint BASE_LEVEL_UP_EXPERIENCE = 50;

	private const float EXPONENTIAL_FACTOR = 1.3f;

	private const float LEVEL_UP_ATTACK_MULTIPLIER = 1.1f;
	private const float LEVEL_UP_HEALTH_MULTIPLIER = 1.2f;

	private uint level = 1;

	private int currency;

	private float baseAttack;
	private float currentHealth;
	private float maxHealth;

	public int ammo = 0;

	private float experience;

	public PlayerStats(float attack, float health, float experience = 0, int currency = 0, int ammo = 0)
	{
		baseAttack = attack;
		maxHealth = health;
		currentHealth = health;
		this.experience = experience;
		this.currency = currency;
		this.ammo = ammo;
	}

	public static PlayerStats Instance
    {
        get
        {
            if (instance == null)
            {
				// Declare base values
                instance = new PlayerStats(DEFAULT_ATTACK, DEFAULT_HEALTH);
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

	public void AwardBossKillExperience()
	{
		AwardExperience(BOSS_KILL_EXPERIENCE);
	}

	public void AwardCheckpointExperience()
	{
		AwardExperience(CHECKPOINT_EXPERIENCE);
	}

	public void AwardEnemyKillExperience()
	{
		AwardExperience(ENEMY_KILL_EXPERIENCE);
	}

	private void AwardExperience(float experience)
	{
		this.experience += experience;
		CheckForLevelUp();
		NotifyUI();
	}

	private void CheckForLevelUp()
    {
        float experienceToLevelUp = CalculateExperienceToLevelUp();
        while (experience >= experienceToLevelUp)
        {
            experience -= experienceToLevelUp;
            LevelUp();
            experienceToLevelUp = CalculateExperienceToLevelUp();
        }
    }

    public float CalculateExperienceToLevelUp()
    {
        return BASE_LEVEL_UP_EXPERIENCE * (float)Math.Pow(level, EXPONENTIAL_FACTOR);
    }

    private void LevelUp()
    {
        level++;
        baseAttack *= LEVEL_UP_ATTACK_MULTIPLIER;
        maxHealth *= LEVEL_UP_HEALTH_MULTIPLIER;
        currentHealth = maxHealth;

        NotifyUI();
        Console.WriteLine($"Leveled up to level {level}! New stats: Attack = {baseAttack}, Max Health = {maxHealth}");
    }

	public void NotifyUI() => OnStatsChanged?.Invoke();

	public float GetAttack() => baseAttack;
	public float GetHealth() => currentHealth;
	public float GetMaxHealth() => maxHealth;
	public float GetExperience() => experience;
	public uint GetLevel() => level;

	public int GetCurrency() => currency;

	public void CurrentHealthMod(float mod) {
		currentHealth += mod;
		if (currentHealth > maxHealth) {
			currentHealth = maxHealth;
		}
		if(currentHealth <= 0) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			Instance = new PlayerStats(DEFAULT_ATTACK, DEFAULT_HEALTH);
		}
		NotifyUI();
	}

	public void CurrencyMod(int mod) {
		currency += mod;
		NotifyUI();
	}

}