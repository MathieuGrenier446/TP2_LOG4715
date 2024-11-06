using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    public Slider healthBar;
    public Slider experienceBar;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI experienceText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI currencyText;

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
        healthBar.maxValue = PlayerStats.Instance.GetMaxHealth();
        healthBar.value = PlayerStats.Instance.GetHealth();

        experienceBar.maxValue = experienceToLevelUp;
        experienceBar.value = PlayerStats.Instance.GetExperience();

        attackText.text = "Attack: " + PlayerStats.Instance.GetAttack();
        healthText.text = $"{PlayerStats.Instance.GetHealth()}/{PlayerStats.Instance.GetMaxHealth()}";
        experienceText.text = $"{PlayerStats.Instance.GetExperience()}/{experienceToLevelUp}";
        levelText.text = "Level: " + PlayerStats.Instance.GetLevel();
        currencyText.text = "Seashell: " + PlayerStats.Instance.GetCurrency();
    }
}
