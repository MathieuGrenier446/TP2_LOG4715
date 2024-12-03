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
    public Vector3 cameraOffset;
    public float cameraMoveDuration = 2.0f;

    public Camera mainCamera;

    [SerializeField]
    private PlayerController playerController;

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

        StartCoroutine(CinematicSequence());
    }

    private IEnumerator CinematicSequence()
    {
        StartCoroutine(MoveAndShakeCamera(cameraOffset, shakeDuration, shakeIntensity));
        yield return StartCoroutine(FadeOut(fadeOutDuration));
        Menu.Instance.Mainmenu();
    }

    private IEnumerator MoveAndShakeCamera(Vector3 offset, float duration, float intensity)
    {
        Vector3 startPosition = playerTransform.position + new Vector3(20, 20, 0);


        float elapsedTime = 0.0f;

        playerTransform.rotation = Quaternion.Euler(0,90,0);

        while (elapsedTime < duration)
        {
            Vector3 targetPosition = playerTransform.position + offset;

            Vector3 smoothPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / cameraMoveDuration);

            float offsetX = Random.Range(-intensity, intensity);
            float offsetY = Random.Range(-intensity, intensity);
            Vector3 shakeOffset = new Vector3(offsetX, offsetY, 0);

   
            mainCamera.transform.position = smoothPosition + shakeOffset;

            mainCamera.transform.LookAt(playerTransform.position);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Vector3 finalPosition = playerTransform.position + offset;
        mainCamera.transform.position = finalPosition;

        mainCamera.transform.LookAt(playerTransform.position);
    }



    private IEnumerator FadeOut(float duration)
    {
        fadeCanvasGroup.alpha = 0;
        fadeCanvasGroup.gameObject.SetActive(true);

        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            fadeCanvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeCanvasGroup.alpha = 1;
    }
}

