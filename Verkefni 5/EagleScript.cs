using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class EagleScript : MonoBehaviour
{
    // Animations breytur
    [Header("Animations")]
    public RuntimeAnimatorController patrolController;
    public RuntimeAnimatorController attackController;
    public RuntimeAnimatorController hurtController;
    Animator anim;

    // Attack breytur
    [Header("Attack")]
    public float visionRadius = 5f;
    public float attackForce = 3f;
    public int damage = 1;
    public LayerMask detectionMask;

    [Header("Flying")]
    public GameObject eagleMain;
    public GameObject eaglePost;
    public float moveSpeed = 3f;
    public float flyingRange = 5f;
    public float timer = 0.1f;

    [Header("Health")]
    public int health = 3;
    public float invincibleDuration = 1.5f;
    public float blinkInterval = 0.2f;

    [Header("Death")]
    public float despawnDuration = 1f;
    public float deathSpin = 1f;

    [Header("Audio")]
    public AudioClip flyClip;
    public AudioClip attackClip;
    public AudioClip hurtClip;
    AudioSource audioSource;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // Spila animation
        anim.runtimeAnimatorController = patrolController;
        audioSource.clip = flyClip;
        audioSource.Play();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            DeathAnimation();
        }
    }
    void FixedUpdate()
    {
        // Attack
        Collider2D hit = Physics2D.OverlapCircle(transform.position, visionRadius, detectionMask);
        if (anim.runtimeAnimatorController != hurtController) { // Ef ekki í hurt animation
            if (hit != null && hit.CompareTag("Player"))
            {
                if (anim.runtimeAnimatorController != attackController)
                {
                    anim.runtimeAnimatorController = attackController;
                    Attack(hit);
                    Debug.Log("Player in vision radius!");
                }
            }
            else
            {
                if (anim.runtimeAnimatorController != patrolController)
                {
                    anim.runtimeAnimatorController = patrolController;
                    AudioLoop(flyClip);
                    Debug.Log("Back to patrolling!");
                }
            }

            // Hreyfa tilbaka
            if (anim.runtimeAnimatorController == patrolController)
            {

                Vector2 newPos = Vector2.MoveTowards(rb.position, eaglePost.transform.position, moveSpeed * Time.fixedDeltaTime);
                if (rb.position != newPos)
                {
                    rb.MovePosition(newPos);
                }
                else
                {
                    EaglePost eaglePostComponent = eaglePost.GetComponent<EaglePost>();
                    eaglePostComponent.MoveEaglePost(flyingRange, timer);
                }
            }
        }
    }
    void Attack(Collider2D hit)
    {
        // Flares
        AudioLoop(null);
        audioSource.PlayOneShot(attackClip);

        // Skýtur beint á target
        Vector2 direction = (hit.transform.position - transform.position).normalized;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction * attackForce * 2000f);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.DamageHealth(damage);
            }
        }
    }
    public void DamageHealth(int damage)
    {
        if (anim.runtimeAnimatorController != hurtController)
        {
            anim.runtimeAnimatorController = hurtController;
            // Meiðar eagle-ið
            health -= damage;
            if (health <= 0)
            {
                Destroy(eagleMain, despawnDuration);
                DeathAnimation();
            }
            rb.linearVelocity = Vector2.zero;
            audioSource.PlayOneShot(hurtClip);
            StartCoroutine(HurtBlink());
        }
    }
    void AudioLoop(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
    IEnumerator HurtBlink()
    {
        // Blikkar eagle-ið og setja aftur til patrol mode
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        float elapsed = 0f;
        while (elapsed < invincibleDuration)
        {
            yield return new WaitForSeconds(blinkInterval);
            sprite.enabled = !sprite.enabled;
            elapsed += blinkInterval;
        }
        anim.runtimeAnimatorController = patrolController;
        sprite.enabled = true;
    }
    void DeathAnimation()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = false;
        // Reset
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // Skýtur up
        rb.gravityScale = 1f;
        float angle = Random.Range(-45f, 45f);
        Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.up;

        float forceMagnitude = 1000f; // adjust as needed
        rb.AddForce(direction * forceMagnitude, ForceMode2D.Impulse);
        rb.angularVelocity = 90f * deathSpin;
    }
}
