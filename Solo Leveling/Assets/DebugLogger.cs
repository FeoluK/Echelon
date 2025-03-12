using UnityEngine;
using TMPro;

public class DebugLogger : MonoBehaviour
{
    public static DebugLogger instance;
    public TextMeshProUGUI debugText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Log("Debug System Active");  // Confirms it's working
    }

    public static void Log(string message)
    {
        Debug.Log(message);

        if (instance != null && instance.debugText != null)
        {
            instance.debugText.text += message + "\n";  // Display in VR
        }
    }
}
