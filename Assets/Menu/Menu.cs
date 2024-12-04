using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public static Menu Instance { get; private set; }
    public GameObject menuPanel;
    public GameObject confirmPanel;
    public Button mainMenuButton;
    public Button confirmButton;
    public Button cancelButton;
    MainMenu mainMenu;
    public bool isPaused = false;


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
        mainMenu = MainMenu.Instance;
        mainMenuButton.onClick.AddListener(Confirm);
        confirmButton.onClick.AddListener(Mainmenu);
        cancelButton.onClick.AddListener(Back);

        menuPanel.SetActive(false);
        confirmPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && mainMenu.getIsGameStart())
        {
            if (!isPaused && Time.timeScale == 0f)
                return;
            else if (isPaused && Time.timeScale == 0F)
                ResumeGame();
            else
                PauseGame();
        }
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
        confirmPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Mainmenu() {
        mainMenu.SwitchToMenuCamera();
        menuPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Confirm() {
        menuPanel.SetActive(false);
        confirmPanel.SetActive(true);
    }

    public void Back() {
        menuPanel.SetActive(true);
        confirmPanel.SetActive(false);
    }
}

