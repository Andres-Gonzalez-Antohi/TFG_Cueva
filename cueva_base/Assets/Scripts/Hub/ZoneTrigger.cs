using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ZoneEvent : UnityEvent<ZoneTrigger.ZoneType> { }

public class ZoneTrigger : MonoBehaviour
{
    public enum ZoneType { Start, Level1, Level2, Level3 }
    [Tooltip("Tipo de zona (inicial o nivel X)")]
    public ZoneType zoneType;

    [Space, Tooltip("Evento al entrar con la cabeza del jugador")]
    public ZoneEvent onPlayerEnter;
    [Tooltip("Evento al salir con la cabeza del jugador")]
    public ZoneEvent onPlayerExit;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            onPlayerEnter.Invoke(zoneType);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            onPlayerExit.Invoke(zoneType);
    }
}
