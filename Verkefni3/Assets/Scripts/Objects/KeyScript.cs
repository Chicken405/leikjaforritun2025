using Unity.VisualScripting;
using UnityEngine;
public class KeyScript : MonoBehaviour
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
        instance = gameManager.GetComponent<GameManager>();
        sound = GetComponent<AudioSource>();
    }
    void OnCollisionEnter(Collision collision)
    {
        Collider collider = GetComponent<Collider>();
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject keyHolder = GameObject.FindGameObjectWithTag("KeyHolder");
            if (keyHolder)
            {
                // Key fer í keyholder sem er partur af player
                sound.Play();
                Transform keyHolderTransform = keyHolder.transform;
                this.transform.SetParent(keyHolderTransform);
                this.transform.localPosition = Vector3.zero; // Reset position
                this.transform.localRotation = Quaternion.identity; // Reset rotation

                instance.key = true;
                collider.isTrigger = true;
                this.enabled = false;
            }
        }
    }

}
