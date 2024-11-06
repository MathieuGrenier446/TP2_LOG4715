using UnityEngine;

public class BonusCollectible : MonoBehaviour
{
    public float rotationSpeed = 180.0f;
    private BonusUIManager bonusUIManager;
    private void Start()
    {
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
            // TODO no need to make 2 objects just have singleton
            Bonus bonus1 = new BonusGenerator().GenerateBonus();
            Bonus bonus2 = new BonusGenerator().GenerateBonus();

            if (bonus1 == null || bonus2 == null)
            {
                Debug.LogError("Failed to generate bonuses.");
                return;
            }

            bonusUIManager.ShowBonusOptions(bonus1, bonus2);
            Destroy(gameObject);
        }
    }
}