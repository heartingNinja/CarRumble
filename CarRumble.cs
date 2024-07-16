using UnityEngine;
using UnityEngine.InputSystem;

public class CarRumble : MonoBehaviour
{
    private Rigidbody rb;
    private Gamepad gamepad;

    // Acceleration threshold to trigger rumble (adjust as needed)
    [Tooltip("The acceleration need to trigger a rumble")]
    public float rumbleAccelerationThreshold = 15.0f;
    [Tooltip("How much do you want to increase the rumble")]
    public float rumbleAccelerationMultiplier = 1.0f;
    [Tooltip("The max acceleration to normalize the acceleration")]
    public float maxRumbleAcceleration = 50;
    [Tooltip("The right rumble should be more powerful than left")]
    public float increaseRightRumble = .5f;

    private Vector3 prevVelocity;

    // Start is called before the first frame update
    void Start()
    {
        gamepad = Gamepad.current;
        rb = GetComponent<Rigidbody>();

        // Initialize previous velocity
        prevVelocity = rb.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        if (gamepad != null && rb != null)
        {
            // Calculate acceleration
            Vector3 acceleration = (rb.velocity - prevVelocity) / Time.deltaTime;
            // magnitude takes the square root of the (x, y, z) each squared and added
            float magnitude = acceleration.magnitude;

            // Check if acceleration exceeds the threshold to trigger rumble
            if (magnitude > rumbleAccelerationThreshold)
            {
                // Map acceleration magnitude to rumble intensity (0 to 1 range)
                float rumbleIntensity = Mathf.Clamp01(magnitude / maxRumbleAcceleration);
                rumbleIntensity = rumbleIntensity * rumbleAccelerationMultiplier;
                // Trigger rumble based on acceleration
                gamepad.SetMotorSpeeds(rumbleIntensity, Mathf.Clamp01(rumbleIntensity + increaseRightRumble));
            }
            else
            {
                // If acceleration is below the threshold, stop rumble
                gamepad.SetMotorSpeeds(0f, 0f);
            }

            // Update previous velocity for the next frame
            prevVelocity = rb.velocity;
        }
    }

    void OnDestroy()
    {
        if (gamepad != null)
        {
            // Stop rumble when the object is destroyed
            gamepad.SetMotorSpeeds(0f, 0f);
        }
    }

    void OnApplicationQuit()
    {
        if (gamepad != null)
        {
            // Stop rumble when the application quits
            gamepad.SetMotorSpeeds(0f, 0f);
        }
    }
}
