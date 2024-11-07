using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    public Slider healthBar;
    public Slider experienceBar;
    public Text healthText;
    public Text attackText;
    public Text experienceText;
    public Text levelText;
    public Text currencyText;
    public Text ammoText;

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
        float experienceToLevelUp = PlayerStats.Instance.CalculateExperienceToLevelUp();
        healthBar.maxValue = PlayerStats.Instance.GetMaxHealth();
        healthBar.value = PlayerStats.Instance.GetHealth();

        experienceBar.maxValue = experienceToLevelUp;
        experienceBar.value = PlayerStats.Instance.GetExperience();

        attackText.text = "Attack: " + (uint)PlayerStats.Instance.GetAttack();
        healthText.text = $"{(uint)PlayerStats.Instance.GetHealth()}/{(uint)PlayerStats.Instance.GetMaxHealth()}";
        experienceText.text = $"{PlayerStats.Instance.GetExperience()}/{experienceToLevelUp}";
        levelText.text = "Level: " + PlayerStats.Instance.GetLevel();
        currencyText.text = "Seashell: " + PlayerStats.Instance.GetCurrency();
        ammoText.text = "Ice Ball: " + PlayerStats.Instance.ammo;
    }
}
