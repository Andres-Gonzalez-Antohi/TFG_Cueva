using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class HubManager : MonoBehaviour
{
    [System.Serializable]
    public class ZoneDef
    {
        public ZoneTrigger.ZoneType zoneType;
        public Collider zoneCollider;
        public GameObject highlightQuad;
    }

    [Header("¿Dónde está la cabeza del jugador?")]
    public Transform headTransform;       public Collider headCollider;     
    [Header("Zonas y resaltados")]
    public List<ZoneDef> zones;

    [Header("Mensaje de cuenta atrás")]
    public GameObject confirmDialog;           public TextMeshProUGUI confirmText;    
    [Header("Mapeo de escenas")]
    public string[] levelSceneNames; 
        private Dictionary<ZoneTrigger.ZoneType, bool> inside = new Dictionary<ZoneTrigger.ZoneType, bool>();
    private Coroutine countdownCoroutine;

    void Awake()
    {
                foreach (var z in zones)
        {
            inside[z.zoneType] = false;
            if (z.highlightQuad != null)
                z.highlightQuad.SetActive(false);
        }
        if (confirmDialog != null)
            confirmDialog.SetActive(false);
    }

    void Update()
    {
        CheckZones();
    }

                void CheckZones()
    {
        if (headTransform == null || headCollider == null)
        {
            Debug.LogError("HubManager: headTransform o headCollider no asignados!");
            return;
        }

        foreach (var z in zones)
        {
                        bool nowIn = z.zoneCollider.bounds.Intersects(headCollider.bounds);

            if (nowIn && !inside[z.zoneType])
                EnterZone(z);
            else if (!nowIn && inside[z.zoneType])
                ExitZone(z);

            inside[z.zoneType] = nowIn;
        }
    }

                void EnterZone(ZoneDef z)
    {
        if (z.zoneType == ZoneTrigger.ZoneType.Start)
        {
                        if (z.highlightQuad != null)
                z.highlightQuad.SetActive(false);
            MessagePopup.instance.ShowMessage("Colóquese frente a un nivel", 2f);
        }
        else
        {
                        if (z.highlightQuad != null)
                z.highlightQuad.SetActive(true);
            if (countdownCoroutine != null)
                StopCoroutine(countdownCoroutine);
            countdownCoroutine = StartCoroutine(LevelCountdown(z));
        }
    }

                void ExitZone(ZoneDef z)
    {
        if (z.zoneType == ZoneTrigger.ZoneType.Start)
        {
            if (z.highlightQuad != null)
                z.highlightQuad.SetActive(true);
            MessagePopup.instance.ShowMessage("Por favor, colóquese en la casilla inicial", 3f);
        }
        else
        {
            if (z.highlightQuad != null)
                z.highlightQuad.SetActive(false);
            if (countdownCoroutine != null)
            {
                StopCoroutine(countdownCoroutine);
                countdownCoroutine = null;
            }
            if (confirmDialog != null)
                confirmDialog.SetActive(false);
        }
    }

                IEnumerator LevelCountdown(ZoneDef z)
    {
        if (confirmDialog != null)
            confirmDialog.SetActive(true);

        int idx = (int)z.zoneType - 1;
        int countdown = 10;
        while (countdown >= 0)
        {
            if (confirmText != null)
                confirmText.text = $"Nivel {idx + 1}: mantente en la zona para iniciar en {countdown} ¡Prepárate!";
            yield return new WaitForSeconds(1f);
            countdown--;
        }

                if (confirmDialog != null)
            confirmDialog.SetActive(false);
        countdownCoroutine = null;
        SceneManager.LoadScene(levelSceneNames[idx]);
    }
}
