using UnityEngine;

public class GazeFollow : MonoBehaviour
{
    [Tooltip("Transform del head tracker o cámara VR")]
    public Transform headTransform;
    [Tooltip("Distancia en metros delante de la mirada")]
    public float distance = 0.5f;

    void Reset()
    {
        if (headTransform == null && Camera.main != null)
            headTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (headTransform == null) return;
        transform.position = headTransform.position + headTransform.forward * distance;
        transform.rotation = Quaternion.LookRotation(headTransform.forward, Vector3.up);
    }
}
