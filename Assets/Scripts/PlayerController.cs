using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 25.0f;
    public float rotationSpeed = 25.0f;
    public float score = 0f;
    private float health = 100.0f;

    public GameObject bulletPrefab;

    private GameObject healthText;
    private GameObject scoreText;

    private float shootInterval = 0.25f;
    private float shootIntervalMortar = 0.9f;
    private float currentShootIntervalLeft = 2.0f;
    private float currentShootIntervalRight = 2.0f;
    private float currentShootIntervalMortar = 2.0f;
    private Rigidbody rb;

    private float vertical;
    private float horizontal;

    void Start()
    {
        healthText = GameObject.Find("HealthText");
        scoreText = GameObject.Find("ScoreText");
        if (healthText)
        {
            healthText.GetComponent<TMP_Text>().text = "Health: " + health;
            scoreText.GetComponent<TMP_Text>().text = "Score: " + score;
        }
        else
        {
            Debug.LogWarning("No health text found.");
        }
        rb = gameObject.GetComponent<Rigidbody>();

    }

    void FixedUpdate()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        Vector3 velocity = transform.forward * vertical * speed * Time.fixedDeltaTime;
        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;
        transform.Rotate(transform.up * horizontal * rotationSpeed * Time.fixedDeltaTime);
    }

    void Update()
    {

        if (currentShootIntervalLeft < shootInterval)
        {
            currentShootIntervalLeft += Time.deltaTime;
        }
        if (currentShootIntervalRight < shootInterval)
        {
            currentShootIntervalRight += Time.deltaTime;
        }
        if (currentShootIntervalMortar < shootIntervalMortar)
        {
            currentShootIntervalMortar += Time.deltaTime;
        }

        // if (Input.GetKey(KeyCode.W))
        // {
        //     velocity += 0.1f;
        //     if (velocity < 0)
        //         velocity += 0.1f;
        // }
        // else
        // {
        //     if (velocity > 0)
        //         velocity -= 0.01f;
        // }
        // if (Input.GetKey(KeyCode.S))
        // {
        //     if (velocity > 0)
        //     {
        //         velocity -= 0.8f;
        //     }
        //     else
        //     {
        //         velocity -= 0.05f;
        //     }
        // }
        // else
        // {
        //     if (velocity < 0)
        //         velocity += 0.01f;
        // }

        // if (velocity > speed)
        // {
        //     velocity = speed;
        // }
        // if (velocity < -speed / 2)
        // {
        //     velocity = -speed / 2;
        // }

        // print(velocity);
        // rb.linearVelocity = velocity * Time.deltaTime * 50.0f * transform.forward;

        // if (Input.GetKey(KeyCode.A))
        // {
        //     gameObject.transform.Rotate(0, -1 * Time.deltaTime * 50.0f, 0);
        // }
        // if (Input.GetKey(KeyCode.D))
        // {
        //     gameObject.transform.Rotate(0, 1 * Time.deltaTime * 50.0f, 0);
        // }

        // if (speed < 25.0f)
        // {
        //     speed += 0.01f;
        // }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentShootIntervalRight < shootInterval)
            {
                return;
            }
            Quaternion rotation = transform.rotation;
            rotation *= Quaternion.Euler(0, 90, 0);
            GameObject bullet = Instantiate(bulletPrefab, transform.position + transform.forward + Vector3.up * 3f, rotation);
            bullet.GetComponent<BulletController>().damage = 80.0f;
            currentShootIntervalRight = 0.0f;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (currentShootIntervalLeft < shootInterval)
            {
                return;
            }
            Quaternion rotation = transform.rotation;
            rotation *= Quaternion.Euler(0, -90, 0);
            GameObject bullet = Instantiate(bulletPrefab, transform.position + transform.forward + Vector3.up * 3f, rotation);
            bullet.GetComponent<BulletController>().damage = 80.0f;
            currentShootIntervalLeft = 0.0f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentShootIntervalMortar < shootIntervalMortar)
            {
                return;
            }
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
            Vector3 direction = target - transform.position;
            direction.y = 0;
            Quaternion rotation = Quaternion.LookRotation(direction);
            rotation *= Quaternion.Euler(-5, 0, 0);
            GameObject bullet = Instantiate(bulletPrefab, transform.position + transform.forward + Vector3.up * 3f, rotation);
            bullet.GetComponent<BulletController>().damage = 30.0f;
            currentShootIntervalMortar = 0.0f;
        }


    }

    private void GetHit(float damage = 35)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
        }
        healthText.GetComponent<TMP_Text>().text = "Health: " + health;
        if (health <= 0)
        {

            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Loot"))
        {
            score++;
            scoreText.GetComponent<TMP_Text>().text = "Score: " + score;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            GetHit();
        }
    }
}
