using System.Collections;                      using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerLivesManager_BeatPosition : MonoBehaviour
{
    public static PlayerLivesManager_BeatPosition instance;
    public int lives = 5;                             public TextMeshProUGUI livesText;                 public GameObject gameOverPanel;                  public TextMeshProUGUI finalScoreText;        
    [Header("Configuración de HUB")]
    [Tooltip("Nombre de la escena HUB a la que volver tras Game Over")]
    public string hubSceneName = "Hub";           
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
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
            GameOver();
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = "Vidas: " + lives;
    }

    private void GameOver()
    {
        Time.timeScale = 0f;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (finalScoreText != null && ScoreManager_BeatPosition.instance != null)
            finalScoreText.text = "Puntaje Final: " + ScoreManager_BeatPosition.instance.score;

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
