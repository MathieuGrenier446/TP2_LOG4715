using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TreasureChest : MonoBehaviour
{
    public GameObject closedChest;
    public GameObject openChest;
    public GameObject coins;
    private bool isOpened = false;
    private bool isPlayerNearby = false;
    public LayerMask whatIsPlayer;
    public float detectionRadius = 3.0f;
    public PlayerControler playerControler;
    [SerializeField] TextMeshProUGUI dashUnlockedText;  

    void Update()
    {
        if (!isOpened && Input.GetButtonDown("Interact"))
        {
            checkIsPlayerNearby();
            if (isPlayerNearby) OpenChest();
        }
    }

    private void OpenChest()
    {
        closedChest.SetActive(false);
        openChest.SetActive(true);   
        coins.SetActive(true);        
        isOpened = true;

        playerControler.dashUnlocked = true;
        PlayerStats.Instance.AwardCheckpointExperience();
        DisplayText(dashUnlockedText);
    }

    private void checkIsPlayerNearby()
    {
        isPlayerNearby = Physics.CheckSphere(transform.position, detectionRadius, whatIsPlayer);
    }

    public void DisplayText(TextMeshProUGUI Text)
    {
        Text.gameObject.SetActive(true);   
        StartCoroutine(FloatAndFadeCoroutine(Text));
        Destroy(Text.gameObject);   
    }

    private IEnumerator FloatAndFadeCoroutine(TextMeshProUGUI Text)
    {
        Vector3 startPosition = Text.transform.position;
        Color textColor = Text.color;
        float startAlpha = textColor.a;
        float fadeDuration = 1f;
        float floatSpeed = 1f;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;

            Text.transform.position = startPosition + new Vector3(0, t * floatSpeed, 0); // Move up

            textColor.a = Mathf.Lerp(startAlpha, 0, t);
            Text.color = textColor;

            elapsedTime += Time.deltaTime;
            yield return null; 
        }
    }
}
