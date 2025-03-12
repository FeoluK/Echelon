using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class VRDebugPanel : MonoBehaviour
{
    public TextMeshProUGUI debugText;
    private Queue<string> logQueue = new Queue<string>();
    private int maxLines = 10; // Max lines to display

    void Awake()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDestroy()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        logQueue.Enqueue(logString);

        if (logQueue.Count > maxLines)
        {
            logQueue.Dequeue();
        }

        debugText.text = string.Join("\n", logQueue.ToArray());
    }
}
