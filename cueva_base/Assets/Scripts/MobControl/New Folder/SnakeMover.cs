using UnityEngine;

public class SnakeMover : MonoBehaviour
{
    [HideInInspector] public Transform bridgeStart;
    [HideInInspector] public Transform bridgeEnd;

    [Header("Movement")]
    public float speed = 2f;

    [Header("Ground Raycast")]
    public LayerMask groundLayer;
    public float groundRayHeight = 5f;
    public float groundRayDistance = 10f;

    private Vector3 moveDir;

    void Start()
    {
        if (bridgeStart == null || bridgeEnd == null)
        {
            Debug.LogError("BridgeStart/BridgeEnd no asignados en SnakeMover.");
            enabled = false;
            return;
        }

                Vector3 raw = bridgeEnd.position - bridgeStart.position;
        raw.y = 0;
        moveDir = raw.normalized;

                Vector3 p = bridgeStart.position;
        p.y = SampleGroundY(p);
        transform.position = p;

                transform.rotation = Quaternion.LookRotation(moveDir, Vector3.up);

                if (TryGetComponent<Rigidbody>(out var rb))
            rb.isKinematic = true;
    }

    void Update()
    {
                Vector3 next = transform.position + moveDir * speed * Time.deltaTime;

                next.y = SampleGroundY(next);

        transform.position = next;
    }

    float SampleGroundY(Vector3 atXZ)
    {
        RaycastHit hit;
        Vector3 origin = atXZ + Vector3.up * groundRayHeight;
        if (Physics.Raycast(origin, Vector3.down, out hit, groundRayDistance, groundLayer))
            return hit.point.y;

                return transform.position.y;
    }

    void OnCollisionEnter(Collision collision)
    {
                if (collision.gameObject.CompareTag("Soldier"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
                if (bridgeStart != null && bridgeEnd != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(bridgeStart.position, bridgeEnd.position);
        }
    }
}
