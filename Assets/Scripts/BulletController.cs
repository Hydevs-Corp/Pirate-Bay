using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BulletController : MonoBehaviour
{

    public bool hasGravity = true;
    public float speed = 75.0f;
    float lifeTime = 2.0f;
    float currentLifeTime = 0.0f;
    public GameObject WaterDropPrefab;

    public int damage = 10;

    void Update()
    {
        this.gameObject.transform.position += this.gameObject.transform.forward * (speed * Time.deltaTime);
        if (hasGravity) this.gameObject.transform.position += Vector3.down * (Mathf.Exp(currentLifeTime * 2.5f) * Time.deltaTime);
        currentLifeTime += Time.deltaTime;
        if (currentLifeTime >= lifeTime || this.gameObject.transform.position.y < 1.4f)
        {
            Instantiate(WaterDropPrefab, this.gameObject.transform.position, Quaternion.identity * Quaternion.Euler(-90, 0, 0));
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Bullet")) return;
        if (collision.gameObject.CompareTag("EnemyBullet")) return;
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (gameObject.CompareTag("Bullet"))
                Instantiate(WaterDropPrefab, this.gameObject.transform.position, Quaternion.identity * Quaternion.Euler(-90, 0, 0));
            return;
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameObject.CompareTag("EnemyBullet"))
                Instantiate(WaterDropPrefab, this.gameObject.transform.position, Quaternion.identity * Quaternion.Euler(-90, 0, 0));
            return;
        }
        ;
        if (collision.gameObject.name == "Sea") return;
        Instantiate(WaterDropPrefab, this.gameObject.transform.position, Quaternion.identity * Quaternion.Euler(-90, 0, 0));
        Destroy(this.gameObject);
    }
}
