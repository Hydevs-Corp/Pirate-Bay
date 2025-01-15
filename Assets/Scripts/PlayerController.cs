using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public float speed = 1.0f;
    public float maxSpeed = 50.0f;
    public float acceleration = 0.0f;
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
    private Vector2 lastRightStickDirection = Vector2.zero;
    private float lastTimeCollided = 0.0f;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
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
    }

    void FixedUpdate()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        if (vertical > 0.2)
        {
            vertical = 1;
        }
        if (vertical < -0.2)
        {
            vertical = -1;
        }
        if (vertical != 0)
        {
            acceleration += vertical * Time.fixedDeltaTime;
        }
        else
        {
            if (acceleration < 0.1 && acceleration > -0.1)
            {
                acceleration = 0;
            }
            acceleration = Mathf.Lerp(acceleration, 0, Time.fixedDeltaTime * 2);
        }

        float rotationFactor = Mathf.Abs(horizontal);
        acceleration *= 1 - rotationFactor * 0.01f;

        acceleration = Mathf.Clamp(acceleration, -maxSpeed, maxSpeed);
        Vector3 velocity = acceleration * speed * Time.fixedDeltaTime * transform.forward;
        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;

        transform.Rotate(horizontal * rotationSpeed * Time.fixedDeltaTime * transform.up);
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

        if (Gamepad.current != null)
        {
            if (Gamepad.current.rightStick.ReadValue().magnitude > 0.1)
            {
                lastRightStickDirection = Gamepad.current.rightStick.ReadValue();
            }
            if (Gamepad.current.rightTrigger.wasPressedThisFrame) Shoot(90);
            if (Gamepad.current.leftTrigger.wasPressedThisFrame) Shoot(-90);
            if (Gamepad.current.rightShoulder.wasPressedThisFrame) ShootMortar("gamepad");
        }

        if (Input.GetKeyDown(KeyCode.E)) Shoot(90);

        if (Input.GetKeyDown(KeyCode.Q)) Shoot(-90);


        if (Input.GetKeyDown(KeyCode.Space)) ShootMortar();

    }

    private void Shoot(float direction)
    {
        if (currentShootIntervalLeft < shootInterval)
        {
            return;
        }
        Quaternion rotation = transform.rotation;
        rotation *= Quaternion.Euler(0, direction, 0);
        GameObject bullet = Instantiate(bulletPrefab, transform.position + transform.forward + Vector3.up * 3f, rotation);
        bullet.GetComponent<BulletController>().damage = 80.0f;
        currentShootIntervalLeft = 0.0f;
    }

    private void ShootMortar(String input = "mouse")
    {
        if (currentShootIntervalMortar < shootIntervalMortar)
        {
            return;
        }
        GameObject plane = GameObject.Find("Plane");
        Ray ray;
        if (input == "mouse")
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane planeObj = new(Vector3.up, plane.transform.position);
            if (planeObj.Raycast(ray, out float distance))
            {
                Vector3 target = ray.GetPoint(distance);
                Vector3 direction = target - transform.position;
                direction.y = 0;
                Quaternion rotation = Quaternion.LookRotation(direction);
                rotation *= Quaternion.Euler(-5, 0, 0);
                GameObject bullet = Instantiate(bulletPrefab, transform.position + transform.forward + Vector3.up * 3f, rotation);
                bullet.GetComponent<BulletController>().damage = 35.0f;
                currentShootIntervalMortar = 0.0f;
            }
        }
        else
        {
            Vector3 rightStickDirection = new(lastRightStickDirection.x, 0, lastRightStickDirection.y);
            Quaternion rotation = Quaternion.LookRotation(rightStickDirection);
            rotation *= Quaternion.Euler(-5, gameObject.transform.rotation.y, 0);
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
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Loot"))
        {
            score++;
            scoreText.GetComponent<TMP_Text>().text = "Score: " + score;
            health += 15;
            if (health > 100)
            {
                health = 100;
            }
            healthText.GetComponent<TMP_Text>().text = "Health: " + health;
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            GetHit(collision.gameObject.GetComponent<BulletController>().damage);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Bullet")) { }
        else
        {
            if (Time.time - lastTimeCollided < 3.0f)
            {
                return;
            }
            GetHit(5);
            acceleration /= 10;
            lastTimeCollided = Time.time;
        }
    }
}
