using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private float projectileLifepsan; // Lífs tíma af byssu skotinn
    [SerializeField] private int projectileDamage; // Damage
    
    [Header("Forces")]
    [SerializeField] private int knockbackForce;
    [SerializeField] private int tossUpForce;

    [Header("Target Tags")]
    [SerializeField] private string targetTag;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        projectileLifepsan -= Time.deltaTime;
        if (projectileLifepsan <= 0) Destroy(gameObject);
    }

    void OnCollisionEnter(Collision hit)
    {
        if (hit.collider.CompareTag(this.tag)) return; // Safety

        if (hit.collider.CompareTag(targetTag))
        {
            // Meiðir óvininn
            var healthComponent = hit.collider.GetComponent<Health>();
            if (healthComponent)
            {
                healthComponent.TakeDamage(projectileDamage);
            }
            // Knockback fyrir óvininn
            Rigidbody targetRigidBody = hit.collider.GetComponent<Rigidbody>();
            if (targetRigidBody)
            {
                // Kasta óvininn upp
                Vector3 upwardsForce = targetRigidBody.transform.up * 100 * tossUpForce;
                targetRigidBody.AddForce(upwardsForce, ForceMode.Impulse);

                // Ýttir óvininn afturback
                Vector3 backwardsForce = -targetRigidBody.transform.forward * 100 * knockbackForce;
                targetRigidBody.AddForce(backwardsForce, ForceMode.Impulse);
            }
            EnemyScript enemyScript = hit.collider.GetComponent<EnemyScript>();
            Destroy(gameObject);
        }
    }
}