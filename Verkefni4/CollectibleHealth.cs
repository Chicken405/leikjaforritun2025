using UnityEngine;

public class CollectibleHealth : MonoBehaviour
{
    public int heal;
    public AudioClip collectedClip;

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        // Ef lífin er ekki full
        if (controller != null && controller.health < controller.maxHealth)
        {
            controller.PlaySound(collectedClip);
            controller.ChangeHealth(1);
            Destroy(gameObject);
        }
    }
}
