using UnityEngine;
using UnityEngine.Audio;

public class EnemyController : MonoBehaviour
{
    // Public variables
    public float changeTime = 3.0f;
    public float speed = 1.0f;
    public bool vertical;
    public int health = 3;

    // Private variables
    float timer;
    int direction = 1;
    Rigidbody2D rb;
    WinCondition gameManager;
    Animator animator;
    bool broken = true;

    // Particles
    public ParticleSystem smokeEffect;

    // Audio
    public AudioClip fixClip1;
    public AudioClip fixClip2;

    public AudioClip hurtClip1;
    public AudioClip hurtClip2;

    AudioSource audioSource;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        gameManager = Object.FindObjectOfType<WinCondition>();

        timer = changeTime;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!broken)
        {
            return;
        }
        Vector2 position = rb.position;
        if (vertical)
        {
            position.y = position.y + speed * direction * Time.deltaTime;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + speed * direction * Time.deltaTime;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }
        rb.MovePosition(position);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }
    public void Damage()
    {
        health -= 1;
        if (health <= 0 && broken)
        {
            Fix();
        }
        else
        {
            // Hurt clip
            int randomIndex = Random.Range(1, 3);
            if (randomIndex == 1)
            {
                PlaySound(hurtClip1);
            }
            else if(randomIndex == 2)
            {
                PlaySound(hurtClip2);
            }
        }
    }
    public void Fix()
    {
        audioSource.clip = null;
        int randomIndex = Random.Range(1, 3);
        if (randomIndex == 1)
        {
            PlaySound(fixClip1);
        }
        else if (randomIndex == 2)
        {
            PlaySound(fixClip2);
        }
        broken = false;
        rb.simulated = false;
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
        gameManager.EnemyDown();
    }
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
