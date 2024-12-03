using UnityEngine;

public class SoundEmitter : MonoBehaviour
{
    protected AudioSource audioSource;

    protected void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }
    protected void PlaySoundAndDestroy(AudioClip audioClip, bool hideVisuals = true, bool disableColliders = true)
    {
        PlaySound(audioClip);
        if(hideVisuals)
        {
            HideVisuals();
        }
        if(disableColliders)
        {
            DisableColliders();
        }
        Destroy(gameObject, audioClip.length);
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

    private void DisableColliders()
    {
        // Disable all colliders (prevent further interactions)
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
    }
}