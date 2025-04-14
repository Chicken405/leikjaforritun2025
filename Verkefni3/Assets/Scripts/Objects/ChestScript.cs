using Unity.VisualScripting;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    private static GameManager instance;
    private AudioSource sound;
    void Start()
    {
        GameObject gameManager = GameObject.FindWithTag("GameManager");
        if (!gameManager)
        {
            Debug.LogError("No GameManager found! Very bad!!!");
        }
        sound = GetComponent<AudioSource>();

        instance = gameManager.GetComponent<GameManager>();
    }
    void OnTriggerEnter(Collider collision)
    {
        Collider collider = GetComponent<Collider>();
        // Ef key
        if (collision.gameObject.CompareTag("Key"))
        {
            if (instance.key)
            {
                instance.chest = true;
                sound.Play();
                instance.CallWinUIButton();

                Destroy(gameObject);

                // Lásinn hverfur
                this.enabled = false;
                MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
                meshRenderer.enabled = false;
            }
        }
    }
}