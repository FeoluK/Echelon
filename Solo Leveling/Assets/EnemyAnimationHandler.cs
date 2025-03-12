using UnityEngine;
using System.Collections;

public class EnemyAnimationHandler : MonoBehaviour
{
    private Animator animator;
    private Transform playerTransform;
    private PlayerHealthHandler playerHealth;

    private const string RUN_ANIMATION = "Armature|Orc_Run";
    private const string ATTACK_ANIMATION = "Armature|Orc_Punch";
    private const string DIE_ANIMATION = "Armature|Death_Bow";

    private const float detectionRange = 1f;
    private const float attackRange = 1f;
    private bool isDead = false;

    // Enemy Health
    private float enemyHealth = 100f;
    private float minDamage = 18f;
    private float maxDamage = 22f;

    // Attack Cooldown
    private bool canDealDamage = true;
    private float attackCooldown = 1f;
    private float attackTimer = 0f;

    [Header("Attack Sounds")] 
    public AudioClip[] attackSounds; // Array for attack sounds (drag in Inspector)
    private AudioSource audioSource;
    private float attackSoundCooldown = 2.5f; // Cooldown for playing sounds
    private float lastAttackSoundTime = -2.5f; // Track last sound play time

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator component not found on enemy!");
        }

        // Find the player by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerHealth = player.GetComponent<PlayerHealthHandler>();

            if (playerHealth == null)
            {
                Debug.LogError("PlayerHealthHandler component not found on player!");
            }
        }
        else
        {
            Debug.LogError("No GameObject found with the tag 'Player'!");
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 4f;
    }

    private void Update()
    {
        if (isDead || playerTransform == null) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance <= attackRange)
        {
            PlayAttackAnimation();
        }
        else if (distance <= detectionRange)
        {
            PlayRunAnimation();
        }

        if (!canDealDamage)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackCooldown)
            {
                canDealDamage = true;
                attackTimer = 0f;
            }
        }
    }

    public void PlayRunAnimation()
    {
        if (animator != null)
        {
            animator.Play(RUN_ANIMATION);
        }
    }

    public void PlayAttackAnimation()
    {
        if (animator != null)
        {
            animator.Play(ATTACK_ANIMATION);

            // Deal damage only if cooldown allows
            if (playerHealth != null && canDealDamage)
            {
                float damage = Random.Range(8f, 12f);
                playerHealth.TakeDamage(damage);
                Debug.Log("Enemy Attacked! Player took " + damage + " damage.");

                canDealDamage = false;
            }

            // 🔥 Play a random attack sound (only if cooldown passed)
            if (attackSounds.Length > 0 && Time.time >= lastAttackSoundTime + attackSoundCooldown)
            {
                AudioClip randomClip = attackSounds[Random.Range(0, attackSounds.Length)];
                audioSource.PlayOneShot(randomClip);
                lastAttackSoundTime = Time.time; // Update cooldown timer
                Debug.Log("Enemy attack sound played: " + randomClip.name);
            }
        }
    }

    public void PlayDieAnimation()
    {
        if (animator != null)
        {
            animator.Play(DIE_ANIMATION);
            Debug.Log("Die Animation Triggered");
            isDead = true;

            StartCoroutine(DespawnAfterDelay(1.5f));
        }
    }

    private IEnumerator DespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword") && !isDead)
        {
            float damage = Random.Range(minDamage, maxDamage);
            enemyHealth -= damage;
            Debug.Log("Enemy Hit! Took " + damage + " damage. Remaining HP: " + enemyHealth);

            if (enemyHealth <= 0)
            {
                PlayDieAnimation();
            }
        }
    }
}
