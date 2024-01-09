using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WindForce : MonoBehaviour
{
    public float initialWindForce = 10f;
    public float windUpdateInterval = 0.5f;
    public Transform ball;
    public Image windArrow;
    public TextMeshProUGUI windText;

    private Rigidbody ballRigidbody;
    private float currentWindForce;
    private float nextWindUpdateTime;
    private Vector3 windDirection;

    void Start()
    {
        ballRigidbody = ball.GetComponent<Rigidbody>();
        currentWindForce = initialWindForce;
        nextWindUpdateTime = Time.time + windUpdateInterval;
    }

    void Update()
    {
        // Periodically update the wind force during the ball's travel
        if (Time.time >= nextWindUpdateTime)
        {
            UpdateWindForce();
            nextWindUpdateTime = Time.time + windUpdateInterval;
        }
    }

    void UpdateWindForce()
    {
        currentWindForce *= 0.9f; 
    }

    public void ApplyWindForce()
    {
        Rigidbody rb = ballRigidbody; 
        if (rb != null)
        {
            Vector3 windDirection = GetComponent<WindForce>().GetCurrentWindDirection();
            float currentWindForce = GetComponent<WindForce>().GetCurrentWindForce();

            // Apply the wind force to the ball
            rb.AddForce(windDirection * currentWindForce, ForceMode.Impulse);
        }
        else
        {
            Debug.LogWarning("Rigidbody not found on the ball. Make sure to attach a Rigidbody component to the ball GameObject.");
        }
    }

    void RotateWindArrow(Vector3 windDirection)
    {
        // Calculate the rotation angle based on the wind direction
        float angle = Mathf.Atan2(windDirection.x, windDirection.z) * Mathf.Rad2Deg;

        // Apply the rotation to the wind arrow
        windArrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void UpdateWindDirection()
    {
        windDirection = Random.onUnitSphere;
        currentWindForce = Random.Range(1000000, 333333333); // Assuming you have min and max wind force values

        // Convert wind force to mph
        float windSpeedMph = (currentWindForce * 0.000621371f) / 3600f;

        // Update the wind arrow rotation
        RotateWindArrow(windDirection);

        // Display wind speed in mph
        if (windText != null)
        {
            windText.text = "Wind Speed: " + windSpeedMph.ToString("F1") + " mph";
        }
    }

    public float GetCurrentWindForce()
    {
        return currentWindForce;
    }

    public Vector3 GetCurrentWindDirection()
    {
        return windDirection;
    }
}
