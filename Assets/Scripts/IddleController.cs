using UnityEngine;

public class IddleController : MonoBehaviour
{
    public float force = 1.0f;
    public float rotationSpeed = 50.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position += Vector3.up * (Mathf.Sin(Time.time) * force * Time.deltaTime);
        // move back and forth
        this.gameObject.transform.position += Vector3.forward * (Mathf.Sin(Time.time) * force * Time.deltaTime);
    }
}
