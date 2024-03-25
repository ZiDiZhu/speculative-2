using UnityEngine;

public class SimpleVehicleMovement : MonoBehaviour
{
    public float speed = 5.0f; // Speed of the movement

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); // Get horizontal input (A/D or Left/Right Arrow)
        float moveVertical = Input.GetAxis("Vertical"); // Get vertical input (W/S or Up/Down Arrow)

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical); // Create movement vector

        transform.Translate(movement * speed * Time.deltaTime); // Apply movement
    }
}
