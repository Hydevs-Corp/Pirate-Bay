using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public float speed = 20.0f;
    public float rotationSpeed = 0.1f;

    private Transform target;

    void Start()
    {
        target = GameObject.Find("Player").transform;
    }

    void Update()
    {

        if (target)
        {
            // CALCULATE DISTANCE
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance > 25.0f)
            {
                if (speed < 25.0f)
                {
                    speed += 0.01f;
                }
                Vector3 direction = target.position - transform.position;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime / 10);

                transform.Translate(0, 0, speed * Time.deltaTime);
            }
            else
            {
                if (speed > 15f && speed < 25.0f)
                {
                    speed -= 0.01f;
                }


                Vector3 direction = target.position - transform.position;

                Quaternion toRotation = Quaternion.LookRotation(direction);
                // rotate the rotation 180 degrees
                toRotation *= Quaternion.Euler(0, 85, 0);

                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime / 10);

                transform.Translate(0, 0, speed * Time.deltaTime);
            }

        }

    }

    void OnCollisionEnter(Collision collision)
    {
        speed = 5.0f;
    }
}
