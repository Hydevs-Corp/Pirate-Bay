using UnityEngine;

public class RotatingAndFloating : MonoBehaviour
{
    [Tooltip("Axis of rotation")]
    public Vector3 rotationAxis = Vector3.up;

    [Tooltip("Speed of rotation")]
    public float rotationSpeed = 40f;

    [Header("Optional Floating Effect")]
    [Tooltip("Enable floating motion")]
    public bool enableFloating = true;

    [Tooltip("Floating height")]
    public float floatHeight = 0.5f;

    [Tooltip("Floating speed")]
    public float floatSpeed = 1f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);

        if (enableFloating)
        {
            float newY = startPosition.y + (Mathf.Sin(Time.time * floatSpeed) * floatHeight);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }

    public void ToggleFloating(bool isEnabled)
    {
        enableFloating = isEnabled;
    }
}
