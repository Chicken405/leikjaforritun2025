using System.Collections;
using UnityEngine;

public class CrankScript : MonoBehaviour
{
    [Header("Crank")]
    public Sprite crankDown;
    public Transform cameraPosition;
    public GameObject objectDestroy;
    public bool flipped = false;

    SpriteRenderer spriteRenderer;

    [Header("Audio Clips")]
    public AudioClip crankSwitchClip;
    public AudioClip destroyClip;
    AudioSource audioSource;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }
    public IEnumerator SwitchLever(float lifeTime)
    {
        if (!flipped)
        {
            flipped = true;
            spriteRenderer.sprite = crankDown;
            audioSource.PlayOneShot(crankSwitchClip);
            yield return new WaitForSeconds(1f);
            StartCoroutine(DestroyObject(gameObject.transform, lifeTime / 2));
        }
    }
    IEnumerator DestroyObject(Transform targetTransform, float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        audioSource.PlayOneShot(destroyClip);
        Destroy(objectDestroy);
    }
}
