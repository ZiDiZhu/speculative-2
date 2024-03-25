using UnityEngine;

public class VehicleMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 180f; // Degrees per second

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Get input for forward/backward movement and turning
        float moveInput = Input.GetAxis("Vertical"); // W/S for forward/backward
        float turnInput = Input.GetAxis("Horizontal"); // A/D for left/right

        // Calculate movement and turning vectors
        Vector3 move = transform.forward * moveInput * moveSpeed * Time.deltaTime;
        float turn = turnInput * turnSpeed * Time.deltaTime;

        // Apply movement and rotation
        rb.MovePosition(rb.position + move);
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, turn, 0f));
    }
}
