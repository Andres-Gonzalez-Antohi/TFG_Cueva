using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SoldierController : MonoBehaviour
{
    public LayerMask groundLayer;
    public float groundRayHeight = 5f;
    public float groundRayDistance = 10f;

    private Rigidbody rb;

    void Awake()
    {
                if (!TryGetComponent<Rigidbody>(out rb))
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

                rb.useGravity = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        StickToGround();
    }

    void StickToGround()
    {
        Vector3 pos = transform.position;
        Vector3 origin = pos + Vector3.up * groundRayHeight;
        if (Physics.Raycast(origin, Vector3.down, out var hit, groundRayDistance, groundLayer))
        {
            pos.y = hit.point.y;
            transform.position = pos;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            Destroy(col.gameObject);
            Destroy(gameObject);
        }
    }
}
