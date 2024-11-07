using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public static Menu Instance { get; private set; }
    public GameObject menuPanel;
    public GameObject upgradesPanel;
    public Button resumeButton;
    public Button upgradesButton;
    public Button upgradeHealthButton;
    public Button upgradeAttackButton;
    public Button backButton;
    public Button restartButton;

    public Text upgradeInfo;

    public string upgradeFeedback = "";

    public string playerStats;

    private bool isPaused = false;


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
        resumeButton.onClick.AddListener(Resume);
        upgradesButton.onClick.AddListener(OpenUpgrades);
        upgradeAttackButton.onClick.AddListener(UpgradeAttack);
        upgradeHealthButton.onClick.AddListener(UpgradeHealth);
        restartButton.onClick.AddListener(Restart);
        backButton.onClick.AddListener(Back);

        menuPanel.SetActive(false);
        upgradesPanel.SetActive(false);
    }

    void Update()
    {
        playerStats = "Starting Attack: " + PlayerStats.DEFAULT_ATTACK + "\n" + "Starting Health: " + PlayerStats.DEFAULT_HEALTH + "\n" + "Seashells: " + PlayerStats.Instance.GetCurrency();
        upgradeInfo.text = playerStats + upgradeFeedback;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    private void Resume() {
        ResumeGame();
        menuPanel.SetActive(false);
    }

    private void OpenUpgrades() {
        menuPanel.SetActive(false);
        upgradesPanel.SetActive(true);
    }

    private void Back() {
        menuPanel.SetActive(true);
        upgradesPanel.SetActive(false);
    }

    public void PauseGame()
    {
        menuPanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
        public void ResumeGame()
    {
        menuPanel.SetActive(false);
        upgradesPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
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

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		PlayerStats.Instance = new PlayerStats(PlayerStats.DEFAULT_ATTACK, PlayerStats.DEFAULT_HEALTH);
        Time.timeScale = 1f;
    }
}

