using UnityEngine;
using TMPro;  // Import TextMeshPro namespace

public class EnemyFloatingTextSpawner : MonoBehaviour
{
    public GameObject floatingTextPrefab;                 // Prefab for the floating text
    public Vector3 textOffset = new Vector3(0, 2, 0);     // Offset for the text position
    public float floatSpeed = 0.5f;                       // Speed of floating text
    public float destroyAfter = 1.5f;                     // Time to destroy text

    public void SpawnFloatingText(float damage)
    {
        if (floatingTextPrefab != null)
        {
            // Spawn the floating text at the enemy's position + offset
            Vector3 spawnPosition = transform.position + textOffset;
            GameObject floatingText = Instantiate(floatingTextPrefab, spawnPosition, Quaternion.identity);

            // Get TextMeshPro component
            TextMeshPro textMeshPro = floatingText.GetComponent<TextMeshPro>();
            if (textMeshPro != null)
            {
                // Set text to damage amount with 2 decimal places
                textMeshPro.text = damage.ToString("F2");

                // Set text color to red
                textMeshPro.color = Color.red;

                textMeshPro.fontSize *= 0.7f;
            }
            else
            {
                Debug.LogError("TextMeshPro component missing on FloatingText prefab!");
            }

            // Add a floating effect
            FloatingTextMovement textMovement = floatingText.AddComponent<FloatingTextMovement>();
            textMovement.floatSpeed = floatSpeed;

            // Destroy the text after a certain time
            Destroy(floatingText, destroyAfter);
        }
        else
        {
            Debug.LogError("Floating text prefab not assigned!");
        }
    }
}
