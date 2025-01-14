using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 25.0f;
    public float baseVelocity = 0f;
    private float velocity = 0f;
    private float health = 100.0f;

    public GameObject bulletPrefab;

    private GameObject text;

    void Start()
    {
        velocity = baseVelocity;
        text = GameObject.Find("HealthText");
        if (text)
        {
            text.GetComponent<TMP_Text>().text = "Health: " + health;
        }
        else
        {
            Debug.LogWarning("No health text found.");
        }

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
            GameObject bullet = Instantiate(bulletPrefab, this.transform.position + this.transform.forward + Vector3.up * 3f, rotation);
            bullet.GetComponent<BulletController>().damage = 20.0f;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Quaternion rotation = this.transform.rotation;
            rotation *= Quaternion.Euler(0, -90, 0);
            GameObject bullet = Instantiate(bulletPrefab, this.transform.position + this.transform.forward + Vector3.up * 3f, rotation);
            bullet.GetComponent<BulletController>().damage = 20.0f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootMortar();
        }

    }

    private void ShootMortar()
    {
        GameObject plane = GameObject.Find("Plane");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane planeObj = new Plane(Vector3.up, plane.transform.position);
        if (planeObj.Raycast(ray, out float distance))
        {
            Vector3 target = ray.GetPoint(distance);
            Vector3 direction = target - this.transform.position;
            direction.y = 0;
            Quaternion rotation = Quaternion.LookRotation(direction);
            rotation *= Quaternion.Euler(-5, 0, 0);
            GameObject bullet = Instantiate(bulletPrefab, this.transform.position + this.transform.forward + Vector3.up * 3f, rotation);
            bullet.GetComponent<BulletController>().damage = 50.0f;
        }


    }

    private void GetHit(float damage = 35)
    {
        health -= damage;
        text.GetComponent<TMP_Text>().text = "Health: " + health;
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            print("Hit by enemy bullet");
            GetHit(collision.gameObject.GetComponent<BulletController>().damage);
            Destroy(collision.gameObject);
        }
    }
}
