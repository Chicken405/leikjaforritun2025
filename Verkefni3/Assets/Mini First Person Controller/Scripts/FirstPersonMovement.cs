using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.LeftShift;

    Rigidbody rb;

    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();



    void Awake()
    {
        // Get the rigidbody on this.
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Update IsRunning from input.
        IsRunning = canRun && Input.GetKey(runningKey);

        // Get targetMovingSpeed.
        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        // Get targetVelocity from input.
        Vector2 targetVelocity = new Vector2(Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);
        if (targetVelocity.magnitude > 0.1f)
        {
            // Convert input to world space
            Vector3 desiredVelocity = transform.rotation * new Vector3(targetVelocity.x, 0f, targetVelocity.y);
            Vector3 currentVelocity = rb.linearVelocity;

            // Keep vertical motion intact, blend horizontal velocity toward desired
            Vector3 blendedVelocity = Vector3.Lerp(
                new Vector3(currentVelocity.x, 0f, currentVelocity.z),
                desiredVelocity,
                Time.fixedDeltaTime * 10f // control responsiveness
            );
            rb.linearVelocity = new Vector3(blendedVelocity.x, currentVelocity.y, blendedVelocity.z);
        }
    }
}