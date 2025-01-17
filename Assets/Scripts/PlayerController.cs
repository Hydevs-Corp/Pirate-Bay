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
    public int score = 0;

    public GameObject bulletPrefab;

    private GameObject scoreText;

    private float shootInterval = 0.25f;
    private float shootIntervalMortar = 0.35f;
    private float currentShootIntervalLeft = 2.0f;
    private float currentShootIntervalRight = 2.0f;
    private float currentShootIntervalMortar = 2.0f;
    private Rigidbody rb;

    private float vertical;
    private float horizontal;
    private Vector2 lastRightStickDirection = Vector2.zero;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        scoreText = GameObject.Find("ScoreText");
        scoreText.GetComponent<TMP_Text>().text = "" + score;
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
            vertical = -0.5f;
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

        acceleration = Mathf.Clamp(acceleration, -maxSpeed / 2, maxSpeed);
        Vector3 velocity = acceleration * speed * Time.fixedDeltaTime * transform.forward;
        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;

        float rotationModifier = acceleration;
        if (Mathf.Abs(rotationModifier) < 0.8f)
        {
            rotationModifier = 2f;
        }

        transform.Rotate(horizontal * rotationSpeed * rotationModifier * Time.fixedDeltaTime * transform.up);
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
            if (Gamepad.current.leftTrigger.wasPressedThisFrame)
            {
                if (currentShootIntervalLeft < shootInterval)
                {
                    return;
                }
                Shoot(90);
                Shoot(-90);
                currentShootIntervalLeft = 0.0f;
            }
            if (Gamepad.current.rightTrigger.wasPressedThisFrame) ShootMortar("gamepad");
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {

            if (currentShootIntervalLeft < shootInterval)
            {
                return;
            }
            Shoot(90);
            Shoot(-90);
            currentShootIntervalLeft = 0.0f;
        }


        if (Input.GetKey(KeyCode.Mouse0)) ShootMortar();

        if (Input.GetKeyDown(KeyCode.H))
        {
            gameObject.GetComponent<LifeSystem>().TakeDamage(1);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
                Time.timeScale = 1;
            else
                Time.timeScale = 0;
        }
    }

    private void Shoot(float direction)
    {
        gameObject.GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(0.8f, 1.2f);
        gameObject.GetComponent<AudioSource>().PlayOneShot(gameObject.GetComponent<AudioSource>().clip, UnityEngine.Random.Range(0.05f, 0.1f));
        Quaternion rotation = transform.rotation;
        rotation *= Quaternion.Euler(0, direction, 0);
        GameObject bullet = Instantiate(bulletPrefab, transform.position + transform.forward + Vector3.up * 3f, rotation);
        bullet.GetComponent<BulletController>().damage = 1;
    }

    private void ShootMortar(string input = "mouse")
    {
        if (currentShootIntervalMortar < shootIntervalMortar)
        {
            return;
        }
        gameObject.GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(0.8f, 1.2f);
        gameObject.GetComponent<AudioSource>().PlayOneShot(gameObject.GetComponent<AudioSource>().clip, UnityEngine.Random.Range(0.05f, 0.1f));
        GameObject plane = GameObject.Find("Sea");
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
                bullet.GetComponent<BulletController>().damage = 2;
                currentShootIntervalMortar = 0.0f;
            }
        }
        else
        {
            Vector3 rightStickDirection = new(lastRightStickDirection.x, 0, lastRightStickDirection.y);
            Quaternion rotation = Quaternion.LookRotation(rightStickDirection);
            rotation *= Quaternion.Euler(-5, gameObject.transform.rotation.eulerAngles.y, 0);
            GameObject bullet = Instantiate(bulletPrefab, transform.position + transform.forward + Vector3.up * 3f, rotation);
            bullet.GetComponent<BulletController>().damage = 2;
            currentShootIntervalMortar = 0.0f;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Loot"))
        {
            Destroy(collision.gameObject);
            score += 1;
            if (score > PlayerPrefs.GetInt("highscore", 0))
                PlayerPrefs.SetInt("highscore", score);
            scoreText.GetComponent<TMP_Text>().text = "" + score;
            gameObject.GetComponent<LifeSystem>().Heal(1);
        }
    }
}
