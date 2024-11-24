using UnityEngine;

public class BonusCollectible : MonoBehaviour
{
    public float rotationSpeed = 180.0f;
    private BonusUIManager bonusUIManager;
    private AudioSource audioSource;
    [SerializeField] private AudioClip pickUpSound;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        bonusUIManager = BonusUIManager.Instance;
    }
    private void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.PlayOneShot(pickUpSound);
            // TODO no need to make 2 objects just have singleton
            Bonus bonus1 = new BonusGenerator().GenerateBonus();
            Bonus bonus2 = new BonusGenerator().GenerateBonus();

            if (bonus1 == null || bonus2 == null)
            {
                Debug.LogError("Failed to generate bonuses.");
                return;
            }

            bonusUIManager.ShowBonusOptions(bonus1, bonus2);
            HideVisuals();
            Destroy(gameObject, pickUpSound.length);
        }
    }

    // The item must stay alive while the sound is playing, but we still want to make it invisible when the sound is playing
    private void HideVisuals()
    {
        // Could only get MeshRenderer components and disable it, but this is more modulable.
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;
        }
    }
}