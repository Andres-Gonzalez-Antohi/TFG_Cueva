using UnityEngine;
using TMPro;


public class ScoreManager_BeatPosition : MonoBehaviour
{
    public static ScoreManager_BeatPosition instance;
    public int score = 0;
    public TextMeshProUGUI scoreText; 
    private int perfectStreak = 0;
    private const int perfectStreakThreshold = 5;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Debug.Log("ScoreManager_BeatPosition instancia asignada");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateScoreUI();
    }

                            public void AddScore(int basePoints, bool perfect)
    {
        if (perfect)
        {
            perfectStreak++;
            if (perfectStreak >= perfectStreakThreshold)
            {
                basePoints *= 2;             }
        }
        else
        {
            perfectStreak = 0;
        }

        score += basePoints;
        UpdateScoreUI();
        Debug.Log("Puntaje actualizado: " + score + " (PerfectStreak: " + perfectStreak + ")");
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Puntos: " + score;
        }
    }
}
