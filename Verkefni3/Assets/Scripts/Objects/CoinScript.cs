using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 50f;
    private ScoreUIScript scoreScript;
    private AudioSource sound;
    void Start()
    {
        sound = GetComponent<AudioSource>();
        scoreScript = ScoreUIScript.Instance;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            sound.Play();
            scoreScript.AddPoints(1);
            Renderer renderer = GetComponent<Renderer>();
            // Sl�kkva
            renderer.enabled = false;
            this.enabled = false;

            Destroy(gameObject, 2f);
        }
    }

    void LateUpdate()
    {
        // Sn�a
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
