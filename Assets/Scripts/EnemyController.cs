using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    public GameObject target;
    public NavMeshAgent agent;
    public GameObject bulletPrefab;

    public GameObject loot;
    private int health = 6;
    private float shootInterval = 2.0f;
    private float currentShootInterval = 2.0f;

    void Update()
    {

        if (agent && target)
        {

            float distance = Vector3.Distance(gameObject.transform.position, target.transform.position);

            Vector3 left = target.transform.position + target.transform.forward * 20f;
            float distanceToLeft = Vector3.Distance(gameObject.transform.position, left);

            Vector3 right = target.transform.position + target.transform.right * 20f;
            float distanceToRight = Vector3.Distance(gameObject.transform.position, right);


            if (distanceToLeft < distanceToRight)
            {
                agent.SetDestination(left);
            }
            else if (distanceToLeft > distanceToRight)
            {
                agent.SetDestination(right);
            }

            if (distanceToLeft < 40f || distanceToRight < 40f || distance < 40f)
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
        Vector3 direction = (target.transform.position + (target.transform.forward * 3f) - gameObject.transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);
        rotation *= Quaternion.Euler(-3, 0, 0);
        GameObject bullet = Instantiate(bulletPrefab, gameObject.transform.position + Vector3.up * 3f, rotation);
        float distance = Vector3.Distance(gameObject.transform.position, target.transform.position);
        bullet.GetComponent<BulletController>().speed = Mathf.Clamp(distance, 20.0f, 40.0f) * 2;
        bullet.GetComponent<BulletController>().damage = 1;
        currentShootInterval = 0.0f;
    }

    private void GetHit(int damage = 1)
    {
        health -= damage;
        if (health <= 0)
        {
            if (loot)
                Instantiate(loot, gameObject.transform.position + Vector3.up * 1f, gameObject.transform.rotation);
            GameObject.Find("Spawner").GetComponent<WaveManager>().EnemyDied();
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {

            GetHit(collision.gameObject.GetComponent<BulletController>().damage);
            Destroy(collision.gameObject);
            return;
        }
        if (collision.gameObject.CompareTag("Player"))
            GetHit(1);
    }
}
