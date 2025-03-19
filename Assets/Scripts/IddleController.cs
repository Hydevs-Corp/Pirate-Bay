using UnityEngine;

public class IddleController : MonoBehaviour
{
    public float force = 1.0f;
    public float rotationSpeed = 5.0f;
    public bool rotate = false;

    public float timeOffset = 0.0f;


    void Update()
    {
        this.gameObject.transform.position += Vector3.up * (Mathf.Sin(Time.time + timeOffset) * force * Time.deltaTime);
        this.gameObject.transform.position += Vector3.forward * (Mathf.Sin(Time.time + timeOffset) * force * Time.deltaTime);

        if (rotate)
        {
            this.gameObject.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
}
