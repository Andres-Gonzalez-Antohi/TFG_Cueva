using UnityEngine;
using TMPro;  
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;      public int score = 0;      private int scoreMultiplier = 1;      public TextMeshProUGUI scoreText;  
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);          }
    }

    private void Start()
    {
        UpdateScoreUI();
    }

        public void AddPoints(int points)
    {
        score += points * scoreMultiplier;
        UpdateScoreUI();          Debug.Log("Puntos ganados: " + points + " (Multiplicador x" + scoreMultiplier + ")");
    }

        public void SetMultiplier(int multiplier)
    {
        scoreMultiplier = multiplier;
        Debug.Log("Multiplicador de puntos activado: x" + scoreMultiplier);
    }

        private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Puntos: " + score;
        }
    }
}
