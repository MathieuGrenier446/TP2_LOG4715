using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    public GameObject uiContainer;
    public Slider healthBar;
    public Slider experienceBar;
    public Text healthText;
    public Text attackText;
    public Text experienceText;
    public Text levelText;
    public Text currencyText;
    public Text ammoText;

    MainMenu mainMenu;

    private void Start()
    {
        mainMenu = MainMenu.Instance;
        PlayerStats.Instance.OnStatsChanged += UpdateUI;
        HideUI();
        UpdateUI();
    }

    private void Update()
    {
        if(mainMenu.getIsGameStart())
        {
            ShowUI();
        }
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

    private void HideUI()
    {
        // Iterate over all UI elements in the container and disable their renderers
        foreach (var renderer in uiContainer.GetComponentsInChildren<CanvasRenderer>())
        {
            renderer.SetAlpha(0); // Set the alpha to 0 (invisible)
        }
    }

    private void ShowUI()
    {
        // Iterate over all UI elements in the container and enable their renderers
        foreach (var renderer in uiContainer.GetComponentsInChildren<CanvasRenderer>())
        {
            renderer.SetAlpha(1); // Set the alpha back to 1 (visible)
        }
    }
}
