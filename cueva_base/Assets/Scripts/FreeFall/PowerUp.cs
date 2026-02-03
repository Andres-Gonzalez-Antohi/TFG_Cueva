
using System.Collections;
using UnityEngine;

public enum PowerUpEffect
{
    SlowMotion,
    DoublePoints,
    ClearAll
}

public class PowerUp : MonoBehaviour
{
    private Obstacle_Generator obstacleGenerator;
    private Rigidbody rb;
    private PowerUpEffect effectType;

    public void Initialize(Obstacle_Generator generator, PowerUpEffect effect)
    {
        obstacleGenerator = generator;
        effectType = effect;

        rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.velocity = Vector3.up * obstacleGenerator.riseSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyEffect();
            Destroy(gameObject);
        }
    }

    private void ApplyEffect()
    {
        switch (effectType)
        {
            case PowerUpEffect.SlowMotion:
                StartCoroutine(SlowMotionEffect());
                break;
            case PowerUpEffect.DoublePoints:
                StartCoroutine(DoublePointsEffect());
                break;
            case PowerUpEffect.ClearAll:
                DestroyAllObstacles();
                break;
        }
    }

    private IEnumerator SlowMotionEffect()
    {
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        yield return new WaitForSecondsRealtime(5f);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }

    private IEnumerator DoublePointsEffect()
    {
        ScoreManager.instance.SetMultiplier(2);
        yield return new WaitForSeconds(5f);
        ScoreManager.instance.SetMultiplier(1);
    }

    private void DestroyAllObstacles()
    {
        foreach (GameObject obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            Destroy(obstacle);
        }
    }
}
