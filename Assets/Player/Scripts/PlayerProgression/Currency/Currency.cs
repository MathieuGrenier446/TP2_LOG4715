using UnityEngine;

public class Currency : SoundEmitter
{
    public float rotationSpeed = 180.0f;
    [SerializeField] private AudioClip pickUpSound;

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            PlaySoundAndDestroy(pickUpSound);
    }
}