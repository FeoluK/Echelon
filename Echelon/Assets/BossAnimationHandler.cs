using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class BossAnimationHandler : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private Transform playerTransform;

    public float detectionRange = 6f;
    public float attackRange = 3f;
    private bool isAttacking = false;
    private bool isDead = false;
    private float attackCooldown = 3f;
    private float attackTimer = 0f;

    // Boss Health
    private float bossHealth = 100f;
    private float minDamage = 18f;
    private float maxDamage = 22f;

    [Header("Boss Voice Lines")] 
    public AudioClip[] attackVoices; // 9 different attack sounds
    public AudioClip deathVoice;     // 1 unique death sound
    private AudioSource audioSource;
    private float voiceCooldown = 6f; // Prevents overlapping
    private float lastVoiceTime = -6f; // Tracks last sound play time

    private void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (animator == null) Debug.LogError("Animator component not found on Boss!");
        if (navMeshAgent == null) Debug.LogError("NavMeshAgent component not found on Boss!");

        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform not found! Make sure the player is tagged as 'Player'.");
        }

        // 🔥 Create an AudioSource in code (no need to attach one manually)
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // Ensure it's 3D sound
    }

    private void Update()
    {
        if (isDead) return; // Stop logic if the boss is dead

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (!isAttacking)
        {
            if (distance <= attackRange)
            {
                StartAttack();
            }
            else if (distance <= detectionRange)
            {
                PlayRunAnimation();
            }
        }
    }

    private void StartAttack()
    {
        if (isDead) return;

        isAttacking = true;
        int randomAttack = Random.Range(1, 6);

        switch (randomAttack)
        {
            case 1:
                animator.SetTrigger("Trigger_Attack1");
                Debug.Log("Boss performed Attack 1");
                break;
            case 2:
                animator.SetTrigger("Trigger_Attack2");
                Debug.Log("Boss performed Attack 2");
                break;
            case 3:
                animator.SetTrigger("Trigger_Attack3");
                Debug.Log("Boss performed Attack 3");
                break;
            case 4:
                animator.SetTrigger("Trigger_Attack4");
                Debug.Log("Boss performed Attack 4");
                break;
            case 5:
                animator.SetTrigger("Trigger_Attack5");
                Debug.Log("Boss performed Attack 5");
                break;
        }

        // 🔥 Play random attack voice (only if cooldown passed & boss is alive)
        if (!isDead && attackVoices.Length > 0 && Time.time >= lastVoiceTime + voiceCooldown)
        {
            AudioClip randomVoice = attackVoices[Random.Range(0, attackVoices.Length)];
            audioSource.PlayOneShot(randomVoice, 4f); // 🔥 Play sound 4x louder
            lastVoiceTime = Time.time; // Update cooldown timer
            Debug.Log("Boss attack voice played: " + randomVoice.name);
        }

        Invoke(nameof(EndAttack), attackCooldown);
    }

    private void EndAttack()
    {
        isAttacking = false;
        animator.SetBool("IsRunning", true);
    }

    public void PlayRunAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("IsRunning", true);
            Debug.Log("Boss Running");
        }
    }

    // 💀 Boss takes damage and dies when health reaches 0
    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return; // Ignore damage if already dead

        if (other.CompareTag("Sword")) // Ensure sword is tagged properly
        {
            float damage = Random.Range(minDamage, maxDamage);
            bossHealth -= damage;
            Debug.Log("Boss Hit! Took " + damage + " damage. Remaining HP: " + bossHealth);

            if (bossHealth <= 0)
            {
                PlayDieAnimation();
            }
        }
    }

    private void PlayDieAnimation()
    {
        isDead = true;
        animator.SetTrigger("Trigger_Die");
        Debug.Log("Boss Died!");

        audioSource.Stop(); 

        if (deathVoice != null)
        {
            audioSource.PlayOneShot(deathVoice, 4f);
            Debug.Log("Boss Death Voice Played: " + deathVoice.name);
        }

        if (navMeshAgent != null)
            navMeshAgent.enabled = false;

        StartCoroutine(DespawnAfterDelay(5f));
    }

    private IEnumerator DespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Boss removed from scene.");
        Destroy(gameObject);
    }
}
