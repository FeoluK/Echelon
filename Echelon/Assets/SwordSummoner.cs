using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class SwordSummoner : MonoBehaviour
{
    [Header("Sword Settings")]
    public GameObject swordPrefab;                          // Sword prefab to spawn
    public Transform rightHandTransform;                     // Assign in Inspector (Right XR Controller)
    public Vector3 swordOffset = new Vector3(0f, 0f, 0.5f);  // Offset to adjust sword position
    public Vector3 swordRotation = new Vector3(0f, 0f, 0f);  // Rotation adjustment
    public float swordScale = 1.0f;                          // Scale parameter

    [Header("Particle Settings")]
    public GameObject particlePrefab;                        
    public float particleDuration = 2f;                      

    [Header("Sound Settings")]
    public AudioClip swordSound;                             
    public float soundVolume = 0.7f;                         

    private InputAction buttonAction;                        // Action for B button
    private GameObject spawnedSword;                         // Reference to the spawned sword
    private AudioSource audioSource;                         
    private bool swordSummoned = false;                      // Track if sword is summoned

    void Start()
    {
        // Setup Input Action for B button on Right Controller
        buttonAction = new InputAction("SummonSword", InputActionType.Button, "<XRController>{RightHand}/secondaryButton");
        buttonAction.Enable();
        buttonAction.performed += ctx => ToggleSword();      // Call ToggleSword() when B is pressed

        // 🔥 Create AudioSource if swordSound is assigned
        if (swordSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = swordSound;
            audioSource.volume = soundVolume;
        }

        Debug.Log("SwordSummoner initialized and ready.");
    }

    void ToggleSword()
    {
        Debug.Log("B Button Pressed");                     

        if (swordSummoned)
        {
            Destroy(spawnedSword);
            swordSummoned = false;
            Debug.Log("Sword Destroyed");
        }
        else
        {
            spawnedSword = Instantiate(swordPrefab, rightHandTransform);
            spawnedSword.transform.localPosition = swordOffset;
            spawnedSword.transform.localRotation = Quaternion.Euler(swordRotation);
            spawnedSword.transform.localScale = Vector3.one * swordScale; 
            swordSummoned = true;
            Debug.Log("Sword Spawned with scale: " + swordScale);

            if (swordSound != null && audioSource != null)
            {
                audioSource.Play();
                Debug.Log("Sword sound played.");
            }
            else
            {
                Debug.LogWarning("No sword sound assigned!");
            }

            if (particlePrefab != null)
            {
                GameObject particles = Instantiate(particlePrefab, spawnedSword.transform.position, Quaternion.identity);
                particles.transform.SetParent(spawnedSword.transform);  // Attach to sword
                Destroy(particles, particleDuration);                    // Destroy particles after duration
                Debug.Log("Blue particles spawned around sword.");
            }
            else
            {
                Debug.LogWarning("No particlePrefab assigned!");
            }
        }
    }

    void OnDestroy()
    {
        buttonAction.performed -= ctx => ToggleSword();       // Cleanup event
        buttonAction.Disable();                               // Disable input action
    }
}
