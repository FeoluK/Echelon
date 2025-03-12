using UnityEngine;

public class FloatingTextMovement : MonoBehaviour
{
    public float floatSpeed = 1f;  // Speed at which the text floats
    private Vector3 randomDirection;  // Random direction for floating

    void Start()
    {
        // Generate a random direction for floating
        randomDirection = new Vector3(Random.Range(-1f, 1f), 1, Random.Range(-1f, 1f)).normalized;
    }

    void Update()
    {
        // Move the text in the random direction
        transform.Translate(randomDirection * floatSpeed * Time.deltaTime);

        // Slowly reduce speed to create a slowing effect
        floatSpeed = Mathf.Lerp(floatSpeed, 0, Time.deltaTime * 1.5f);
    }
}
