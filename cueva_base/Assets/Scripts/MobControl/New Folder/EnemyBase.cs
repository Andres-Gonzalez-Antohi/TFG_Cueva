using System.Collections;                      using UnityEngine;
using UnityEngine.SceneManagement;             
public class EnemyBase : MonoBehaviour
{
    public int lives = 5;
    public int pointsPerHit = 20;
    private bool isDestroyed = false;

    public void TakeDamage(int dmg)
    {
        if (isDestroyed) return;
        lives -= dmg;
        ScoreManager.instance.AddPoints(pointsPerHit);
        if (lives <= 0)
        {
            isDestroyed = true;
            Debug.Log("¡Base enemiga destruida!");

                        StartCoroutine(ReturnToHubAfterDelay());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Soldier"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }

    private IEnumerator ReturnToHubAfterDelay()
    {
                yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("Hub");
    }
}
