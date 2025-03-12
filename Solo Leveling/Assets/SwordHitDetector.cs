using UnityEngine;

public class SwordHitDetector : MonoBehaviour
{
    public string enemyTag = "Enemy";  

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(enemyTag))
        {
            Debug.Log("Hit enemy: " + other.name);

            // Generate random damage between 20 and 22 with 2 decimal places
            float damage = Mathf.Round(Random.Range(20f, 22f) * 100f) / 100f;

            // Call the enemy's function to spawn floating text
            EnemyFloatingTextSpawner textSpawner = other.GetComponent<EnemyFloatingTextSpawner>();
            if (textSpawner != null)
            {
                textSpawner.SpawnFloatingText(damage);
            }
        }
    }
}
