using UnityEngine;

public class ClonedTag : MonoBehaviour { }

public class MultiplierGate : MonoBehaviour
{
    [Header("Multiplicador")]
    [Tooltip("Número total de unidades tras pasar")] public int multiplier = 2;

    [Header("Movimiento (opcional)")]
    public bool isMobile = false;
    public float moveAmplitude = 2f;
    public float moveSpeed = 1f;

    private float startX;

    void Start()
    {
        startX = transform.position.x;
    }

    void Update()
    {
        if (isMobile)
        {
            float offset = Mathf.PingPong(Time.time * moveSpeed, moveAmplitude) - (moveAmplitude * 0.5f);
            Vector3 pos = transform.position;
            pos.x = startX + offset;
            transform.position = pos;
        }
    }

    void OnTriggerEnter(Collider other)
    {
                if (other.GetComponent<ClonedTag>() != null)
            return;

                if (!other.CompareTag("Soldier") && !other.CompareTag("Enemy"))
            return;

                Rigidbody origRb = other.attachedRigidbody;
        Vector3 origVel = origRb ? origRb.velocity : Vector3.zero;
        Vector3 origAng = origRb ? origRb.angularVelocity : Vector3.zero;
        SnakeMover origMover = other.GetComponent<SnakeMover>();

                for (int i = 1; i < multiplier; i++)
        {
            GameObject clone = Instantiate(
                other.gameObject,
                other.transform.position,
                other.transform.rotation
            );

                        clone.AddComponent<ClonedTag>();

                        if (origRb != null && clone.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.velocity = origVel;
                rb.angularVelocity = origAng;
            }

                        if (origMover != null && clone.TryGetComponent<SnakeMover>(out var mover))
            {
                mover.bridgeStart = origMover.bridgeStart;
                mover.bridgeEnd = origMover.bridgeEnd;
                mover.speed = origMover.speed;
                mover.groundLayer = origMover.groundLayer;
                mover.groundRayHeight = origMover.groundRayHeight;
                mover.groundRayDistance = origMover.groundRayDistance;
            }
        }
    }

}
