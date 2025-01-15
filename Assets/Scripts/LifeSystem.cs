using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LifeSystem : MonoBehaviour
{
    private int maxHeart = 1;
    public int startHeart = 1;
    public int currentHealth;
    private int maxHealth;
    private int healthPerHeart = 2;

    public Image[] healthImages;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    private float lastTimeCollided = 0.0f;

    void Start()
    {
        currentHealth = startHeart * healthPerHeart;
        maxHealth = maxHeart * healthPerHeart;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth == 0)
        {
            return;
        }
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
        if (currentHealth == 0)
        {
            StartCoroutine(Die());

        }
    }

    IEnumerator Die()
    {
        float elapsedTime = 0;
        float waitTime = 1.5f;
        while (elapsedTime < waitTime)
        {
            transform.Rotate(0, 0, 45 * (Time.deltaTime / waitTime));
            transform.Translate(0, -0.5f * (Time.deltaTime / waitTime), 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        for (int i = 0; i < maxHeart; i++)
        {
            if (i < startHeart)
            {

                healthImages[i].enabled = true;
                int heartHealth = Mathf.Clamp(currentHealth - (i * healthPerHeart), 0, healthPerHeart);

                if (heartHealth == healthPerHeart)
                {
                    healthImages[i].sprite = fullHeart;
                }
                else if (heartHealth > 0)
                {
                    healthImages[i].sprite = halfHeart;
                }
                else
                {
                    healthImages[i].sprite = emptyHeart;
                }
            }
            else
            {
                healthImages[i].enabled = false;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Loot"))
        {
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            TakeDamage(collision.gameObject.GetComponent<BulletController>().damage);

            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Bullet")) { }
        else
        {
            if (Time.time - lastTimeCollided < 3.0f)
            {
                return;
            }
            TakeDamage(1);
            lastTimeCollided = Time.time;
        }
    }
}


