using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Generator : MonoBehaviour
{
    public GameObject[] objectPrefabs;      public GameObject[] powerUpPrefabs; 

    public float spawnRate = 3.0f;
    public float minSpawnRate = 0.5f;
    public float maxSpawnRate = 3.0f;

    public float riseSpeed = 1.0f;
    public float minRiseSpeed = 1.0f;
    public float maxRiseSpeed = 10.0f;

    public Vector2 floorSize = new Vector2(3.67f, 2.7f); 
    private float elapsedTime = 0f;
    private float nextSpawnTime = 0f;

    public PlayerLivesManager playerLivesManager;

    private void Start()
    {
        if (playerLivesManager == null)
        {
            playerLivesManager = FindObjectOfType<PlayerLivesManager>();
            if (playerLivesManager == null)
            {
                Debug.LogError("No se encontró un PlayerLivesManager en la escena.");
            }
        }

        nextSpawnTime = Time.time + spawnRate;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        spawnRate = Mathf.Clamp(maxSpawnRate - elapsedTime / 15f, minSpawnRate, maxSpawnRate);
        riseSpeed = Mathf.Clamp(minRiseSpeed + elapsedTime / 10f, minRiseSpeed, maxRiseSpeed);

        if (Time.time >= nextSpawnTime)
        {
            SpawnFloatingObject();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnFloatingObject()
    {
        if (objectPrefabs.Length == 0)
        {
            Debug.LogWarning("No hay prefabs asignados en la lista de objectPrefabs.");
            return;
        }

                bool spawnPowerUp = UnityEngine.Random.value < 0.1f;

        if (spawnPowerUp && powerUpPrefabs.Length > 0)
        {
            SpawnPowerUp();
        }
        else
        {
            SpawnObstacle();
        }
    }

    void SpawnObstacle()
    {
        GameObject selectedPrefab = objectPrefabs[UnityEngine.Random.Range(0, objectPrefabs.Length)];
        Vector3 spawnPosition = GetSpawnPosition(selectedPrefab);
        GameObject floatingObject = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);

                Collider objectCollider = floatingObject.GetComponent<Collider>();
        if (objectCollider == null)
        {
            objectCollider = floatingObject.AddComponent<BoxCollider>();             objectCollider.isTrigger = true;              Debug.Log($"Collider agregado dinámicamente al prefab {floatingObject.name}");
        }

        Rigidbody rb = floatingObject.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogWarning($"El prefab {floatingObject.name} no tiene Rigidbody en el objeto padre.");
            return;
        }

        rb.isKinematic = false;
        rb.useGravity = false;
        rb.velocity = Vector3.up * riseSpeed;

        FloatingObject floatingObjectScript = floatingObject.AddComponent<FloatingObject>();
        floatingObjectScript.Initialize(riseSpeed, this, playerLivesManager);

        floatingObject.AddComponent<ObstacleScoreTrigger>();
    }



    void SpawnPowerUp()
    {
        int index = UnityEngine.Random.Range(0, powerUpPrefabs.Length);         GameObject selectedPowerUp = powerUpPrefabs[index];
        Vector3 spawnPosition = GetSpawnPosition(selectedPowerUp);

        GameObject powerUpObject = Instantiate(selectedPowerUp, spawnPosition, Quaternion.identity);

                Rigidbody rb = powerUpObject.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = powerUpObject.AddComponent<Rigidbody>();
        }

        rb.useGravity = false;
        rb.isKinematic = false;
        rb.velocity = Vector3.up * riseSpeed;

                PowerUp powerUpScript = powerUpObject.AddComponent<PowerUp>();
        powerUpScript.Initialize(this, (PowerUpEffect)index);     }




    Vector3 GetSpawnPosition(GameObject selectedPrefab)
    {
        Renderer renderer = selectedPrefab.GetComponentInChildren<Renderer>();
        Vector3 objectSize = Vector3.one;

        if (renderer != null)
        {
            objectSize = renderer.bounds.size;
        }
        else
        {
            Debug.LogWarning($"El prefab {selectedPrefab.name} no tiene un Renderer. Se usará tamaño por defecto.");
        }

        float halfWidth = floorSize.x / 2;
        float halfDepth = floorSize.y / 2;

        float minX = -halfWidth + (objectSize.x / 2);
        float maxX = halfWidth - (objectSize.x / 2);
        float minZ = -halfDepth + (objectSize.z / 2);
        float maxZ = halfDepth - (objectSize.z / 2);

        float randomX = UnityEngine.Random.Range(minX, maxX);
        float randomZ = UnityEngine.Random.Range(minZ, maxZ);
        return new Vector3(randomX, transform.position.y, randomZ);
    }
}
