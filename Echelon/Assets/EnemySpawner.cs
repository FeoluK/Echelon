using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 22f;

    private MeshCollider spawnArea; 
    private float timer = 0f;

    private int totalSpawnedEnemies = 0;
    private int maxEnemies = 20;

    void Start()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy prefab not assigned!");
        }

        // Automatically get the MeshCollider from the attached object (the floor)
        spawnArea = GetComponent<MeshCollider>();

        if (spawnArea == null)
        {
            Debug.LogError("No MeshCollider found on the object! Make sure the floor has a MeshCollider.");
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval && totalSpawnedEnemies < maxEnemies)
        {
            SpawnEnemy();
            timer = 0f;  // Reset timer
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab != null && spawnArea != null)
        {
            // Get a random point within the bounds of the MeshCollider
            Vector3 spawnPoint = GetRandomPointOnMesh();

            // Instantiate enemy at the random spawn point
            Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
            totalSpawnedEnemies++;  // Increment total spawned enemies count

            Debug.Log("Enemy spawned at: " + spawnPoint + ". Total spawned: " + totalSpawnedEnemies);
        }
    }

    Vector3 GetRandomPointOnMesh()
    {
        // Get the bounds of the MeshCollider
        Bounds bounds = spawnArea.bounds;

        // Keep trying to find a valid point on the mesh
        for (int i = 0; i < 10; i++)
        {
            // Generate a random point within the bounds
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float z = Random.Range(bounds.min.z, bounds.max.z);
            float y = bounds.max.y + 1f; // Start above the mesh

            Vector3 randomPoint = new Vector3(x, y, z);

            // Raycast down to find a valid point on the mesh
            if (Physics.Raycast(randomPoint, Vector3.down, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.collider == spawnArea)  // Ensure we hit the mesh itself
                {
                    return hit.point;  // Return the point where the ray hit
                }
            }
        }

        // If no valid point found, return a default point on the floor
        return bounds.center;
    }
}
