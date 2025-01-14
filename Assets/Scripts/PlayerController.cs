using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 25.0f;
    public float baseVelocity = 0f;
    private float velocity = 0f;

    public GameObject bulletPrefab;

    void Start()
    {
        velocity = baseVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            velocity += 0.1f;
            if (velocity < 0)
                velocity += 0.1f;
        }
        else
        {
            if (velocity > 0)
                velocity -= 0.01f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (velocity > 0)
            {
                velocity -= 0.05f;
            }
            else
            {
                velocity -= 0.01f;
            }
        }
        else
        {
            if (velocity < 0)
                velocity += 0.01f;
        }

        if (velocity > speed)
        {
            velocity = speed;
        }
        if (velocity < -speed / 2)
        {
            velocity = -speed / 2;
        }

        this.transform.position += this.transform.forward * velocity * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            this.gameObject.transform.Rotate(0, -1 * Time.deltaTime * 50.0f, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.gameObject.transform.Rotate(0, 1 * Time.deltaTime * 50.0f, 0);
        }

        if (speed < 25.0f)
        {
            speed += 0.01f;
        }

        if (speed > 15f)
        { this.gameObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero; }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Quaternion rotation = this.transform.rotation;
            rotation *= Quaternion.Euler(0, 90, 0);
            Instantiate(bulletPrefab, this.transform.position + this.transform.forward + Vector3.up * 3f, rotation);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Quaternion rotation = this.transform.rotation;
            rotation *= Quaternion.Euler(0, -90, 0);
            Instantiate(bulletPrefab, this.transform.position + this.transform.forward + Vector3.up * 3f, rotation);
        }

    }
    void OnCollisionEnter(Collision collision)
    {
    }
}
