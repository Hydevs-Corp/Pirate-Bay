using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    public GameObject target;
    public NavMeshAgent agent;
    public GameObject bulletPrefab;

    private float health = 100;
    private float shootInterval = 2.0f;
    private float currentShootInterval = 2.0f;

    void Update()
    {

        if (agent && target)
        {

            float distance = Vector3.Distance(this.gameObject.transform.position, target.transform.position);

            Vector3 left = target.transform.position + target.transform.forward * 20f;
            float distanceToLeft = Vector3.Distance(this.gameObject.transform.position, left);

            Vector3 right = target.transform.position + target.transform.right * 20f;
            float distanceToRight = Vector3.Distance(this.gameObject.transform.position, right);


            if (distanceToLeft < distanceToRight)
            {
                agent.SetDestination(left);
            }
            else if (distanceToLeft > distanceToRight)
            {
                agent.SetDestination(right);
            }

            if (distanceToLeft < 20f || distanceToRight < 20f || distance < 20f)
            {
                Shoot();
            }
        }

    }

    private void Shoot()
    {
        if (currentShootInterval < shootInterval)
        {
            currentShootInterval += Time.deltaTime;
            return;
        }
        Vector3 direction = (target.transform.position + (target.transform.forward * 5f) - this.gameObject.transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);
        Instantiate(bulletPrefab, this.gameObject.transform.position + Vector3.up * 3f, rotation);
        currentShootInterval = 0.0f;
    }

    private void GetHit(float damage = 35)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {

            GetHit(collision.gameObject.GetComponent<BulletController>().damage);
            Destroy(collision.gameObject);

        }
    }
}
