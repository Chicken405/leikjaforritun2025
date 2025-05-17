using System.Collections;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Breytur
    [Header("Player")]
    public CharacterController2D controller;
    public UIManager playerUI;
    public int health = 5;
    public int score = 0;

    public float runSpeed = 40f;

    [Header("Camera")]
    public CinemachineCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;
    public GameObject lookCamera;
    public Transform lockCamera;
    public float snapDistance = 5f;

    [Header("Cinematics")]
    public float shakeTime = 1.5f;
    public float shakeFreq = 3f;


    [Header("Interact")]
    public float interactRadius = 0.1f;
    public LayerMask interactableLayer;

    [Header("Audio Clips")]
    public AudioClip walkingClip;
    public AudioClip shootClip;
    public AudioClip hurtClip;
    public AudioClip collectClip;


    AudioSource audioSource;

    [Header("Cutscene")]
    float cutsceneTime = 3f;
    bool isCutscene = false;

    [Header("Misc")]
    public string defeatScene = "Lose";
    public GameObject diamondBullet;
    public Animator animator;

    float horizontalMove = 0f;
    bool jump = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        virtualCameraNoise = virtualCamera.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();

        // UI
        playerUI.SetHealthText($"Health: {health}");
        playerUI.SetScoreText($"Score: {score}");
    }
    void Update()
    {
        if (!isCutscene) // Ef ekki í cutscene
        {

            Vector3 mousePos = MousePosition(); // Fær mouse position
            LeashedCamera(mousePos);

            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            if (horizontalMove != 0f)
            {
                if (audioSource.clip != walkingClip) { AudioLoop(walkingClip); }
            }
            else
            {
                if (audioSource.clip == walkingClip) { AudioLoop(null); }
            }
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
            // Hoppa
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jump = true;
                animator.SetBool("Jumping", true);
            }
            // Skjóta
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                audioSource.PlayOneShot(shootClip);
                LaunchBullet(mousePos);
            }
            // Interacta
            if (Input.GetKeyDown(KeyCode.X))
            {
                Collider2D hit = Physics2D.OverlapCircle(transform.position, interactRadius, interactableLayer);
                if (hit != null) {
                    CrankScript crank = hit.GetComponent<CrankScript>();
                    if (crank != null)
                    {
                        StartCoroutine(crank.SwitchLever(cutsceneTime));
                        StartCoroutine(DoCutscene(crank.cameraPosition, cutsceneTime));
                        StartCoroutine(ShakeCamera(shakeTime, cutsceneTime / 2));
                    }
                    HouseScript house = hit.GetComponent<HouseScript>();
                    if (house != null)
                    {
                        house.Victory();
                    }
                }
            }
        }
    }
    void FixedUpdate()
    {
        if (!isCutscene) // Ef ekki cutscene
        {
            // Move our character
            controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
            jump = false;
        }
    }
    Vector3 MousePosition()
    {
        Vector3 localMousePos = Mouse.current.position.ReadValue();
        localMousePos.z = Camera.main.nearClipPlane;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(localMousePos);
        return mousePos;
    }
    void LeashedCamera(Vector3 targetPos)
    {
        Vector2 lockPos2D = new Vector2(lockCamera.position.x, lockCamera.position.y);
        Vector2 targetPos2D = new Vector2(targetPos.x, targetPos.y);
        float distance = Vector2.Distance(lockPos2D, targetPos2D);

        Vector3 newTargetPosition;
        if (distance > snapDistance)
        {
            Vector3 direction = (targetPos - lockCamera.position).normalized;
            newTargetPosition = lockCamera.position + direction * snapDistance;
        }
        else
        {
            newTargetPosition = lockCamera.position;
        }
        lookCamera.transform.position = newTargetPosition;
    }
    IEnumerator DoCutscene(Transform targetTransform, float hold)
    {
        yield return new WaitForSeconds(1f);
        isCutscene = true;
        lookCamera.transform.position = targetTransform.position;
        yield return new WaitForSeconds(hold);
        isCutscene = false;
    }
    public IEnumerator ShakeCamera(float shakeTime, float hold)
    {
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(hold);
        float halfTime = shakeTime / 2f;
        
        // Hristar meira og meira
        for (float t = 0; t < halfTime; t += Time.deltaTime)
        {
            float normalizedTime = t / halfTime;
            virtualCameraNoise.FrequencyGain = Mathf.Lerp(0f, shakeFreq, normalizedTime);
            yield return null;
        }

        // Hristar minna og minna
        for (float t = 0; t < halfTime; t += Time.deltaTime)
        {
            float normalizedTime = t / halfTime;
            virtualCameraNoise.FrequencyGain = Mathf.Lerp(shakeFreq, 0f, normalizedTime);
            yield return null;
        }
        virtualCameraNoise.FrequencyGain = 0f;
    }
    void AudioLoop(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
    public void OnLanding()
    {
        animator.SetBool("Jumping", false);
    }
    public void LaunchBullet(Vector3 mousePos)
    {
        // Spawnar skýtur
        GameObject bullet = Instantiate(diamondBullet, transform.position, Quaternion.identity);
        DiamondBullet bulletScript = bullet.GetComponent<DiamondBullet>();
        bulletScript.Launch(mousePos);
    }
    public void DamageHealth(int damage)
    {
        health -= damage;
        audioSource.PlayOneShot(hurtClip);
        playerUI.SetHealthText($"Health: {health}");
        // Dauður fer til dauða sena
        if (health <= 0)
        {
            Defeat();
        }
    }
    public void Defeat()
    {
        SceneManager.LoadScene(defeatScene);
    }

    public void GainScore(int amount)
    {
        audioSource.PlayOneShot(collectClip);
        score += amount;
        playerUI.SetScoreText($"Score: {score}");
    }
}