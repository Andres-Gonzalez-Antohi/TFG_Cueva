
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public enum SpawnType { Front, Right, Left }

public class GroundManager : MonoBehaviour
{
    public GameObject[,] groundCells = new GameObject[2, 3]; 
    public Color defaultColor = Color.white;
    public Color highlightColor = Color.yellow;
    public GameObject[] groundCellObjects;


    private float[,] currentMinDistances = new float[2, 3];

    private void Start()
    {
        if (groundCellObjects.Length != 6)
        {
            Debug.LogError("GroundManager: Debes asignar exactamente 6 celdas en el inspector.");
            return;
        }
        int index = 0;
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                groundCells[i, j] = groundCellObjects[index];
                currentMinDistances[i, j] = 100f;
                index++;
            }
        }
    }

    public void ReportDistance(Vector2Int pos, float distance)
    {
        if (distance < currentMinDistances[pos.x, pos.y])
        {
            currentMinDistances[pos.x, pos.y] = distance;
        }
    }

    private void Update()
    {

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                GameObject cell = groundCells[i, j];
                if (cell != null)
                {
                    float distance = currentMinDistances[i, j];
                    float intensity = Mathf.Clamp01(1 - (distance / 10f));
                    Renderer renderer = cell.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material.color = Color.Lerp(defaultColor, highlightColor, intensity);
                    }
                    currentMinDistances[i, j] = 100f;
                }
            }
        }
    }
}

