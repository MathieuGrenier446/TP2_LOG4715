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
        StartCoroutine(DisplayDashUnlockedText());
    }

    private void checkIsPlayerNearby()
    {
        isPlayerNearby = Physics.CheckSphere(transform.position, detectionRadius, whatIsPlayer);
    }

    private IEnumerator DisplayDashUnlockedText()
    {
        dashUnlockedText.gameObject.SetActive(true);   
        yield return FloatAndFadeCoroutine(); 
        Destroy(dashUnlockedText.gameObject);   
    }

    private IEnumerator FloatAndFadeCoroutine()
    {
        Vector3 startPosition = dashUnlockedText.transform.position;
        Color textColor = dashUnlockedText.color;
        float startAlpha = textColor.a;
        float fadeDuration = 1f;
        float floatSpeed = 1f;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;

            dashUnlockedText.transform.position = startPosition + new Vector3(0, t * floatSpeed, 0); // Move up

            textColor.a = Mathf.Lerp(startAlpha, 0, t);
            dashUnlockedText.color = textColor;

            elapsedTime += Time.deltaTime;
            yield return null; 
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
