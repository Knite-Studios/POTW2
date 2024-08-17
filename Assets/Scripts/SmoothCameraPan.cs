using UnityEngine;

public class SmoothCameraPan : MonoBehaviour
{
    [SerializeField] private float panDistance = 5f; // Distance to pan left and right
    [SerializeField] private float panSpeed = 0.5f; // Speed of panning
    [SerializeField] private float smoothness = 5f; // Higher values make movement smoother

    private Vector3 initialPosition;
    private float timeOffset;

    private void Start()
    {
        initialPosition = transform.position;
        // Random offset to start the pan at a random position
        timeOffset = Random.Range(0f, 2f * Mathf.PI);
    }

    private void Update()
    {
        // Calculate the desired x position using a sine wave
        float desiredX = initialPosition.x + Mathf.Sin((Time.time + timeOffset) * panSpeed) * panDistance;

        // Create the target position
        Vector3 targetPosition = new Vector3(desiredX, transform.position.y, transform.position.z);

        // Smoothly interpolate to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothness);
    }

    // Optional: Method to reset the camera position
    public void ResetPosition()
    {
        transform.position = initialPosition;
    }
}