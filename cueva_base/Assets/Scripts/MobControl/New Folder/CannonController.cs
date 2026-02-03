using System.Collections;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    [Header("References")]
    public Transform playerHeadTransform;
    public Transform enemyBaseTransform;

    [Header("Ground Raycast")]
    public float groundRayHeight = 5f;
    public float groundRayDistance = 10f;
    public LayerMask groundLayer;

    [Header("Firing Settings")]
    public GameObject soldierPrefab;          
    public float fireInterval = 1.0f;
    public float soldierSpeed = 5f;

    private void Start()
    {
        if (playerHeadTransform == null)
        {
            var headObj = GameObject.Find("Cave/OptiTrack/Head");
            if (headObj != null) playerHeadTransform = headObj.transform;
            else Debug.LogError("CannonController: No se encontró 'Cave/OptiTrack/Head'.");
        }

        if (enemyBaseTransform == null)
        {
            var baseObj = GameObject.Find("EnemyBaseZone");
            if (baseObj != null) enemyBaseTransform = baseObj.transform;
            else Debug.LogError("CannonController: No se encontró 'EnemyBaseZone'.");
        }

        StartCoroutine(AutoFire());
    }

    private void Update()
    {
        if (playerHeadTransform != null)
        {
            Vector3 pos = transform.position;
            pos.x = playerHeadTransform.position.x;
            transform.position = pos;
        }
    }

    private IEnumerator AutoFire()
    {
        yield return new WaitForSeconds(0.5f);
        while (true)
        {
            FireSoldier();
            yield return new WaitForSeconds(fireInterval);
        }
    }

    private void FireSoldier()
    {
        if (soldierPrefab == null || enemyBaseTransform == null) return;


        Vector3 spawnPos = transform.position;
        Vector3 rayStart = spawnPos + Vector3.up * groundRayHeight;

        if (Physics.Raycast(rayStart, Vector3.down, out var hit, groundRayDistance, groundLayer))
            spawnPos.y = hit.point.y;

        GameObject soldier = Instantiate(soldierPrefab, spawnPos, Quaternion.identity);


        soldier.tag = "Soldier";

        Vector3 targetPos = new Vector3(enemyBaseTransform.position.x, spawnPos.y, enemyBaseTransform.position.z);
        Vector3 dir = (targetPos - spawnPos);
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.0001f) { Destroy(soldier); return; }
        dir.Normalize();


        var mover = soldier.GetComponent<SoldierMovement>();
        if (mover == null) mover = soldier.AddComponent<SoldierMovement>();
        mover.direction = dir;
        mover.speed = soldierSpeed;


        var controller = soldier.GetComponent<SoldierController>();
        if (controller != null)
        {
            controller.groundLayer = groundLayer;
            controller.groundRayHeight = groundRayHeight;
            controller.groundRayDistance = groundRayDistance;
        }

        if (soldier.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }
}
