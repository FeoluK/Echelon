using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class ChangeScene : MonoBehaviour
{

    void OnTriggerEnter(Collider other) {
    if (other.CompareTag("Player")) {
        SceneManager.LoadScene("Level E Temp");
        Debug.Log("Detected trigger collision");
        }
    }
}
