using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    public GameObject closedChest;
    public GameObject openChest;
    public GameObject coins;
    private bool isOpened = false;
    private bool isPlayerNearby = false;
    public LayerMask whatIsPlayer;
    public float detectionRadius = 3.0f;

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
    }

    private void checkIsPlayerNearby()
    {
        isPlayerNearby = Physics.CheckSphere(transform.position, detectionRadius, whatIsPlayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
