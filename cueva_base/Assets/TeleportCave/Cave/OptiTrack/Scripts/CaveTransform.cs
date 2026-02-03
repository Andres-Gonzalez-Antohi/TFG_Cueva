using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// Script para teletransportar y girar el Cave en la simulación
public class CaveTransform : MonoBehaviour
{
    public VRTeleporter teleporter;             // Prefab de teletransporte

    private float lastTime;                     // Variable usada para rotar el Cave

    public float rotateTime = 2.0f;            // Tiempo de rotación del Cave

    private bool on_rotate = false;             // Bool para evitar que se solapen las rotaciones

    void Awake()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            teleporter.ToggleDisplay(true);
        }
        
        //Si hacemos click izquierdo con el mando, nos transportamos al lugar que apunta el puntero del mando
        if (Input.GetMouseButtonUp(0))
        {
            teleporter.Teleport();
            teleporter.ToggleDisplay(false);
        }

        // Si clickamos el botón derecho del mando (sin pulsar varias veces seguidas), rotamos el Cave 90 grados a la izquierda
        if (Input.GetMouseButtonUp(1) && !on_rotate)
        {
            on_rotate = true;
            StartCoroutine(Rotar(rotateTime));
        }
    }

    // Corrutina para rotar el Cave con una transición
    IEnumerator Rotar(float time)
    {
        float duration = 0.0f;

        float angulo_inicial = transform.rotation.eulerAngles.y;
        float angulo_final = transform.rotation.eulerAngles.y - 90.0f;
        
        while (duration < time)
        {
            Quaternion q = Quaternion.Euler(transform.rotation.eulerAngles.x, Mathf.Lerp(angulo_inicial, angulo_final, duration),transform.rotation.eulerAngles.z);
            transform.rotation = q;
            duration += Time.deltaTime;
            yield return null;
        }

        on_rotate = false;
    }
}
