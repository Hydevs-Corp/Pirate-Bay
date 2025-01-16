using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LifeSystem : MonoBehaviour
{
    private int maxHeart = 6;
    private int startHeart = 6;
    private int currentHealth;
    private int maxHealth;
    private readonly int healthPerHeart = 2;

    public Image[] healthImages;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    [Header("LifeWarning")]
    private float lastTimeCollided = 0.0f;
    private GameObject vignette;
    private GameObject DeathText;

    private GameObject Restart;
    private GameObject MainMenu;
    private GameObject Quit;

    void Start()
    {
        currentHealth = startHeart * healthPerHeart;
        maxHealth = maxHeart * healthPerHeart;
        UpdateHealthUI();
        gameObject.GetComponent<ParticleSystem>().Stop();
        vignette = GameObject.Find("Vignette");
        DeathText = GameObject.Find("DEATH");
        Restart = GameObject.Find("Restart");
        MainMenu = GameObject.Find("MainMenu");
        Quit = GameObject.Find("Quit");
        vignette.SetActive(false);
        Restart.SetActive(false);
        MainMenu.SetActive(false);
        Quit.SetActive(false);

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
            gameObject.GetComponent<PlayerController>().enabled = false;
            gameObject.GetComponent<ParticleSystem>().Play();
            StartCoroutine(Die());
            DeathText.GetComponent<TMP_Text>().text = "Your ship has been destroyed"; //+ at wave nananinanana;
            if (Restart)
                Restart.SetActive(true);
            if (MainMenu)
                MainMenu.SetActive(true);
            if (Quit)
                Quit.SetActive(true);
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
        if (currentHealth < 5)
        {
            if (vignette)
                vignette.SetActive(true);
        }
        else
        {
            if (vignette)
                vignette.SetActive(false);
        }
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
            gameObject.GetComponent<PlayerController>().acceleration = -1;
            if (Time.time - lastTimeCollided < 1.5f)
            {
                return;
            }
            lastTimeCollided = Time.time;
            TakeDamage(1);
        }
    }
}


