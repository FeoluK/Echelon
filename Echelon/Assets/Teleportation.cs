using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string destination;

    private void Start()
    {
        DebugLogger.Log("ChangeScene Script Loaded - Waiting for Trigger...");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null || other.gameObject == null)
        {
            DebugLogger.Log("Trigger detected but GameObject is null!");
            return;
        }

        GameObject obj = other.gameObject;
        DebugLogger.Log("Trigger Detected with: " + obj.name);

        // Check if the player entered the portal
        if (obj.CompareTag("Player"))
        {
            DebugLogger.Log("Player entered the portal! Loading scene: " + destination);
            SceneManager.LoadScene(destination);
        }
        else
        {
            DebugLogger.Log("⚠️ Trigger occurred, but NOT the player. Object: " + obj.name);
        }
    }
}
