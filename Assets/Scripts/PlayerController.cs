using TMPro;
using Unity.VisualScripting;
using UnityEngine;
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
    private GameObject BGPause;
    private GameObject Resume;
    private GameObject Restart;
    private GameObject MainMenu;
    private GameObject Quit;

    private bool isUsingGamepad = false;

    private bool isFiringDash = false;
    private float dashInterval = 2f;
    private float currentDashInterval = 5f;
    private float lastImageRotation = 0;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        scoreText = GameObject.Find("ScoreText");
        scoreText.GetComponent<TMP_Text>().text = "" + score;
        if (PlayerPrefs.GetInt("highscore", 0) != 0)
            scoreText.GetComponent<TMP_Text>().text += " (" + PlayerPrefs.GetInt("highscore", 0) + ")";
        BGPause = GameObject.Find("BGPause");
        Resume = GameObject.Find("Resume");
        Restart = GameObject.Find("RestartPause");
        MainMenu = GameObject.Find("MainMenuPause");
        Quit = GameObject.Find("QuitPause");

        BGPause.SetActive(false);
        Resume.SetActive(false);
        Restart.SetActive(false);
        MainMenu.SetActive(false);
        Quit.SetActive(false);

    }

    void FixedUpdate()
    {

        if (Gamepad.current != null)
        {
            isUsingGamepad = true;
        }
        else
        {
            isUsingGamepad = false;
        }

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

        Vector3 rotation = horizontal * rotationSpeed * rotationModifier * Time.fixedDeltaTime * transform.up;


        if (acceleration < 0)
        {
            rotation *= -1;
        }

        transform.Rotate(rotation);
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

            if (currentShootIntervalLeft >= shootInterval)
            {
                Shoot(90);
                Shoot(-90);
                currentShootIntervalLeft = 0.0f;
            }
        }


        if (Input.GetKey(KeyCode.Mouse0)) ShootMortar();

        if (Input.GetKeyDown(KeyCode.H))
        {
            gameObject.GetComponent<LifeSystem>().TakeDamage(1);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                BGPause.SetActive(false);
                Resume.SetActive(false);
                Restart.SetActive(false);
                MainMenu.SetActive(false);
                Quit.SetActive(false);
            }
            else
            {

                Time.timeScale = 0;
                BGPause.SetActive(true);
                Resume.SetActive(true);
                Restart.SetActive(true);
                MainMenu.SetActive(true);
                Quit.SetActive(true);
            }
            isFiringDash = false;

        }
        // if (Input.GetKeyDown(KeyCode.R))
        // {
        //     Time.timeScale = 0.5F;
        // }
        // if (Input.GetKeyDown(KeyCode.T))
        // {
        //     Time.timeScale = 1.0F;
        // }
        currentDashInterval += Time.deltaTime;

        float directionRotation = 0;
        if (isUsingGamepad)
        {
            if (Gamepad.current.rightStick.ReadValue().magnitude > 0.1)
            {
                lastRightStickDirection = Gamepad.current.rightStick.ReadValue();
                directionRotation = Mathf.Atan2(lastRightStickDirection.y, lastRightStickDirection.x) * Mathf.Rad2Deg;
            }
        }
        else
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10;
            Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
            mousePos.x -= objectPos.x;
            mousePos.y -= objectPos.y;
            directionRotation = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg - 90;
        }

        GameObject directionImage = GameObject.Find("directionimage");
        if (directionImage != null)
        {
            float newRotation = directionRotation;
            directionImage.transform.RotateAround(transform.position, Vector3.up, lastImageRotation - newRotation);
            lastImageRotation = newRotation;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (currentDashInterval < dashInterval)
            {
                return;
            }
            isFiringDash = true;
            GameObject.Find("directionimage").GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            GameObject.Find("directionimage").GetComponent<SpriteRenderer>().enabled = false;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (isFiringDash)
            {
                gameObject.transform.rotation = Quaternion.AngleAxis(gameObject.transform.rotation.eulerAngles.y - directionRotation, Vector3.up);
                if (acceleration <= 0) acceleration = 0.001f;
                float a = Mathf.Clamp(maxSpeed / acceleration / 80, 2, 8);
                print(a + " ||||| " + Mathf.Clamp(maxSpeed / acceleration / 80, 2, 8));
                acceleration += a / 2;

                currentDashInterval = 0.0f;
                isFiringDash = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // transform.position = new Vector3(-3.85914f, transform.position.y, 52.56866f);
            // transform.rotation = Quaternion.Euler(0, 0, 0);
            acceleration = 0;
        }

    }

    private void Shoot(float direction)
    {
        gameObject.GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);
        gameObject.GetComponent<AudioSource>().PlayOneShot(gameObject.GetComponent<AudioSource>().clip, Random.Range(0.05f, 0.1f));
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
            if (PlayerPrefs.GetInt("highscore", 0) != 0)
                scoreText.GetComponent<TMP_Text>().text += " (" + PlayerPrefs.GetInt("highscore", 0) + ")";
            gameObject.GetComponent<LifeSystem>().Heal(1);
        }
    }
}
