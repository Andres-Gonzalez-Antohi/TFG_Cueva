using UnityEngine;
using TMPro;
using System.Collections;

public class MessagePopup : MonoBehaviour
{
    public static MessagePopup instance;
    [Tooltip("Texto central para notificaciones")]
    public TextMeshProUGUI messageText;
    [Tooltip("Duración de la transición (fade) en segundos")]
    public float fadeDuration = 0.5f;

    private CanvasGroup canvasGroup;
    private Coroutine currentCoroutine;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        canvasGroup = messageText.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = messageText.gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
    }

                public void ShowMessage(string message, float duration)
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(ShowAndHide(message, duration));
    }

    private IEnumerator ShowAndHide(string message, float duration)
    {
        messageText.text = message;
                for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            canvasGroup.alpha = t / fadeDuration;
            yield return null;
        }
        canvasGroup.alpha = 1f;
                yield return new WaitForSeconds(duration);
                for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            canvasGroup.alpha = 1 - t / fadeDuration;
            yield return null;
        }
        canvasGroup.alpha = 0f;
        currentCoroutine = null;
    }
}
