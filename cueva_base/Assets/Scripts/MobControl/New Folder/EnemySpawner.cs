using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Snake Settings")]
    public GameObject snakePrefab;
    public float snakeSpeed = 3f;
    public float spawnInterval = 1f;

    [Header("Ground Detection")]
    public float groundRayHeight = 5f;
    public float groundRayDistance = 10f;
    public LayerMask groundLayer;           
    [Header("Bridge Points")]
    public Transform bridgeStart;               public Transform bridgeEnd;             
    void Start()
    {
                InvokeRepeating(nameof(SpawnSnake), 0f, spawnInterval);
    }

    public void SpawnSnake()
    {
        if (snakePrefab == null || bridgeStart == null || bridgeEnd == null)
            return;

                Vector3 spawnPos = bridgeStart.position;
        RaycastHit hit;
        if (Physics.Raycast(spawnPos + Vector3.up * groundRayHeight,
                            Vector3.down, out hit, groundRayDistance, groundLayer))
        {
            spawnPos.y = hit.point.y;
        }

                Vector3 raw = bridgeEnd.position - bridgeStart.position;
        raw.y = 0;
        Vector3 dir = raw.normalized;

                GameObject snake = Instantiate(
            snakePrefab,
            spawnPos,
            Quaternion.LookRotation(dir, Vector3.up)
        );

                if (snake.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = true;
        }

                var mover = snake.GetComponent<SnakeMover>();
        if (mover != null)
        {
            mover.bridgeStart = bridgeStart;
            mover.bridgeEnd = bridgeEnd;
            mover.speed = snakeSpeed;
            mover.groundLayer = groundLayer;
            mover.groundRayHeight = groundRayHeight;
            mover.groundRayDistance = groundRayDistance;
        }
        else
        {
            Debug.LogWarning("El prefab no tiene SnakeMover.");
        }
    }
}
