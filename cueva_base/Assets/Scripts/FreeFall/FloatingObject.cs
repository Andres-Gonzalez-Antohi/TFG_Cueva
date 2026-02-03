using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    private Rigidbody rb;
    private Renderer objRenderer;
    private float riseSpeed;
    private float maxHeight = 2.0f;      private bool isPassed = false;    
        private Obstacle_Generator obstacleGenerator;
    private PlayerLivesManager playerLivesManager;

    private Collider objectCollider;  
        public void Initialize(float riseSpeed, Obstacle_Generator generator, PlayerLivesManager playerLives)
    {
        rb = GetComponent<Rigidbody>();
        objRenderer = GetComponentInChildren<Renderer>();
        this.riseSpeed = riseSpeed;
        this.obstacleGenerator = generator;          this.playerLivesManager = playerLives;  
        objectCollider = GetComponent<Collider>();  
                if (objectCollider == null)
        {
            Debug.LogWarning("Collider no encontrado en " + gameObject.name + ", se agregará uno.");
            objectCollider = gameObject.AddComponent<BoxCollider>();              objectCollider.isTrigger = true;         }
    }

    void Update()
    {
                if (transform.position.y >= maxHeight && !isPassed)
        {
            if (obstacleGenerator != null)
            {
                ScoreManager.instance.AddPoints(1);              }
            isPassed = true;              Destroy(gameObject);         }

                if (transform.position.y >= 0.1f && objectCollider != null && objectCollider.enabled)
        {
            objectCollider.enabled = false;              Debug.Log("Hitbox desactivada");
        }

                if (objRenderer != null)
        {
            float t = Mathf.InverseLerp(5f, 20f, riseSpeed);             Color color = Color.Lerp(new Color(0f, 0.5f, 1f), new Color(0.5f, 0f, 0f), t);             objRenderer.material.color = color;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
                Debug.Log("Colisión detectada con: " + other.gameObject.name);
        Debug.Log("Estado del colisionador: " + (objectCollider != null ? objectCollider.enabled.ToString() : "Collider no encontrado"));

                if (other.CompareTag("Player") && objectCollider != null && objectCollider.enabled)
        {
            Debug.Log("¡Colisión con el jugador!");

                        if (playerLivesManager != null)
            {
                playerLivesManager.LoseLife();
            }

                        if (obstacleGenerator != null)
            {
                ScoreManager.instance.AddPoints(0);             }

                        Destroy(gameObject);
        }
        else
        {
            Debug.Log("No se detectó colisión o la hitbox está desactivada");
        }
    }

}
