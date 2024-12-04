using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossCinematic : MonoBehaviour
{
    public static BossCinematic Instance { get; private set; }

    public CanvasGroup fadeCanvasGroup;
    public float shakeDuration = 10.0f;
    public float shakeIntensity = 0.2f;
    public float fadeOutDuration = 10.0f;

    public Transform playerTransform;

    public Transform bossTransform;

    
    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private BossController bossController;

    [SerializeField]
    private AudioSource mainAudioSource;
    public Vector3 cameraOffset;
    public float cameraMoveDuration = 2.0f;

    public Camera mainCamera;

    private float lockedZPosition;

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

    public void StartCinematic()
    {
        // Disable player controls
        playerController.enabled = false;
        lockedZPosition = playerTransform.position.z;
        bossController.enabled = false;
        StartCoroutine(CinematicSequence());
    }

    private IEnumerator CinematicSequence()
    {
        mainAudioSource.mute = true;
        StartCoroutine(LockPlayerZPosition());
        yield return StartCoroutine(MoveToBossAndShakeCamera(cameraOffset, shakeDuration, shakeIntensity));
        yield return StartCoroutine(MoveToPlayerAndFadeOut(cameraOffset, fadeOutDuration));
        Menu.Instance.Mainmenu();
    }

    private IEnumerator MoveToBossAndShakeCamera(Vector3 offset, float duration, float intensity)
    {
        Vector3 startPosition = bossTransform.position + new Vector3(20, 20, 0);

        float elapsedTime = 0.0f;

        bossTransform.rotation = Quaternion.Euler(0,-90,0);
        bossController.StopMoving();

        while (elapsedTime < duration)
        {
            Vector3 targetPosition = bossTransform.position + offset;

            Vector3 smoothPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / cameraMoveDuration);

            float offsetX = Random.Range(-intensity, intensity);
            float offsetY = Random.Range(-intensity, intensity);
            Vector3 shakeOffset = new Vector3(offsetX, offsetY, 0);


            mainCamera.transform.position = smoothPosition + shakeOffset;

            mainCamera.transform.LookAt(bossTransform.position);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Vector3 finalPosition = bossTransform.position + offset;
        mainCamera.transform.position = finalPosition;

        mainCamera.transform.LookAt(bossTransform.position);

        bossController.HideVisuals();
        bossController.DisableColliders();
    }

    private IEnumerator MoveToPlayerAndFadeOut(Vector3 offset, float duration)
    {
        Vector3 startPosition = playerTransform.position + new Vector3(20, 20, 0);
        Vector3 targetPosition = playerTransform.position + offset;

        fadeCanvasGroup.alpha = 0;
        fadeCanvasGroup.gameObject.SetActive(true);

        float elapsedTime = 0.0f;

        playerTransform.rotation = Quaternion.Euler(0, 90, 0);

        while (elapsedTime < duration)
        {
            Vector3 smoothPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / cameraMoveDuration);
            mainCamera.transform.position = smoothPosition;

            mainCamera.transform.LookAt(playerTransform.position);

            fadeCanvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = targetPosition;
        mainCamera.transform.LookAt(playerTransform.position);
        fadeCanvasGroup.alpha = 1;
    }

    private IEnumerator LockPlayerZPosition()
    {
        while (!playerController.enabled) // Only enforce while the cinematic is active
        {
            Vector3 position = playerTransform.position;
            position.z = lockedZPosition; // Lock the X position
            playerTransform.position = position;

            yield return null;
        }
    }
}

