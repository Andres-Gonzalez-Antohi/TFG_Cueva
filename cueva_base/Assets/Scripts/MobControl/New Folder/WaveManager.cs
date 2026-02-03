using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public int snakesPerSpawner = 5;
    public float spawnDelayBetweenSnakes = 1f;
}

public class WaveManager : MonoBehaviour
{
    public EnemySpawner[] spawners;
    public List<Wave> waves;
    public float timeBetweenWaves = 5f;

    private int currentWave = 0;

    private void Start()
    {
        if (spawners.Length == 0 || waves.Count == 0)
        {
            Debug.LogError("WaveManager: Asigne spawners y defina al menos una oleada.");
            return;
        }
        StartCoroutine(RunWaves());
    }

    private IEnumerator RunWaves()
    {
        yield return new WaitForSeconds(2f);
        while (currentWave < waves.Count)
        {
            var w = waves[currentWave];
            for (int i = 0; i < w.snakesPerSpawner; i++)
            {
                foreach (var sp in spawners)
                    sp.SpawnSnake();
                yield return new WaitForSeconds(w.spawnDelayBetweenSnakes);
            }
            currentWave++;
            yield return new WaitForSeconds(timeBetweenWaves);
        }
        while (true)
        {
            foreach (var sp in spawners)
                sp.SpawnSnake();
            yield return new WaitForSeconds(1f);
        }
    }
}