using UnityEngine;

public class DiamondBullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public int damage = 1;
    public float bulletSpeed = 10f;
    public float lifeTime = 3f;
    private Rigidbody2D rb;

    public void Launch(Vector3 position)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        Vector2 direction = (position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90; // Oddhvassting first
        transform.rotation = Quaternion.Euler(0, 0, angle);
        rb.AddForce(-transform.up * bulletSpeed, ForceMode2D.Impulse);
        Destroy(gameObject, lifeTime);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EagleScript eagleScript = other.GetComponent<EagleScript>();
            if (eagleScript != null)
            {
                eagleScript.DamageHealth(damage);
            }
        }
        Destroy(gameObject);
    }
}
