using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    public float speed = 20.0f;
    public float rotationSpeed = 15f;
    public GameObject target;
    public NavMeshAgent agent;


    void Update()
    {

        if (agent && target)
        {
            agent.SetDestination(target.transform.position);
        }
        else
        {

        }
        // if (target)
        // {
        //     float distance = Vector3.Distance(this.gameObject.transform.position, target.transform.position);
        //     if (distance > 25.0f)
        //     {
        //         if (speed < 25.0f)
        //         {
        //             speed += 0.01f;
        //         }
        //         Vector3 direction = target.transform.position - this.gameObject.transform.position;

        //         transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime / 10);

        //         transform.Translate(0, 0, speed * Time.deltaTime);
        //     }
        //     else
        //     {
        //         if (speed > 15f && speed < 25.0f)
        //         {
        //             speed -= 0.01f;
        //         }


        //         Vector3 direction = target.transform.position - this.gameObject.transform.position;

        //         Quaternion toRotation = Quaternion.LookRotation(direction);
        //         toRotation *= Quaternion.Euler(0, 85, 0);

        //         transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime / 10);

        //         transform.Translate(0, 0, speed * Time.deltaTime);
        //     }

        // }
        // else
        // {
        //     Debug.LogWarning("No target assigned.");
        // }

    }

    void OnCollisionEnter(Collision collision)
    {
        speed = 5.0f;
    }
}
