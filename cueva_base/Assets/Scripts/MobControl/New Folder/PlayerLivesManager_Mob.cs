using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerLivesManager_Mob : MonoBehaviour
{
    public static PlayerLivesManager_Mob instance;

    [Header("Vidas")]
    [Tooltip("Número inicial de vidas")]
    public int lives = 3;
    [Tooltip("TextMeshProUGUI donde se muestran las vidas")]
    public TextMeshProUGUI livesText;

    [Header("Game Over UI")]
    [Tooltip("Panel que se activa al perder todas las vidas")]
    public GameObject gameOverPanel;

    [Header("Configuración de HUB")]
    [Tooltip("Nombre exacto de la escena Hub")]
    public string hubSceneName = "Hub";

    private void Awake()
    {
                if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateLivesUI();
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

                public void LoseLife()
    {
        lives--;
        UpdateLivesUI();

        if (lives <= 0)
            StartCoroutine(HandleGameOver());
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = $"Vidas: {lives}";
    }

    private IEnumerator HandleGameOver()
    {
                Time.timeScale = 0f;

                if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

                float elapsed = 0f;
        while (elapsed < 5f)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                BreakAndReturn();
                yield break;
            }
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        BreakAndReturn();
    }

    private void BreakAndReturn()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(hubSceneName);
    }
}
