using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;   using System.Collections;

public class PlayerLivesManager : MonoBehaviour
{
    public static PlayerLivesManager instance;
    public int lives = 3;                           public TextMeshProUGUI livesText;              public GameObject gameOverPanel;               public TextMeshProUGUI finalScoreText;     
    [Header("Configuración de HUB")]
    [Tooltip("Nombre de la escena HUB a la que volver tras Game Over")]
    public string hubSceneName = "Hub";         
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Debug.Log("PlayerLivesManager instancia asignada");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateLivesUI();
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void LoseLife()
    {
        Debug.Log("LoseLife llamado");
        lives--;
        UpdateLivesUI();
        Debug.Log("Vidas restantes: " + lives);

        if (lives <= 0)
            GameOver();
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = "Vidas: " + lives;
        else
            Debug.LogError("livesText no asignado en el Inspector.");
    }

    private void GameOver()
    {
                Time.timeScale = 0f;

                if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

                if (finalScoreText != null && ScoreManager.instance != null)
            finalScoreText.text = "Puntaje Final: " + ScoreManager.instance.score;

        Debug.Log("¡Game Over!");

                StartCoroutine(RetornoAlHub());
    }

    private IEnumerator RetornoAlHub()
    {
        float elapsed = 0f;
                while (elapsed < 5f)
        {
                        if (Input.GetKeyDown(KeyCode.H))
            {
                VolverAlHub();
                yield break;
            }
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
                VolverAlHub();
    }

    private void VolverAlHub()
    {
                Time.timeScale = 1f;
        SceneManager.LoadScene(hubSceneName);
    }

        public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
