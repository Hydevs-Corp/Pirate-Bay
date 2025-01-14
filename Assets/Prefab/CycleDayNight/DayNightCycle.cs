using UnityEngine;


public class SunScript : MonoBehaviour
{
    public Light directionalLight;
    public float dayLength = 120f; // Length of a full day in seconds
    private float time;

    void Update()
    {
        // Increment time
        time += Time.deltaTime / dayLength;
        time %= 1; // Keep time in range [0, 1]

        // Rotate the directional light to simulate the sun's movement
        float sunAngle = time * 360f - 90f;
        directionalLight.transform.localRotation = Quaternion.Euler(sunAngle, 170f, 0f);

        // Adjust the light's intensity based on the time of day
        if (time <= 0.23f || time >= 0.75f)
        {
            directionalLight.intensity = 0;
        }
        else if (time <= 0.25f)
        {
            directionalLight.intensity = Mathf.Lerp(0, 1, (time - 0.23f) * 50);
        }
        else if (time >= 0.73f)
        {
            directionalLight.intensity = Mathf.Lerp(1, 0, (time - 0.73f) * 50);
        }
        else
        {
            directionalLight.intensity = 1;
        }

    }
}

