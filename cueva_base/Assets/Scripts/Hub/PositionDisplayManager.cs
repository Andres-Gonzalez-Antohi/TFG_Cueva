using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;           using TMPro;                    
public class PositionDisplayManager : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Panel que contiene las entradas (se activa/desactiva con M)")]
    public GameObject positionPanel;

    [Tooltip("Contenedor con VerticalLayoutGroup/ContentSizeFitter")]
    public Transform entriesContainer;

    [Tooltip("Prefab con un TextMeshProUGUI o un Text en su hijo")]
    public GameObject entryPrefab;

    [Header("Objects to Track")]
    [Tooltip("Lista de Transforms cuyas posiciones se mostrarán")]
    public List<Transform> trackedObjects = new List<Transform>();

        private class Entry
    {
        public Transform target;
        public TextMeshProUGUI tmpText;
        public Text uiText;
    }

    private List<Entry> entries = new List<Entry>();

    void Start()
    {
                if (positionPanel != null)
            positionPanel.SetActive(false);

                foreach (Transform t in trackedObjects)
        {
            GameObject go = Instantiate(entryPrefab, entriesContainer);

            var tmp = go.GetComponentInChildren<TextMeshProUGUI>();
            var ui = go.GetComponentInChildren<Text>();

            if (tmp == null && ui == null)
            {
                Debug.LogWarning($"El prefab '{entryPrefab.name}' no contiene ni TextMeshProUGUI ni Text.");
                Destroy(go);
                continue;
            }

                        var e = new Entry { target = t, tmpText = tmp, uiText = ui };
            string line = FormatLine(t);
            if (tmp != null) tmp.text = line;
            else ui.text = line;

            entries.Add(e);
        }
    }

    void Update()
    {
                if (Input.GetKeyDown(KeyCode.M) && positionPanel != null)
            positionPanel.SetActive(!positionPanel.activeSelf);

                if (positionPanel != null && positionPanel.activeSelf)
        {
            foreach (var e in entries)
            {
                string line = FormatLine(e.target);
                if (e.tmpText != null) e.tmpText.text = line;
                else e.uiText.text = line;
            }
        }
    }

        private string FormatLine(Transform t)
    {
        Vector3 p = t.position;
        return $"{t.name}: x={p.x:F2}, y={p.y:F2}, z={p.z:F2}";
    }
}
