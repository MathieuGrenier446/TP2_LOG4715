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

    public AudioSource menuTheme;

    public AudioSource gameTheme;

    public Text upgradeInfo;

    public string upgradeFeedback = "";

    public string playerStats;

    private bool isGameStart;

    public Camera menuCamera;
    public Camera mainCamera;

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
        SwitchToMenuCamera();
        playButton.onClick.AddListener(StartGame);
        upgradesButton.onClick.AddListener(OpenUpgrades);
        upgradeAttackButton.onClick.AddListener(UpgradeAttack);
        upgradeHealthButton.onClick.AddListener(UpgradeHealth);
        backButton.onClick.AddListener(Back);
        isGameStart = false;
        menuTheme.Play();
        gameTheme.Stop();
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
    public void StartGame()
    {
        menuPanel.SetActive(false);
        upgradesPanel.SetActive(false);
        isGameStart = true;
        SwitchToMainCamera();
        menuTheme.Stop();
        gameTheme.Play();
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

    public void SwitchToMenuCamera()
    {
        if (mainCamera != null)
        {
            mainCamera.GetComponent<AudioListener>().enabled = false;
            AudioListener listener = menuCamera.GetComponent<AudioListener>();
            if (listener != null)
            {
                listener.enabled = true;
            }
        }
        menuCamera.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
    }

    public void SwitchToMainCamera()
    {
        if (menuCamera != null)
        {
            menuCamera.GetComponent<AudioListener>().enabled = false;
            AudioListener listener = mainCamera.GetComponent<AudioListener>();
            if (listener != null)
            {
                listener.enabled = true;
            }
        }
        menuCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
    }

    public bool getIsGameStart() {
        return isGameStart;
    }
}


