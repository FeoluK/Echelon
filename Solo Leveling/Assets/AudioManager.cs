using UnityEngine;
using UnityEngine.SceneManagement;  // Needed to access scene information

public class AudioManager : MonoBehaviour
{
    public AudioClip backgroundMusic;
    public string targetScene1 = "PlayerMechanicsTesting";
    public string targetScene2 = "DemoScene";

    private AudioSource audioSource;         // Reference to AudioSource component
    private static AudioManager instance;    // Singleton instance

    void Awake()
    {
        // Check if an instance already exists
        if (instance != null && instance != this)
        {
            Destroy(gameObject);  // Destroy duplicate AudioManager
            return;
        }

        // Set up singleton instance
        instance = this;
        DontDestroyOnLoad(gameObject);  // Persist between scenes

        // Set up AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;          // Loop the background music
        audioSource.playOnAwake = false;  // Prevent play on Awake
        audioSource.volume = 0.35f;        // Set volume (adjust if needed)
        SceneManager.sceneLoaded += OnSceneLoaded;  // Subscribe to scene loaded event
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;  // Unsubscribe from event
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if we are in the target scene
        if (scene.name == targetScene1 || scene.name == targetScene2)
        {
            PlayMusic();  // Start playing music if we enter the target scene
        }
        else
        {
            StopMusic();  // Stop music if we leave the target scene
        }
    }

    public void PlayMusic()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();  // Play music if not already playing
        }
    }

    public void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();  // Stop music if needed
        }
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = Mathf.Clamp(volume, 0f, 1f);  // Adjust volume (0-1 range)
    }
}
