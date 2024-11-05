using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
public class PlayerUI : MonoBehaviour
{
    public Slider healthBar;
    public Slider experienceBar;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI experienceText;
    public TextMeshProUGUI levelText;

    private void Start()
    {
        PlayerStats.Instance.OnStatsChanged += UpdateUI;
        UpdateUI();
    }

    private void OnDestroy()
    {
        PlayerStats.Instance.OnStatsChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        float experienceToLevelUp = (uint)PlayerStats.Instance.CalculateExperienceToLevelUp();
        float maxHealth = PlayerStats.Instance.GetMaxHealth();
        float currentHealth = PlayerStats.Instance.GetHealth();

        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        experienceBar.maxValue = experienceToLevelUp;
        experienceBar.value = PlayerStats.Instance.GetExperience();

        attackText.text = "Attack: " + (uint)Math.Round(PlayerStats.Instance.GetAttack());
        healthText.text = $"{(uint)Math.Round(currentHealth)}/{(uint)Math.Round(maxHealth)}";
        experienceText.text = $"{PlayerStats.Instance.GetExperience()}/{experienceToLevelUp}";
        levelText.text = "Level: " + PlayerStats.Instance.GetLevel();
    }
}
