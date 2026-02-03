using UnityEngine;

public class SoldierMovement : MonoBehaviour
{
    [HideInInspector]
    public Vector3 direction;
    [HideInInspector]
    public float speed;

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}