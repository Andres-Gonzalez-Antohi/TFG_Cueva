using System.Collections;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public GameObject notePrefab;
    public Transform[] spawnPoints;     public Transform[] targetPoints;     public float beatTempo = 1.0f;
    public GroundManager groundManager;

    private void Start()
    {
        if (notePrefab == null || spawnPoints.Length == 0 || targetPoints.Length == 0 || groundManager == null)
        {
            Debug.LogError("NoteSpawner: Faltan asignaciones en el Inspector.");
            return;
        }
        StartCoroutine(SpawnNotes());
    }

    IEnumerator SpawnNotes()
    {
        while (true)
        {
            SpawnNote();
            yield return new WaitForSeconds(beatTempo);
        }
    }

    void SpawnNote()
    {
        int index = Random.Range(0, spawnPoints.Length);
        if (index >= targetPoints.Length) return;

        GameObject note = Instantiate(notePrefab, spawnPoints[index].position, Quaternion.identity);
        Note noteScript = note.GetComponent<Note>();
        if (noteScript != null)
        {
                                                            SpawnType type;
            int var = 0;
            if (index < 3)
            {
                type = SpawnType.Front;
                var = index;
            }
            else if (index < 5)
            {
                type = SpawnType.Right;
                var = index - 3;
            }
            else
            {
                type = SpawnType.Left;
                var = index - 5;
            }
            noteScript.SetTarget(targetPoints[index].position, groundManager, type, var);
        }
    }
}
