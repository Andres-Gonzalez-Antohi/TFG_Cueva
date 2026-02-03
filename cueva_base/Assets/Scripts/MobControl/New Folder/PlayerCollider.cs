using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
                if (other.CompareTag("EnemyProjectile") || other.CompareTag("Soldier"))
        {
                        if (PlayerLivesManager_Mob.instance != null)
            {
                PlayerLivesManager_Mob.instance.LoseLife();
            }
            else
            {
                Debug.LogWarning("PlayerLivesManager_Mob.instance es null. Asegúrate de tener un GameObject con PlayerLivesManager_Mob en escena.");
            }

                        Destroy(other.gameObject);
        }
    }
}
