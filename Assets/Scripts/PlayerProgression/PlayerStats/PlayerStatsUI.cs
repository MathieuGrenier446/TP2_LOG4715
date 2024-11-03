using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    public Slider healthBar;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI experienceText;

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
        healthBar.maxValue = PlayerStats.Instance.GetMaxHealth();
        healthBar.value = PlayerStats.Instance.GetHealth();

        attackText.text = "Attack: " + PlayerStats.Instance.GetAttack();
        healthText.text = $"{PlayerStats.Instance.GetHealth()}/{PlayerStats.Instance.GetMaxHealth()}";
        experienceText.text = "Experience: " + PlayerStats.Instance.GetExperience();
    }
}
