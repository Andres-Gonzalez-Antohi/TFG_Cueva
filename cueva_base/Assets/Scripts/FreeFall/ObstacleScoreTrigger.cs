using UnityEngine;

public class ObstacleScoreTrigger : MonoBehaviour
{
    private bool hasScored = false;

    void Update()
    {
                if (transform.position.y > 10 && !hasScored)          {
            ScoreManager.instance.AddPoints(10);              hasScored = true;              Destroy(gameObject, 2f);          }
    }
}
