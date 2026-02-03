using UnityEngine;

public class FollowHeadSelectiveRotation : MonoBehaviour
{
    public Transform head;  // Asigna aquí el objeto Head en el inspector
    public float yOffset = -1f;  // Desplazamiento vertical

    void LateUpdate()
    {
        if (head == null) return;

        // Posición con offset en Y
        Vector3 targetPosition = head.position + new Vector3(0, yOffset, 0);
        transform.position = targetPosition;

        // Obtener la rotación de la cabeza
        Vector3 headEuler = head.rotation.eulerAngles;

        // Solo copiar rotación en Y, mantener X y Z propios
        Vector3 newEuler = new Vector3(transform.rotation.eulerAngles.x, headEuler.y, transform.rotation.eulerAngles.z);
        transform.rotation = Quaternion.Euler(newEuler);
    }
}
