using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTowerController : MonoBehaviour
{
    private GameObject target;
    public GameObject bulletPrefab;
    public float detectionRadius = 10f;
    public float shootInterval = 2.0f;
    public LayerMask playerLayer;
    public int maxHealth = 6;

    public GameObject bulletSpawnPoint;
    private float currentShootInterval = 0.0f;
    private int currentHealth;
    private bool canShoot = true;
    private bool isDead = false;
    private float regenerationInterval = 30.0f;
    private float timeSinceLastRegen = 0.0f;

    void Start()
    {
        currentHealth = maxHealth;
        gameObject.GetComponent<ParticleSystem>().Stop();
        target = GameObject.Find("Player");
        StartCoroutine(RegenerateHealth());
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }

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

        if (target == null)
        {
            FindClosestPlayer();
            if (target == null)
            {
                return;
            }
        }

        Vector3 direction = (target.transform.position - bulletSpawnPoint.transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, rotation);
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
            gameObject.GetComponent<ParticleSystem>().Play();
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        isDead = true;
        yield return new WaitForSeconds(2); // Attendre 2 secondes avant de détruire la tour pour permettre à l'effet de destruction de se jouer
        // Destroy(gameObject);
        yield return new WaitForSeconds(2); // Attendre 1 minute et 30 secondes avant de respawn
        Respawn();
    }

    private void Respawn()
    {
        currentHealth = maxHealth;
        canShoot = true;
        isDead = false;
        gameObject.SetActive(true);
        gameObject.GetComponent<ParticleSystem>().Stop();
        StartCoroutine(RegenerateHealth());
    }

    private IEnumerator RegenerateHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(regenerationInterval);
            if (currentHealth < maxHealth && currentHealth > 0)
            {
                currentHealth += 1;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            return;
        }
    }
}
