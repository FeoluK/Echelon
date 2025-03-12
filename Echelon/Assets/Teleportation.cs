using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string destination;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null || other.gameObject == null)
        {
            return;
        }

        GameObject obj = other.gameObject;

        // Check if the player entered the portal
        if (obj.CompareTag("Player"))
        {
            SceneManager.LoadScene(destination);
        }
        else
        {
        }
    }
}
