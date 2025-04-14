using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private float projectileLifepsan; // L�fs t�ma af byssu skotinn
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
            // Mei�ir �vininn
            var healthComponent = hit.collider.GetComponent<Health>();
            if (healthComponent)
            {
                healthComponent.TakeDamage(projectileDamage);
            }
            // Knockback fyrir �vininn
            Rigidbody targetRigidBody = hit.collider.GetComponent<Rigidbody>();
            if (targetRigidBody)
            {
                // Kasta �vininn upp
                Vector3 upwardsForce = targetRigidBody.transform.up * 100 * tossUpForce;
                targetRigidBody.AddForce(upwardsForce, ForceMode.Impulse);

                // �ttir �vininn afturback
                Vector3 backwardsForce = -targetRigidBody.transform.forward * 100 * knockbackForce;
                targetRigidBody.AddForce(backwardsForce, ForceMode.Impulse);
            }
            EnemyScript enemyScript = hit.collider.GetComponent<EnemyScript>();
            Destroy(gameObject);
        }
    }
}