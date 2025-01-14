using UnityEngine;

public class BulletController : MonoBehaviour
{
    readonly float speed = 75.0f;
    float lifeTime = 2.0f;
    float currentLifeTime = 0.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position += this.gameObject.transform.forward * (speed * Time.deltaTime);
        this.gameObject.transform.position += Vector3.down * (Mathf.Exp(currentLifeTime * 4f) * Time.deltaTime);
        currentLifeTime += Time.deltaTime;
        if (currentLifeTime >= lifeTime || this.gameObject.transform.position.y < -0.5f)
        {
            Destroy(this.gameObject);
        }
    }
}
