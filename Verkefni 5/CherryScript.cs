using UnityEngine;

public class CherryScript : MonoBehaviour
{
    public int score = 1;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.GainScore(score);
                Destroy(gameObject);
            }
        }
    }
}
