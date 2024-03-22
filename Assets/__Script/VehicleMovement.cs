using UnityEngine;

public class VehicleMovement : MonoBehaviour
{
    public float accelerationPower = 5f;
    public float steeringPower = 1f;
    public float maxSpeed = 10f;

    private Rigidbody rb;
    private float steeringAmount, speed, direction;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        steeringAmount = Input.GetAxis("Horizontal"); // A/D or Left Arrow/Right Arrow by default
        speed = Input.GetAxis("Vertical") * accelerationPower; // W/S or Up Arrow/Down Arrow by default
        direction = Mathf.Sign(Vector3.Dot(rb.velocity, rb.transform.forward)); // Determines if moving forwards or backwards
    }

    void FixedUpdate()
    {
        rb.AddForce(transform.forward * speed);

        float turn = steeringAmount * steeringPower * rb.velocity.magnitude * direction;
        rb.angularVelocity = Vector3.up * turn;

        CapSpeed();
    }

    void CapSpeed()
    {
        float speed = rb.velocity.magnitude;
        if (speed > maxSpeed)
        {
            float brakeSpeed = speed - maxSpeed;  // calculate the speed decrease

            Vector3 normalisedVelocity = rb.velocity.normalized;
            Vector3 brakeVelocity = normalisedVelocity * brakeSpeed;  // make the brake Vector3 value

            rb.AddForce(-brakeVelocity);  // apply opposing brake force
        }
    }
}
