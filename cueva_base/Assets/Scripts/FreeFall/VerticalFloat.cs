using UnityEngine;

public class VerticalFloat : MonoBehaviour
{
    public float amplitude = 0.2f;        public float frequency = 1.0f;    
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * frequency + GetOffset()) * amplitude;
        transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
    }

        float GetOffset()
    {
        return (transform.position.x + transform.position.z) * 0.1f;
    }
}
