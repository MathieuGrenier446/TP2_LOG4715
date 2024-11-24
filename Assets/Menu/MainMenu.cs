using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance { get; private set; }
    public GameObject menuPanel;
    public GameObject upgradesPanel;
    public Button playButton;
    public Button upgradesButton;
    public Button upgradeHealthButton;
    public Button upgradeAttackButton;
    public Button backButton;
    public Button quitButton;

    public Text upgradeInfo;

    public string upgradeFeedback = "";

    public string playerStats;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start() {
        upgradesButton.onClick.AddListener(OpenUpgrades);
        upgradeAttackButton.onClick.AddListener(UpgradeAttack);
        upgradeHealthButton.onClick.AddListener(UpgradeHealth);
        backButton.onClick.AddListener(Back);

        menuPanel.SetActive(true);
        upgradesPanel.SetActive(false);
    }

    void Update()
    {
        playerStats = "Starting Attack: " + PlayerStats.DEFAULT_ATTACK + "\n" + "Starting Health: " + PlayerStats.DEFAULT_HEALTH + "\n" + "Seashells: " + PlayerStats.Instance.GetCurrency();
        upgradeInfo.text = playerStats + upgradeFeedback;
    }

    private void OpenUpgrades() {
        menuPanel.SetActive(false);
        upgradesPanel.SetActive(true);
    }

    private void Back() {
        menuPanel.SetActive(true);
        upgradesPanel.SetActive(false);
    }

    public void UpgradeAttack() {
        if(canPurchase()) {
            PlayerStats.DEFAULT_ATTACK += 2;
            PlayerStats.Instance.CurrencyMod(-5);
            upgradeFeedback = "\nupgrade successful!";
        } else {
            upgradeFeedback = "\nyou don't have enough seashells";
        }
    }

    public void UpgradeHealth() {
        if(canPurchase()) {
            PlayerStats.DEFAULT_HEALTH += 10;
            PlayerStats.Instance.CurrencyMod(-5);
            upgradeFeedback = "\nupgrade successful!";
        } else {
            upgradeFeedback = "\nyou don't have enough seashells";
        }
    }

    public bool canPurchase() {
        if(PlayerStats.Instance.GetCurrency() >= 5){
            return true;
        } else {
            return false;
        }
    }
}


