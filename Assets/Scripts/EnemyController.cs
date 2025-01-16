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
        Vector3 direction = (target.transform.position + (target.transform.forward * 3f) - this.gameObject.transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);
        rotation *= Quaternion.Euler(-6, 0, 0);
        GameObject bullet = Instantiate(bulletPrefab, this.gameObject.transform.position + Vector3.up * 3f, rotation);
        bullet.GetComponent<BulletController>().speed = 40.0f;
        bullet.GetComponent<BulletController>().damage = 1;
        currentShootInterval = 0.0f;
    }

    private void GetHit(int damage = 1)
    {
        health -= damage;
        if (health <= 0)
        {
            if (loot)
                Instantiate(loot, this.gameObject.transform.position + Vector3.up * 1f, this.gameObject.transform.rotation);
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
