using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform playerTransform; // Reference to the player's transform
    public Vector3 offset = new Vector3(0f, 2f, -10f); // Offset to position the camera relative to the player

    void LateUpdate()
    {
        if (playerTransform != null)
        {
            // Calculate the desired camera position based on the player's position and offset
            Vector3 desiredPosition = playerTransform.position + offset;

            // Use SmoothDamp to gradually move the camera towards the desired position
            Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);

            // Set the camera's position to the smoothed position
            transform.position = smoothPosition;
        }
    }

    private Vector3 velocity = Vector3.zero; // Velocity used by SmoothDamp
    public float smoothTime = 0.3f; // Smoothing time for camera movement (adjust to your preference)
}
