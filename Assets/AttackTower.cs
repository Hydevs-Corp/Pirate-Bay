using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public GameObject target;
    public GameObject bulletPrefab;
    public float detectionRadius = 10f;
    public float shootInterval = 2.0f;
    public LayerMask playerLayer;
    public int maxHealth = 6;

    private float currentShootInterval = 0.0f;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (target == null)
        {
            FindClosestPlayer();
        }

        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance <= detectionRadius)
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

        Vector3 direction = (target.transform.position - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);
        GameObject bullet = Instantiate(bulletPrefab, transform.position + Vector3.up * 1.5f, rotation);
        bullet.GetComponent<BulletController>().hasGravity = false;
        bullet.GetComponent<BulletController>().damage = 1;
        currentShootInterval = 0.0f;
    }

    private void FindClosestPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
        float closestDistance = Mathf.Infinity;
        GameObject closestPlayer = null;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPlayer = hitCollider.gameObject;
                }
            }
        }

        target = closestPlayer;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Ajoutez ici le code pour gérer la mort de la tour, par exemple, instancier un effet de destruction, jouer un son, etc.
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cannonball"))
        {
            TakeDamage(1);
            Destroy(collision.gameObject);
        }
    }
}
