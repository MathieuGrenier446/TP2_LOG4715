using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BonusUIManager : MonoBehaviour
{
    public static BonusUIManager Instance { get; private set; }
    public GameObject bonusPanel;
    public Button firstBonusButton;
    public Button secondBonusButton;
    private TMP_Text firstBonusText;
    private TMP_Text secondBonusText;
    private Bonus firstBonus;
    private Bonus secondBonus;


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

    private void Start()
    {
        firstBonusText = firstBonusButton.GetComponentInChildren<TMP_Text>();
        secondBonusText = secondBonusButton.GetComponentInChildren<TMP_Text>();

        firstBonusButton.onClick.AddListener(OnFirstBonusSelected);
        secondBonusButton.onClick.AddListener(OnSecondBonusSelected);

        bonusPanel.SetActive(false);
    }

    public void ShowBonusOptions(Bonus bonus1, Bonus bonus2)
    {   
        firstBonus = bonus1;
        secondBonus = bonus2;
        
        var (firstPositiveText, firstNegativeText) = bonus1.GetStatTextTuple();
        var (secondPositiveText, secondNegativeText) = bonus2.GetStatTextTuple();

        firstBonusText.text = $"{firstPositiveText}\n{firstNegativeText}";

        secondBonusText.text = $"{secondPositiveText}\n{secondNegativeText}";

        bonusPanel.SetActive(true);
    }

    public void OnFirstBonusSelected()
    {
        ApplyBonus(firstBonus);
        bonusPanel.SetActive(false);
    }

    public void OnSecondBonusSelected()
    {
        ApplyBonus(secondBonus);
        bonusPanel.SetActive(false);
    }

    private void ApplyBonus(Bonus bonus)
    {
        PlayerStatsFactors.Instance += bonus;
        PlayerStats.Instance *= PlayerStatsFactors.Instance;
        PlayerStatsFactors.Instance.ResetFactors();
        PlayerStats.Instance.NotifyUI();
        Debug.Log($"new player stats: Health = {PlayerStats.Instance.GetHealth()}, Attack = {PlayerStats.Instance.GetAttack()}");
    }
}
