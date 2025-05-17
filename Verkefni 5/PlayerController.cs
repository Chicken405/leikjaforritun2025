using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.SceneView;

public class PlayerController : MonoBehaviour
{
    // Breytur
    [Header("Player")]
    public CharacterController2D controller;
    public int health = 5;
    public float runSpeed = 40f;

    [Header("Camera")]
    public GameObject lookCamera;
    public Transform lockCamera;
    public float snapDistance = 5f;

    public GameObject diamondBullet;
    public Animator animator;


    float horizontalMove = 0f;
    bool jump = false;

    void Update()
    {
        Vector3 mousePos = MousePosition(); // Fær mouse position
        CameraMove(mousePos);

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("Jumping", true);
            jump = true;
        }
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            LaunchBullet(mousePos);
        }
    }
    Vector3 MousePosition()
    {
        Vector3 localMousePos = Mouse.current.position.ReadValue();
        localMousePos.z = Camera.main.nearClipPlane;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(localMousePos);
        return mousePos;
    }
    void CameraMove(Vector3 targetPos)
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
    void FixedUpdate()
    {
        // Move our character
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }
    public void DamageHealth(int damage)
    {
        health -= damage;
        if (health > 0)
        {
            
        }
    }
}