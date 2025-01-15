using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 75.0f;
    float lifeTime = 2.0f;
    float currentLifeTime = 0.0f;

    public int damage = 10;

    void Start()
    {
        gameObject.GetComponent<ParticleSystem>().Stop();
    }

    void Update()
    {
        this.gameObject.transform.position += this.gameObject.transform.forward * (speed * Time.deltaTime);
        this.gameObject.transform.position += Vector3.down * (Mathf.Exp(currentLifeTime * 2.5f) * Time.deltaTime);
        currentLifeTime += Time.deltaTime;
        if (currentLifeTime >= lifeTime || this.gameObject.transform.position.y < 1.4f)
        {
            gameObject.GetComponent<ParticleSystem>().Play();
            // Destroy(this.gameObject);

        }
    }

    void OnTriggerEnter(Collider other)
    {
    }
}
