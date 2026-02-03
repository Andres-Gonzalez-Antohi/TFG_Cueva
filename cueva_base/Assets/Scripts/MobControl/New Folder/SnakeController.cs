using UnityEngine;

public class SnakeController : MonoBehaviour
{
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public float speed;

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
                if (other.CompareTag("Soldier"))
        {
            Destroy(other.gameObject);             Destroy(gameObject);               }
    }

}
